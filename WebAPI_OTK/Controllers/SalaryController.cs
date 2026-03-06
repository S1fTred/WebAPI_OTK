using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WebAPI_OTK.Dtos;

namespace WebAPI_OTK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaryController : ControllerBase
    {
        private readonly Model1 _context;

        public SalaryController(Model1 context) => _context = context;

        // POST: api/Зарплата/calculate
        [HttpPost("calculate")]
        public async Task<IActionResult> РассчитатьЗарплату([FromBody] РасчетЗарплатыDto параметры)
        {
            if (параметры.КонецПериода < параметры.НачалоПериода)
                return BadRequest(new { сообщение = "Конец периода не может быть раньше начала периода" });

            var операции = await _context.Операция_МЛ
                .AsNoTracking()
                .Include(o => o.МЛ)
                .ThenInclude(m => m.Изделие)
                .Where(o => o.ДатаОкончания >= параметры.НачалоПериода &&
                            o.ДатаОкончания <= параметры.КонецПериода &&
                            o.Статус == "Завершена")
                .ToListAsync();

            var старыеЗаписи = await _context.Зарплата
                .Where(z => z.Период >= параметры.НачалоПериода && z.Период <= параметры.КонецПериода)
                .ToListAsync();

            if (старыеЗаписи.Count > 0)
                _context.Зарплата.RemoveRange(старыеЗаписи);

            foreach (var операция in операции)
            {
                // базовые данные
                var часы = операция.ФактическаяДлительностьЧас ?? 0m;
                var ставка = await GetСтавкаСотрудника(операция.СотрудникID);

                var суммаОклад = часы * ставка;
                var премия = 0m;

                if (параметры.ВключатьПремии)
                    премия = await РассчитатьПремию(операция, ставка);

                if (параметры.ВключатьНадбавки)
                {

                    if (операция.ДатаНачала.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
                    {
                        var надбавка = суммаОклад * 1.0m;
                        суммаОклад += надбавка;
                    }
                }

                var entity = new Зарплата
                {
                    ОперацияМЛID = операция.ID,
                    СотрудникID = операция.СотрудникID,
                    ЧасыОтработано = часы,
                    Период = операция.ДатаОкончания,
                    СтавкаЧасовая = ставка,
                    СуммаОклад = суммаОклад,
                    Премия = премия,
                    ИтогоКВыплате = суммаОклад + премия
                };

                _context.Зарплата.Add(entity);
            }

            await _context.SaveChangesAsync();

            return Ok(new { сообщение = $"Рассчитана зарплата для {операции.Count} операций" });
        }

        private static System.Linq.Expressions.Expression<Func<Зарплата, ЗарплатаDto>> ToDtoProjection()
        {
            return z => new ЗарплатаDto
            {
                ID = z.ID,
                ОперацияМЛID = z.ОперацияМЛID,
                СотрудникID = z.СотрудникID,
                Период = z.Период,
                ЧасыОтработано = z.ЧасыОтработано,
                СтавкаЧасовая = z.СтавкаЧасовая,
                СуммаОклад = z.СуммаОклад,
                Премия = z.Премия,
                ИтогоКВыплате = z.ИтогоКВыплате,
                Сотрудник = z.Сотрудник != null ? z.Сотрудник.ФИО : null,
                НомерМЛ = z.Операция_МЛ != null && z.Операция_МЛ.МЛ != null ? z.Операция_МЛ.МЛ.НомерМЛ : null
            };
        }

        private async Task<decimal> GetСтавкаСотрудника(int сотрудникId)
        {
            // TODO: заменить на реальную логику (например из таблицы ставок/должности)
            await Task.CompletedTask;
            return 500m;
        }

        private async Task<decimal> РассчитатьПремию(Операция_МЛ операция, decimal ставкаЧасовая)
        {
            var дата = операция.ДатаОкончания ?? операция.ДатаНачала;

            var k = await _context.ПремиальныеКоэффициенты
                .AsNoTracking()
                .Where(x =>
                    x.ИзделиеID == операция.МЛ.ИзделиеID &&
                    x.ДСЕID == операция.МЛ.ДСЕID &&
                    x.ТипОперацииID == операция.ТипОперацииID &&
                    дата >= x.ДатаНачала &&
                    (x.ДатаОкончания == null || дата <= x.ДатаОкончания))
                .FirstOrDefaultAsync();

            if (k == null) return 0m;

            var часы = операция.ФактическаяДлительностьЧас ?? 0m;
            return часы * ставкаЧасовая * k.Коэффициент;
        }

       
    }
}
