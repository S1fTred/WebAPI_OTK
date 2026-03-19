using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_OTK.Dtos;

namespace WebAPI_OTK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly Model1 _context;

        public ProductController(Model1 context) => _context = context;

        // GET: api/Изделие
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ИзделиеDto>>> GetИзделия()
        {
            var list = await _context.Изделие
                .AsNoTracking()
                .Select(i => new ИзделиеDto
                {
                    ID = i.ID,
                    Наименование = i.Наименование,
                    Описание = i.Описание,
                    Состояние = i.Состояние,
                    ДатаСоздания = i.ДатаСоздания,
                    КоличествоДСЕ = i.ДСЕ != null ? i.ДСЕ.Count : 0,
                    КоличествоМЛ = i.МЛ != null ? i.МЛ.Count : 0
                })
                .OrderBy(i => i.Наименование)
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/Изделие/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ИзделиеDetailDto>> GetИзделие(int id)
        {
            var item = await _context.Изделие
                .AsNoTracking()
                .Where(i => i.ID == id)
                .Select(i => new ИзделиеDetailDto
                {
                    ID = i.ID,
                    Наименование = i.Наименование,
                    Описание = i.Описание,
                    Состояние = i.Состояние,
                    ДатаСоздания = i.ДатаСоздания,
                    ДСЕ = i.ДСЕ
                        .Select(d => new ДсеShortDto
                        {
                            ID = d.ID,
                            Код = d.Код,
                            Наименование = d.Наименование,
                            Чертеж = d.Чертеж
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (item == null) return NotFound();
            return Ok(item);
        }

        // GET: api/Изделие/5/дсе
        [HttpGet("{id:int}/DCE")]
        public async Task<ActionResult<IEnumerable<ДсеListItemDto>>> GetДСЕИзделия(int id)
        {
            // проверка наличия изделия (чтобы возвращать 404, а не пустой список по ошибке id)
            var exists = await _context.Изделие.AsNoTracking().AnyAsync(x => x.ID == id);
            if (!exists) return NotFound();

            var list = await _context.ДСЕ
                .AsNoTracking()
                .Where(d => d.ИзделиеID == id)
                .Select(d => new ДсеListItemDto
                {
                    ID = d.ID,
                    Код = d.Код,
                    Наименование = d.Наименование,
                    Чертеж = d.Чертеж,
                    КоличествоМЛ = d.МЛ != null ? d.МЛ.Count : 0
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/Изделие/5/мл
        [HttpGet("{id:int}/ML")]
        public async Task<ActionResult<IEnumerable<МлListItemDto>>> GetМЛИзделия(int id)
        {
            var exists = await _context.Изделие.AsNoTracking().AnyAsync(x => x.ID == id);
            if (!exists) return NotFound();

            var list = await _context.МЛ
                .AsNoTracking()
                .Where(m => m.ИзделиеID == id)
                .Select(m => new МлListItemDto
                {
                    ID = m.ID,
                    НомерМЛ = m.НомерМЛ,
                    ДатаСоздания = m.ДатаСоздания,
                    Закрыт = m.Закрыт,
                    ДСЕ = m.ДСЕ != null ? m.ДСЕ.Наименование : null,
                    СотрудникОТК = m.Сотрудник != null ? m.Сотрудник.ФИО : null,
                    КоличествоОпераций = m.Операция_МЛ != null ? m.Операция_МЛ.Count : 0
                })
                .OrderByDescending(m => m.ДатаСоздания)
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/Изделие/5/карта-техпланирования
        [HttpGet("{id:int}/techplan-card")]
        public async Task<ActionResult<КартаТехпланированияDto>> GetКартаТехпланирования(int id)
        {
            var изделие = await _context.Изделие
                .AsNoTracking()
                .Where(i => i.ID == id)
                .Select(i => new
                {
                    i.Наименование,
                    ДСЕ = i.ДСЕ.Select(d => new
                    {
                        d.ID,
                        d.Код,
                        d.Наименование,
                        Операции = d.МЛ
                            .SelectMany(m => m.Операция_МЛ)
                            .Select(o => new
                            {
                                ТипОперации = o.ТипОперации != null ? o.ТипОперации.Наименование : null,
                                o.ДатаИсполнения,
                                o.ДатаЗакрытия
                            })
                            .ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (изделие == null) return NotFound();

            // Убираем дубли “на уровне DTO” (EF Distinct по анонимным может быть нестабилен)
            var карта = new КартаТехпланированияDto
            {
                Изделие = изделие.Наименование,
                ДСЕ = изделие.ДСЕ.Select(d => new КартаДсеDto
                {
                    ID = d.ID,
                    Код = d.Код,
                    Наименование = d.Наименование,
                    Операции = d.Операции
                        .GroupBy(x => new { x.ТипОперации, x.ДатаИсполнения, x.ДатаЗакрытия })
                        .Select(g => new КартаОперацияDto
                        {
                            ТипОперации = g.Key.ТипОперации,
                            ДатаНачала = g.Key.ДатаИсполнения,
                            ДатаОкончания = g.Key.ДатаЗакрытия
                        })
                        .ToList()
                }).ToList()
            };

            return Ok(карта);
        }        
    }
}
