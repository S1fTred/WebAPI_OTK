using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace WebAPI_OTK.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExportController : ControllerBase
{
    private readonly OtkDbContext _context;

    public ExportController(OtkDbContext context)
    {
        _context = context;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    // Экспорт каталога ДСЕ в Excel
    [HttpGet("dce/excel")]
    public async Task<IActionResult> ExportDCEToExcel([FromQuery] int? изделиеId = null)
    {
        var query = _context.ДСЕ
            .Include(d => d.Изделие)
            .AsQueryable();

        if (изделиеId.HasValue)
        {
            query = query.Where(d => d.ИзделиеID == изделиеId.Value);
        }

        var dceList = await query.OrderBy(d => d.Код).ToListAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Каталог ДСЕ");

        // Заголовки
        worksheet.Cell(1, 1).Value = "Код";
        worksheet.Cell(1, 2).Value = "Наименование";
        worksheet.Cell(1, 3).Value = "Изделие";
        worksheet.Cell(1, 4).Value = "Чертеж";

        // Стиль заголовков
        var headerRange = worksheet.Range(1, 1, 1, 4);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

        // Данные
        int row = 2;
        foreach (var dce in dceList)
        {
            worksheet.Cell(row, 1).Value = dce.Код;
            worksheet.Cell(row, 2).Value = dce.Наименование ?? "";
            worksheet.Cell(row, 3).Value = dce.Изделие?.Наименование ?? "";
            worksheet.Cell(row, 4).Value = dce.Чертеж ?? "";
            row++;
        }

        // Автоподбор ширины колонок
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return File(stream.ToArray(), 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"DCE_Catalog_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
    }

    // Экспорт каталога ДСЕ в PDF
    [HttpGet("dce/pdf")]
    public async Task<IActionResult> ExportDCEToPDF([FromQuery] int? изделиеId = null)
    {
        var query = _context.ДСЕ
            .Include(d => d.Изделие)
            .AsQueryable();

        if (изделиеId.HasValue)
        {
            query = query.Where(d => d.ИзделиеID == изделиеId.Value);
        }

        var dceList = await query.OrderBy(d => d.Код).ToListAsync();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .AlignCenter()
                    .Text("Каталог ДСЕ")
                    .FontSize(20)
                    .Bold();

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        // Заголовки
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Код").Bold();
                            header.Cell().Element(CellStyle).Text("Наименование").Bold();
                            header.Cell().Element(CellStyle).Text("Изделие").Bold();
                            header.Cell().Element(CellStyle).Text("Чертеж").Bold();
                        });

                        // Данные
                        foreach (var dce in dceList)
                        {
                            table.Cell().Element(CellStyle).Text(dce.Код);
                            table.Cell().Element(CellStyle).Text(dce.Наименование ?? "");
                            table.Cell().Element(CellStyle).Text(dce.Изделие?.Наименование ?? "");
                            table.Cell().Element(CellStyle).Text(dce.Чертеж ?? "");
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Страница ");
                        x.CurrentPageNumber();
                        x.Span(" из ");
                        x.TotalPages();
                    });
            });
        });

        var pdfBytes = document.GeneratePdf();
        return File(pdfBytes, "application/pdf", $"DCE_Catalog_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
    }

    // Экспорт маршрутных листов в Excel
    [HttpGet("routelists/excel")]
    public async Task<IActionResult> ExportRouteListsToExcel(
        [FromQuery] string? номерМЛ = null,
        [FromQuery] int? изделиеId = null,
        [FromQuery] int? дсеId = null,
        [FromQuery] bool? закрыт = null)
    {
        var query = _context.МЛ
            .Include(m => m.Изделие)
            .Include(m => m.ДСЕ)
            .AsQueryable();

        if (!string.IsNullOrEmpty(номерМЛ))
            query = query.Where(m => m.НомерМЛ.Contains(номерМЛ));
        if (изделиеId.HasValue)
            query = query.Where(m => m.ИзделиеID == изделиеId.Value);
        if (дсеId.HasValue)
            query = query.Where(m => m.ДСЕID == дсеId.Value);
        if (закрыт.HasValue)
            query = query.Where(m => m.Закрыт == закрыт.Value);

        var mlList = await query.OrderByDescending(m => m.ДатаСоздания).ToListAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Маршрутные листы");

        // Заголовки
        worksheet.Cell(1, 1).Value = "Номер МЛ";
        worksheet.Cell(1, 2).Value = "Изделие";
        worksheet.Cell(1, 3).Value = "ДСЕ";
        worksheet.Cell(1, 4).Value = "Дата создания";
        worksheet.Cell(1, 5).Value = "Статус";
        worksheet.Cell(1, 6).Value = "Количество ОТК";
        worksheet.Cell(1, 7).Value = "Количество брака";

        var headerRange = worksheet.Range(1, 1, 1, 7);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

        int row = 2;
        foreach (var ml in mlList)
        {
            worksheet.Cell(row, 1).Value = ml.НомерМЛ;
            worksheet.Cell(row, 2).Value = ml.Изделие?.Наименование ?? "";
            worksheet.Cell(row, 3).Value = ml.ДСЕ?.Код ?? "";
            worksheet.Cell(row, 4).Value = ml.ДатаСоздания?.ToString("dd.MM.yyyy") ?? "";
            worksheet.Cell(row, 5).Value = ml.Закрыт == true ? "Закрыт" : "Открыт";
            worksheet.Cell(row, 6).Value = ml.КоличествоОТК ?? 0;
            worksheet.Cell(row, 7).Value = ml.КоличествоБрак ?? 0;
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Route_Lists_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
    }

    // Экспорт маршрутных листов в PDF
    [HttpGet("routelists/pdf")]
    public async Task<IActionResult> ExportRouteListsToPDF(
        [FromQuery] string? номерМЛ = null,
        [FromQuery] int? изделиеId = null,
        [FromQuery] int? дсеId = null,
        [FromQuery] bool? закрыт = null)
    {
        var query = _context.МЛ
            .Include(m => m.Изделие)
            .Include(m => m.ДСЕ)
            .AsQueryable();

        if (!string.IsNullOrEmpty(номерМЛ))
            query = query.Where(m => m.НомерМЛ.Contains(номерМЛ));
        if (изделиеId.HasValue)
            query = query.Where(m => m.ИзделиеID == изделиеId.Value);
        if (дсеId.HasValue)
            query = query.Where(m => m.ДСЕID == дсеId.Value);
        if (закрыт.HasValue)
            query = query.Where(m => m.Закрыт == закрыт.Value);

        var mlList = await query.OrderByDescending(m => m.ДатаСоздания).ToListAsync();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(1.5f, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header()
                    .AlignCenter()
                    .Text("Маршрутные листы")
                    .FontSize(18)
                    .Bold();

                page.Content()
                    .PaddingVertical(0.5f, Unit.Centimetre)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Номер МЛ").Bold();
                            header.Cell().Element(CellStyle).Text("Изделие").Bold();
                            header.Cell().Element(CellStyle).Text("ДСЕ").Bold();
                            header.Cell().Element(CellStyle).Text("Дата").Bold();
                            header.Cell().Element(CellStyle).Text("Статус").Bold();
                            header.Cell().Element(CellStyle).Text("Количество ОТК").Bold();
                            header.Cell().Element(CellStyle).Text("Количество брака").Bold();
                        });

                        foreach (var ml in mlList)
                        {
                            table.Cell().Element(CellStyle).Text(ml.НомерМЛ);
                            table.Cell().Element(CellStyle).Text(ml.Изделие?.Наименование ?? "");
                            table.Cell().Element(CellStyle).Text(ml.ДСЕ?.Код ?? "");
                            table.Cell().Element(CellStyle).Text(ml.ДатаСоздания?.ToString("dd.MM.yyyy") ?? "");
                            table.Cell().Element(CellStyle).Text(ml.Закрыт == true ? "Закрыт" : "Открыт");
                            table.Cell().Element(CellStyle).Text((ml.КоличествоОТК ?? 0).ToString());
                            table.Cell().Element(CellStyle).Text((ml.КоличествоБрак ?? 0).ToString());
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Страница ");
                        x.CurrentPageNumber();
                        x.Span(" из ");
                        x.TotalPages();
                    });
            });
        });

        var pdfBytes = document.GeneratePdf();
        return File(pdfBytes, "application/pdf", $"Route_Lists_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
    }

    // Экспорт премиальных коэффициентов в Excel
    [HttpGet("coefficients/excel")]
    public async Task<IActionResult> ExportCoefficientsToExcel([FromQuery] bool? толькоАктивные = null)
    {
        var query = _context.ПремиальныеКоэффициенты
            .Include(k => k.Изделие)
            .Include(k => k.ДСЕ)
            .Include(k => k.ТипОперации)
            .AsQueryable();

        if (толькоАктивные == true)
        {
            var currentDate = DateTime.Now.Date;
            query = query.Where(k => k.ДатаНачала <= currentDate && 
                                    (k.ДатаОкончания == null || k.ДатаОкончания >= currentDate));
        }

        var koefList = await query.OrderBy(k => k.Наименование).ToListAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Премиальные коэффициенты");

        // Заголовки
        worksheet.Cell(1, 1).Value = "Наименование";
        worksheet.Cell(1, 2).Value = "Коэффициент";
        worksheet.Cell(1, 3).Value = "Дата начала";
        worksheet.Cell(1, 4).Value = "Дата окончания";
        worksheet.Cell(1, 5).Value = "Изделие";
        worksheet.Cell(1, 6).Value = "ДСЕ";
        worksheet.Cell(1, 7).Value = "Тип операции";
        worksheet.Cell(1, 8).Value = "Статус";

        var headerRange = worksheet.Range(1, 1, 1, 8);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

        int row = 2;
        var currentDate2 = DateTime.Now.Date;
        foreach (var koef in koefList)
        {
            var isActive = koef.ДатаНачала <= currentDate2 && 
                          (koef.ДатаОкончания == null || koef.ДатаОкончания >= currentDate2);
            
            worksheet.Cell(row, 1).Value = koef.Наименование;
            worksheet.Cell(row, 2).Value = koef.Коэффициент;
            worksheet.Cell(row, 3).Value = koef.ДатаНачала.ToString("dd.MM.yyyy");
            worksheet.Cell(row, 4).Value = koef.ДатаОкончания?.ToString("dd.MM.yyyy") ?? "Бессрочно";
            worksheet.Cell(row, 5).Value = koef.Изделие?.Наименование ?? "Все";
            worksheet.Cell(row, 6).Value = koef.ДСЕ?.Код ?? "Все";
            worksheet.Cell(row, 7).Value = koef.ТипОперации?.Наименование ?? "Все";
            worksheet.Cell(row, 8).Value = isActive ? "Активный" : "Неактивный";
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Premium_Coefficients_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
    }

    // Вспомогательный метод для стилизации ячеек
    private static IContainer CellStyle(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(5);
    }

    // Экспорт премиальных коэффициентов в PDF
    [HttpGet("coefficients/pdf")]
    public async Task<IActionResult> ExportCoefficientsToPDF([FromQuery] bool? толькоАктивные = null)
    {
        var query = _context.ПремиальныеКоэффициенты
            .Include(k => k.Изделие)
            .Include(k => k.ДСЕ)
            .Include(k => k.ТипОперации)
            .AsQueryable();

        if (толькоАктивные == true)
        {
            var currentDate = DateTime.Now.Date;
            query = query.Where(k => k.ДатаНачала <= currentDate && 
                                    (k.ДатаОкончания == null || k.ДатаОкончания >= currentDate));
        }

        var koefList = await query.OrderBy(k => k.Наименование).ToListAsync();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(1.5f, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header()
                    .AlignCenter()
                    .Text("Премиальные коэффициенты")
                    .FontSize(18)
                    .Bold();

                page.Content()
                    .PaddingVertical(0.5f, Unit.Centimetre)
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1.5f);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Наименование").Bold();
                            header.Cell().Element(CellStyle).Text("Коэф.").Bold();
                            header.Cell().Element(CellStyle).Text("Дата начала").Bold();
                            header.Cell().Element(CellStyle).Text("Дата окончания").Bold();
                            header.Cell().Element(CellStyle).Text("Изделие").Bold();
                            header.Cell().Element(CellStyle).Text("ДСЕ").Bold();
                            header.Cell().Element(CellStyle).Text("Тип операции").Bold();
                            header.Cell().Element(CellStyle).Text("Статус").Bold();
                        });

                        var currentDate2 = DateTime.Now.Date;
                        foreach (var koef in koefList)
                        {
                            var isActive = koef.ДатаНачала <= currentDate2 && 
                                          (koef.ДатаОкончания == null || koef.ДатаОкончания >= currentDate2);
                            
                            table.Cell().Element(CellStyle).Text(koef.Наименование);
                            table.Cell().Element(CellStyle).Text(koef.Коэффициент.ToString("F2"));
                            table.Cell().Element(CellStyle).Text(koef.ДатаНачала.ToString("dd.MM.yyyy"));
                            table.Cell().Element(CellStyle).Text(koef.ДатаОкончания?.ToString("dd.MM.yyyy") ?? "Бессрочно");
                            table.Cell().Element(CellStyle).Text(koef.Изделие?.Наименование ?? "Все");
                            table.Cell().Element(CellStyle).Text(koef.ДСЕ?.Код ?? "Все");
                            table.Cell().Element(CellStyle).Text(koef.ТипОперации?.Наименование ?? "Все");
                            table.Cell().Element(CellStyle).Text(isActive ? "Активный" : "Неактивный");
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Страница ");
                        x.CurrentPageNumber();
                        x.Span(" из ");
                        x.TotalPages();
                    });
            });
        });

        var pdfBytes = document.GeneratePdf();
        return File(pdfBytes, "application/pdf", $"Premium_Coefficients_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
    }

    // Экспорт конкретного МЛ с операциями в PDF
    [HttpGet("routelist/{id}/excel")]
    public async Task<IActionResult> ExportRouteListDetailToExcel(int id)
    {
        var ml = await _context.МЛ
            .AsNoTracking()
            .Include(m => m.Изделие)
            .Include(m => m.ДСЕ)
            .Include(m => m.Сотрудник)
            .FirstOrDefaultAsync(m => m.ID == id);

        if (ml == null)
        {
            return NotFound();
        }

        var operationRows = await BuildRouteListOperationExportRowsAsync(ml);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Маршрутный лист");

        worksheet.Cell(1, 1).Value = $"Маршрутный лист № {ml.НомерМЛ}";
        worksheet.Range(1, 1, 1, 14).Merge();
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 16;

        worksheet.Cell(3, 1).Value = "Дата создания";
        worksheet.Cell(3, 2).Value = ml.ДатаСоздания?.ToString("dd.MM.yyyy") ?? "-";
        worksheet.Cell(4, 1).Value = "Изделие";
        worksheet.Cell(4, 2).Value = ml.Изделие?.Наименование ?? "-";
        worksheet.Cell(5, 1).Value = "ДСЕ";
        worksheet.Cell(5, 2).Value = ml.ДСЕ?.Код ?? "-";
        worksheet.Cell(6, 1).Value = "Статус";
        worksheet.Cell(6, 2).Value = ml.Закрыт == true ? "Закрыт" : "Открыт";
        worksheet.Cell(7, 1).Value = "Сотрудник ОТК";
        worksheet.Cell(7, 2).Value = ml.Сотрудник?.ФИО ?? "-";
        worksheet.Cell(8, 1).Value = "Количество ОТК";
        worksheet.Cell(8, 2).Value = ml.КоличествоОТК ?? 0;
        worksheet.Cell(9, 1).Value = "Количество брака";
        worksheet.Cell(9, 2).Value = ml.КоличествоБрак ?? 0;

        var infoRange = worksheet.Range(3, 1, 9, 2);
        infoRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        infoRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        worksheet.Range(3, 1, 9, 1).Style.Font.Bold = true;

        const int operationsHeaderRow = 11;
        worksheet.Cell(operationsHeaderRow, 1).Value = "Тип операции";
        worksheet.Cell(operationsHeaderRow, 2).Value = "Сотрудник";
        worksheet.Cell(operationsHeaderRow, 3).Value = "Кол-во";
        worksheet.Cell(operationsHeaderRow, 4).Value = "Норма, ч";
        worksheet.Cell(operationsHeaderRow, 5).Value = "Тариф";
        worksheet.Cell(operationsHeaderRow, 6).Value = "Цена, ₽/ч";
        worksheet.Cell(operationsHeaderRow, 7).Value = "Дата исп.";
        worksheet.Cell(operationsHeaderRow, 8).Value = "Дата закр.";
        worksheet.Cell(operationsHeaderRow, 9).Value = "Базовая сумма, ₽";
        worksheet.Cell(operationsHeaderRow, 10).Value = "Коэф. сделки";
        worksheet.Cell(operationsHeaderRow, 11).Value = "Надбавка, ₽";
        worksheet.Cell(operationsHeaderRow, 12).Value = "Коэф. премии";
        worksheet.Cell(operationsHeaderRow, 13).Value = "Премия, ₽";
        worksheet.Cell(operationsHeaderRow, 14).Value = "Итого, ₽";

        var headerRange = worksheet.Range(operationsHeaderRow, 1, operationsHeaderRow, 14);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        var row = operationsHeaderRow + 1;
        foreach (var op in operationRows)
        {
            worksheet.Cell(row, 1).Value = op.OperationType;
            worksheet.Cell(row, 2).Value = op.Employee;
            worksheet.Cell(row, 3).Value = op.Quantity;
            worksheet.Cell(row, 4).Value = op.TimeStandardHours;
            worksheet.Cell(row, 5).Value = op.Tariff;
            worksheet.Cell(row, 6).Value = op.PricePerHour;
            worksheet.Cell(row, 7).Value = op.ExecutionDate?.ToString("dd.MM.yyyy HH:mm") ?? "-";
            worksheet.Cell(row, 8).Value = op.ClosureDate?.ToString("dd.MM.yyyy HH:mm") ?? "-";
            worksheet.Cell(row, 9).Value = op.BaseAmount;
            worksheet.Cell(row, 10).Value = op.DealCoefficient;
            worksheet.Cell(row, 11).Value = op.SurchargeAmount;
            worksheet.Cell(row, 12).Value = op.PremiumCoefficient;
            worksheet.Cell(row, 13).Value = op.PremiumAmount;
            worksheet.Cell(row, 14).Value = op.TotalAmount;
            row++;
        }

        if (operationRows.Any())
        {
            var totalsRow = row;
            worksheet.Cell(totalsRow, 1).Value = "Итого";
            worksheet.Cell(totalsRow, 3).Value = operationRows.Sum(x => x.Quantity);
            worksheet.Cell(totalsRow, 9).Value = operationRows.Sum(x => x.BaseAmount);
            worksheet.Cell(totalsRow, 11).Value = operationRows.Sum(x => x.SurchargeAmount);
            worksheet.Cell(totalsRow, 13).Value = operationRows.Sum(x => x.PremiumAmount);
            worksheet.Cell(totalsRow, 14).Value = operationRows.Sum(x => x.TotalAmount);

            var operationsRange = worksheet.Range(operationsHeaderRow + 1, 1, totalsRow, 14);
            operationsRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            operationsRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            var totalsRange = worksheet.Range(totalsRow, 1, totalsRow, 14);
            totalsRange.Style.Font.Bold = true;
            totalsRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        worksheet.Column(4).Style.NumberFormat.Format = "0.00";
        worksheet.Column(6).Style.NumberFormat.Format = "#,##0.00";
        worksheet.Column(9).Style.NumberFormat.Format = "#,##0.00";
        worksheet.Column(10).Style.NumberFormat.Format = "0.00";
        worksheet.Column(11).Style.NumberFormat.Format = "#,##0.00";
        worksheet.Column(12).Style.NumberFormat.Format = "0.00";
        worksheet.Column(13).Style.NumberFormat.Format = "#,##0.00";
        worksheet.Column(14).Style.NumberFormat.Format = "#,##0.00";
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return File(
            stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"ML_{ml.НомерМЛ}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
    }

    [HttpGet("routelist/{id}/pdf")]
    public async Task<IActionResult> ExportRouteListDetailToPDF(int id)
    {
        var ml = await _context.МЛ
            .AsNoTracking()
            .Include(m => m.Изделие)
            .Include(m => m.ДСЕ)
            .Include(m => m.Сотрудник)
            .FirstOrDefaultAsync(m => m.ID == id);

        if (ml == null)
        {
            return NotFound();
        }

        var operationRows = await BuildRouteListOperationExportRowsAsync(ml);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(1.2f, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(8));

                page.Header()
                    .Column(column =>
                    {
                        column.Item().AlignCenter().Text($"Маршрутный лист № {ml.НомерМЛ}").FontSize(18).Bold();
                        column.Item().AlignCenter().Text($"Дата создания: {ml.ДатаСоздания?.ToString("dd.MM.yyyy") ?? "-"}").FontSize(12);
                    });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        // Информация о МЛ
                        column.Item().Element(container =>
                        {
                            container.Border(1).BorderColor(Colors.Grey.Medium).Padding(10).Column(col =>
                            {
                                col.Item().Text("Информация о маршрутном листе").FontSize(14).Bold();
                                col.Item().PaddingTop(5).Row(row =>
                                {
                                    row.RelativeItem().Text($"Изделие: {ml.Изделие?.Наименование ?? "-"}");
                                });
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Text($"ДСЕ: {ml.ДСЕ?.Код ?? "-"}");
                                });
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Text($"Статус: {(ml.Закрыт == true ? "Закрыт" : "Открыт")}");
                                });
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Text($"Сотрудник ОТК: {ml.Сотрудник?.ФИО ?? "-"}");
                                });
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Text($"Количество ОТК: {ml.КоличествоОТК ?? 0}");
                                });
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Text($"Количество брака: {ml.КоличествоБрак ?? 0}");
                                });
                            });
                        });

                        // Операции
                        column.Item().PaddingTop(15).Text("Операции и расчёт").FontSize(13).Bold();
                        
                        if (operationRows.Any())
                        {
                            column.Item().PaddingTop(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2.4f);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1.1f);
                                    columns.RelativeColumn(1.1f);
                                    columns.RelativeColumn(1.3f);
                                    columns.RelativeColumn(1.6f);
                                    columns.RelativeColumn(1.6f);
                                    columns.RelativeColumn(1.4f);
                                    columns.RelativeColumn(1.1f);
                                    columns.RelativeColumn(1.4f);
                                    columns.RelativeColumn(1.1f);
                                    columns.RelativeColumn(1.4f);
                                    columns.RelativeColumn(1.4f);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CompactCellStyle).Text("Тип операции").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("Сотрудник").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("Кол-во").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("Норма, ч").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("Тариф").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("Цена, ₽/ч").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("Дата исп.").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("Дата закр.").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("База, ₽").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("Ксд").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("Надб., ₽").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("Кпрем").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("Прем., ₽").Bold();
                                    header.Cell().Element(CompactCellStyle).Text("Итого, ₽").Bold();
                                });

                                foreach (var op in operationRows)
                                {
                                    table.Cell().Element(CompactCellStyle).Text(op.OperationType);
                                    table.Cell().Element(CompactCellStyle).Text(op.Employee);
                                    table.Cell().Element(CompactCellStyle).Text(op.Quantity.ToString());
                                    table.Cell().Element(CompactCellStyle).Text(op.TimeStandardHours.ToString("0.00"));
                                    table.Cell().Element(CompactCellStyle).Text(op.Tariff);
                                    table.Cell().Element(CompactCellStyle).Text(op.PricePerHour.ToString("#,##0.00"));
                                    table.Cell().Element(CompactCellStyle).Text(op.ExecutionDate?.ToString("dd.MM.yyyy") ?? "-");
                                    table.Cell().Element(CompactCellStyle).Text(op.ClosureDate?.ToString("dd.MM.yyyy") ?? "-");
                                    table.Cell().Element(CompactCellStyle).Text(op.BaseAmount.ToString("#,##0.00"));
                                    table.Cell().Element(CompactCellStyle).Text(op.DealCoefficient.ToString("0.00"));
                                    table.Cell().Element(CompactCellStyle).Text(op.SurchargeAmount.ToString("#,##0.00"));
                                    table.Cell().Element(CompactCellStyle).Text(op.PremiumCoefficient.ToString("0.00"));
                                    table.Cell().Element(CompactCellStyle).Text(op.PremiumAmount.ToString("#,##0.00"));
                                    table.Cell().Element(CompactCellStyle).Text(op.TotalAmount.ToString("#,##0.00"));
                                }
                            });

                            column.Item().PaddingTop(10).Element(container =>
                            {
                                container.Border(1).BorderColor(Colors.Grey.Lighten1).Padding(6).Row(row =>
                                {
                                    row.RelativeItem().Text($"Операций: {operationRows.Count}");
                                    row.RelativeItem().Text($"Кол-во: {operationRows.Sum(x => x.Quantity)}");
                                    row.RelativeItem().Text($"База: {operationRows.Sum(x => x.BaseAmount):#,##0.00} ₽");
                                    row.RelativeItem().Text($"Надбавка: {operationRows.Sum(x => x.SurchargeAmount):#,##0.00} ₽");
                                    row.RelativeItem().Text($"Премия: {operationRows.Sum(x => x.PremiumAmount):#,##0.00} ₽");
                                    row.RelativeItem().AlignRight().Text($"Итого: {operationRows.Sum(x => x.TotalAmount):#,##0.00} ₽").Bold();
                                });
                            });
                        }
                        else
                        {
                            column.Item().PaddingTop(10).Text("Операции отсутствуют").Italic();
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Страница ");
                        x.CurrentPageNumber();
                        x.Span(" из ");
                        x.TotalPages();
                    });
            });
        });

        var pdfBytes = document.GeneratePdf();
        return File(pdfBytes, "application/pdf", $"ML_{ml.НомерМЛ}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
    }

    private async Task<List<RouteListOperationExportRow>> BuildRouteListOperationExportRowsAsync(МЛ ml)
    {
        var operations = await _context.Операция_МЛ
            .AsNoTracking()
            .Include(o => o.ТипОперации)
            .Include(o => o.Сотрудник)
            .Where(o => o.МЛID == ml.ID)
            .OrderBy(o => o.ДатаИсполнения)
            .ToListAsync();

        var coefficients = await _context.ПремиальныеКоэффициенты
            .AsNoTracking()
            .Where(k => k.ИзделиеID == ml.ИзделиеID && k.ДСЕID == ml.ДСЕID)
            .ToListAsync();

        var result = new List<RouteListOperationExportRow>(operations.Count);

        foreach (var op in operations)
        {
            var quantity = op.Количество ?? 0;
            var timeStandardHours = op.НормаВремениЧас ?? 0m;
            var pricePerHour = op.ЦенаЗаЧас ?? 0m;
            var baseAmount = quantity * timeStandardHours * pricePerHour;
            var calculationMoment = op.ДатаЗакрытия ?? DateTime.Now;
            var dealCoefficient = coefficients
                .Where(k => k.ТипОперацииID == op.ТипОперацииID)
                .Where(k => k.ДатаНачала <= calculationMoment && (k.ДатаОкончания == null || k.ДатаОкончания >= calculationMoment))
                .Select(k => (decimal?)k.Коэффициент)
                .FirstOrDefault() ?? 1.0m;

            var surchargeAmount = (dealCoefficient - 1m) * baseAmount;
            var premiumCoefficient = !(op.ДатаЗакрытия.HasValue && op.ДатаИсполнения.HasValue && op.ДатаЗакрытия > op.ДатаИсполнения)
                ? 0.8m
                : 0m;
            var premiumAmount = premiumCoefficient * (baseAmount + surchargeAmount);
            var totalAmount = baseAmount + surchargeAmount + premiumAmount;

            result.Add(new RouteListOperationExportRow
            {
                OperationType = op.ТипОперации?.Наименование ?? "-",
                Employee = FormatEmployeeDisplay(op.Сотрудник),
                Quantity = quantity,
                TimeStandardHours = timeStandardHours,
                Tariff = string.IsNullOrWhiteSpace(op.НазваниеТарифа) ? "-" : op.НазваниеТарифа,
                PricePerHour = pricePerHour,
                ExecutionDate = op.ДатаИсполнения,
                ClosureDate = op.ДатаЗакрытия,
                BaseAmount = baseAmount,
                DealCoefficient = dealCoefficient,
                SurchargeAmount = surchargeAmount,
                PremiumCoefficient = premiumCoefficient,
                PremiumAmount = premiumAmount,
                TotalAmount = totalAmount
            });
        }

        return result;
    }

    private static string FormatEmployeeDisplay(Сотрудник? employee)
    {
        if (employee == null)
        {
            return "-";
        }

        if (string.IsNullOrWhiteSpace(employee.ТабельныйНомер))
        {
            return employee.ФИО ?? "-";
        }

        if (string.IsNullOrWhiteSpace(employee.ФИО))
        {
            return employee.ТабельныйНомер;
        }

        return $"{employee.ТабельныйНомер} {employee.ФИО}";
    }

    private sealed class RouteListOperationExportRow
    {
        public string OperationType { get; init; } = "-";
        public string Employee { get; init; } = "-";
        public int Quantity { get; init; }
        public decimal TimeStandardHours { get; init; }
        public string Tariff { get; init; } = "-";
        public decimal PricePerHour { get; init; }
        public DateTime? ExecutionDate { get; init; }
        public DateTime? ClosureDate { get; init; }
        public decimal BaseAmount { get; init; }
        public decimal DealCoefficient { get; init; }
        public decimal SurchargeAmount { get; init; }
        public decimal PremiumCoefficient { get; init; }
        public decimal PremiumAmount { get; init; }
        public decimal TotalAmount { get; init; }
    }

    private static IContainer CompactCellStyle(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingVertical(3)
            .PaddingHorizontal(2);
    }
}

