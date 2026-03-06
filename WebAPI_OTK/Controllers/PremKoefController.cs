using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_OTK.Dtos;

namespace WebAPI_OTK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PremKoefController : ControllerBase
    {
        private readonly Model1 _context;

        public PremKoefController(Model1 context) => _context = context;

        // GET: api/PremKoef
        // Фильтры: ?толькоАктивные=true&изделиеId=1&дсеId=2&типОперацииId=3&поиск=название
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ПремиальныйКоэффициентDto>>> GetПремиальныеКоэффициенты(
            [FromQuery] bool толькоАктивные = false,
            [FromQuery] int? изделиеId = null,
            [FromQuery] int? дсеId = null,
            [FromQuery] int? типОперацииId = null,
            [FromQuery] string? поиск = null)
        {
            var q = _context.ПремиальныеКоэффициенты.AsNoTracking().AsQueryable();

            // Фильтр по активности
            if (толькоАктивные)
            {
                var сегодня = DateTime.Today;
                q = q.Where(k => k.ДатаНачала <= сегодня && (k.ДатаОкончания == null || k.ДатаОкончания >= сегодня));
            }

            // Фильтр по изделию
            if (изделиеId.HasValue)
                q = q.Where(k => k.ИзделиеID == изделиеId.Value);

            // Фильтр по ДСЕ
            if (дсеId.HasValue)
                q = q.Where(k => k.ДСЕID == дсеId.Value);

            // Фильтр по типу операции
            if (типОперацииId.HasValue)
                q = q.Where(k => k.ТипОперацииID == типОперацииId.Value);

            // Поиск по наименованию
            if (!string.IsNullOrWhiteSpace(поиск))
                q = q.Where(k => k.Наименование.Contains(поиск));

            var list = await q
                .Select(ToDtoProjection())
                .OrderByDescending(k => k.ДатаНачала)
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/PremKoef/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ПремиальныйКоэффициентDto>> GetПремиальныйКоэффициент(int id)
        {
            var item = await _context.ПремиальныеКоэффициенты
                .AsNoTracking()
                .Where(k => k.ID == id)
                .Select(ToDtoProjection())
                .FirstOrDefaultAsync();

            if (item == null) return NotFound();
            return Ok(item);
        }

        // GET: api/PremKoef/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ПремиальныйКоэффициентDto>>> GetАктивныеКоэффициенты()
        {
            var сегодня = DateTime.Today;
            
            var list = await _context.ПремиальныеКоэффициенты
                .AsNoTracking()
                .Where(k => k.ДатаНачала <= сегодня && (k.ДатаОкончания == null || k.ДатаОкончания >= сегодня))
                .Select(ToDtoProjection())
                .OrderBy(k => k.Наименование)
                .ToListAsync();

            return Ok(list);
        }

        // POST: api/PremKoef
        [HttpPost]
        public async Task<ActionResult<ПремиальныйКоэффициентDto>> PostПремиальныйКоэффициент(
            [FromBody] ПремиальныйКоэффициентCreateDto dto)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(dto.Наименование))
                return BadRequest(new { message = "Наименование обязательно" });

            if (dto.Коэффициент <= 0)
                return BadRequest(new { message = "Коэффициент должен быть больше 0" });

            // Проверка дат
            if (dto.ДатаОкончания.HasValue && dto.ДатаНачала.HasValue && dto.ДатаОкончания < dto.ДатаНачала)
                return BadRequest(new { message = "Дата окончания не может быть раньше даты начала" });

            var entity = new ПремиальныеКоэффициенты
            {
                Наименование = dto.Наименование,
                Коэффициент = dto.Коэффициент,
                ДатаНачала = dto.ДатаНачала ?? DateTime.Today,
                ДатаОкончания = dto.ДатаОкончания,
                ИзделиеID = dto.ИзделиеID,
                ДСЕID = dto.ДСЕID,
                ТипОперацииID = dto.ТипОперацииID
            };

            _context.ПремиальныеКоэффициенты.Add(entity);
            await _context.SaveChangesAsync();

            var result = await _context.ПремиальныеКоэффициенты
                .AsNoTracking()
                .Where(k => k.ID == entity.ID)
                .Select(ToDtoProjection())
                .FirstAsync();

            return CreatedAtAction(nameof(GetПремиальныйКоэффициент), new { id = entity.ID }, result);
        }

        // PUT: api/PremKoef/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutПремиальныйКоэффициент(int id, [FromBody] ПремиальныйКоэффициентUpdateDto dto)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(dto.Наименование))
                return BadRequest(new { message = "Наименование обязательно" });

            if (dto.Коэффициент <= 0)
                return BadRequest(new { message = "Коэффициент должен быть больше 0" });

            if (dto.ДатаОкончания.HasValue && dto.ДатаНачала.HasValue && dto.ДатаОкончания < dto.ДатаНачала)
                return BadRequest(new { message = "Дата окончания не может быть раньше даты начала" });

            var entity = await _context.ПремиальныеКоэффициенты.FindAsync(id);
            if (entity == null) return NotFound();

            entity.Наименование = dto.Наименование;
            entity.Коэффициент = dto.Коэффициент;
            entity.ДатаНачала = dto.ДатаНачала ?? entity.ДатаНачала;
            entity.ДатаОкончания = dto.ДатаОкончания;
            entity.ИзделиеID = dto.ИзделиеID;
            entity.ДСЕID = dto.ДСЕID;
            entity.ТипОперацииID = dto.ТипОперацииID;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/PremKoef/deactivate/5
        // "Мягкое" удаление - установка даты окончания
        [HttpPut("deactivate/{id:int}")]
        public async Task<IActionResult> ДеактивироватьКоэффициент(int id)
        {
            var entity = await _context.ПремиальныеКоэффициенты.FindAsync(id);
            if (entity == null) return NotFound();

            // Проверка: уже деактивирован?
            if (entity.ДатаОкончания.HasValue && entity.ДатаОкончания <= DateTime.Today)
                return BadRequest(new { message = "Коэффициент уже деактивирован" });

            // Устанавливаем дату окончания = вчера (чтобы сегодня он уже не действовал)
            entity.ДатаОкончания = DateTime.Today.AddDays(-1);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/PremKoef/5
        // Физическое удаление (только для неактивных)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteПремиальныйКоэффициент(int id)
        {
            var entity = await _context.ПремиальныеКоэффициенты.FindAsync(id);
            if (entity == null) return NotFound();

            // Проверка: можно удалять только неактивные
            var сегодня = DateTime.Today;
            var активный = entity.ДатаНачала <= сегодня && (entity.ДатаОкончания == null || entity.ДатаОкончания >= сегодня);

            if (активный)
                return BadRequest(new { message = "Нельзя удалить активный коэффициент. Сначала деактивируйте его." });

            _context.ПремиальныеКоэффициенты.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ===== PROJECTION =====
        private static System.Linq.Expressions.Expression<Func<ПремиальныеКоэффициенты, ПремиальныйКоэффициентDto>> ToDtoProjection()
        {
            return k => new ПремиальныйКоэффициентDto
            {
                ID = k.ID,
                Наименование = k.Наименование,
                Коэффициент = k.Коэффициент,
                ДатаНачала = k.ДатаНачала,
                ДатаОкончания = k.ДатаОкончания,
                Активный = k.ДатаНачала <= DateTime.Today && (k.ДатаОкончания == null || k.ДатаОкончания >= DateTime.Today),
                ИзделиеID = k.ИзделиеID,
                Изделие = k.Изделие != null ? k.Изделие.Наименование : null,
                ДСЕID = k.ДСЕID,
                ДСЕ = k.ДСЕ != null ? k.ДСЕ.Наименование : null,
                ТипОперацииID = k.ТипОперацииID,
                ТипОперации = k.ТипОперации != null ? k.ТипОперации.Наименование : null
            };
        }
    }
}
