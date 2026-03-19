using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_OTK.Dtos;

namespace WebAPI_OTK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly Model1 _context;

        public ReportController(Model1 context) => _context = context;

        // GET: api/Отчеты/ведомость-выработки?начало=2026-02-01&конец=2026-02-29&подразделение=Цех1
        [HttpGet("Report-production")]
        public async Task<ActionResult<ВедомостьВыработкиResponseDto>> GetВедомостьВыработки(
            [FromQuery] DateTime начало,
            [FromQuery] DateTime конец,
            [FromQuery] string подразделение)
        {
            if (конец < начало)
                return BadRequest(new { message = "конец не может быть раньше начала" });

            if (string.IsNullOrWhiteSpace(подразделение))
                return BadRequest(new { message = "подразделение обязательно" });

            // ВАЖНО: никаких Include не нужно — всё берём через Select/GroupBy
            // Плюс убираем DateTime.Now из расчёта — расчёт привязан к данным операции.
            var rows = await _context.Операция_МЛ
                .AsNoTracking()
                .Where(o =>
                    o.ДатаЗакрытия >= начало &&
                    o.ДатаЗакрытия <= конец &&
                    o.Статус == "Завершена" &&
                    o.Подразделение == подразделение)
                .GroupBy(o => new { o.СотрудникID, ФИО = o.Сотрудник.ФИО })
                .Select(g => new ВедомостьВыработкиRowDto
                {
                    СотрудникID = g.Key.СотрудникID,
                    ФИО = g.Key.ФИО,
                    ВсегоЧасов = 0m,
                    КоличествоОпераций = g.Count(),
                    СуммаОплаты = g.Count() * 500m * 
                        (g.Any(x => x.ДатаИсполнения.HasValue && 
                            (x.ДатаИсполнения.Value.DayOfWeek == DayOfWeek.Saturday || 
                             x.ДатаИсполнения.Value.DayOfWeek == DayOfWeek.Sunday)) ? 2.0m : 1.0m)
                })
                .OrderBy(r => r.ФИО)
                .ToListAsync();

            var response = new ВедомостьВыработкиResponseDto
            {
                Начало = начало,
                Конец = конец,
                Период = $"{начало:dd.MM.yyyy} - {конец:dd.MM.yyyy}",
                Подразделение = подразделение,
                Данные = rows
            };

            return Ok(response);
        }
    }
}
