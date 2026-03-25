using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_OTK.Dtos;

namespace WebAPI_OTK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DCEController : ControllerBase
    {
        private readonly OtkDbContext _context;

        public DCEController(OtkDbContext context) => _context = context;

        // GET: api/DCE
        // Параметры: ?page=1&pageSize=50&search=поиск&изделиеId=1
        [HttpGet]
        public async Task<ActionResult<ДсеPagedResponseDto>> GetДСЕ(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? search = null,
            [FromQuery] int? изделиеId = null)
        {
            // Валидация параметров пагинации
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;
            if (pageSize > 100) pageSize = 100; // Максимум 100 записей на страницу

            var q = _context.ДСЕ.AsNoTracking().AsQueryable();

            // Фильтр по изделию
            if (изделиеId.HasValue)
                q = q.Where(d => d.ИзделиеID == изделиеId.Value);

            // Поиск по коду или наименованию
            if (!string.IsNullOrWhiteSpace(search))
            {
                q = q.Where(d => d.Код.Contains(search) || 
                                (d.Наименование != null && d.Наименование.Contains(search)));
            }

            // Подсчет общего количества
            var totalCount = await q.CountAsync();

            // Получение данных с пагинацией
            var data = await q
                .OrderBy(d => d.Код)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToDtoProjection())
                .ToListAsync();

            var response = new ДсеPagedResponseDto
            {
                Данные = data,
                Страница = page,
                РазмерСтраницы = pageSize,
                ВсегоЗаписей = totalCount,
                ВсегоСтраниц = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return Ok(response);
        }

        // GET: api/DCE/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ДсеDto>> GetДСЕ(int id)
        {
            var item = await _context.ДСЕ
                .AsNoTracking()
                .Where(d => d.ID == id)
                .Select(ToDtoProjection())
                .FirstOrDefaultAsync();

            if (item == null) return NotFound();
            return Ok(item);
        }

        // GET: api/DCE/5/ml
        // Получить МЛ для конкретного ДСЕ
        [HttpGet("{id:int}/ml")]
        public async Task<ActionResult<IEnumerable<МлListItemDto>>> GetМЛДСЕ(int id)
        {
            var exists = await _context.ДСЕ.AsNoTracking().AnyAsync(d => d.ID == id);
            if (!exists) return NotFound();

            var list = await _context.МЛ
                .AsNoTracking()
                .Where(m => m.ДСЕID == id)
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

        // POST: api/DCE
        [HttpPost]
        public async Task<ActionResult<ДсеDto>> PostДСЕ([FromBody] ДсеCreateDto dto)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(dto.Код))
                return BadRequest(new { message = "Код обязателен" });

            // Проверка уникальности кода
            var существует = await _context.ДСЕ.AnyAsync(d => d.Код == dto.Код);
            if (существует)
                return BadRequest(new { message = $"ДСЕ с кодом '{dto.Код}' уже существует" });

            var entity = new ДСЕ
            {
                Код = dto.Код,
                Наименование = dto.Наименование,
                Чертеж = dto.Чертеж,
                ИзделиеID = dto.ИзделиеID
            };

            _context.ДСЕ.Add(entity);
            await _context.SaveChangesAsync();

            var result = await _context.ДСЕ
                .AsNoTracking()
                .Where(d => d.ID == entity.ID)
                .Select(ToDtoProjection())
                .FirstAsync();

            return CreatedAtAction(nameof(GetДСЕ), new { id = entity.ID }, result);
        }

        // PUT: api/DCE/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutДСЕ(int id, [FromBody] ДсеUpdateDto dto)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(dto.Код))
                return BadRequest(new { message = "Код обязателен" });

            var entity = await _context.ДСЕ.FindAsync(id);
            if (entity == null) return NotFound();

            // Проверка уникальности кода (если изменился)
            if (entity.Код != dto.Код)
            {
                var существует = await _context.ДСЕ.AnyAsync(d => d.Код == dto.Код && d.ID != id);
                if (существует)
                    return BadRequest(new { message = $"ДСЕ с кодом '{dto.Код}' уже существует" });
            }

            entity.Код = dto.Код;
            entity.Наименование = dto.Наименование;
            entity.Чертеж = dto.Чертеж;
            entity.ИзделиеID = dto.ИзделиеID;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/DCE/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteДСЕ(int id)
        {
            var entity = await _context.ДСЕ
                .Include(d => d.МЛ)
                .FirstOrDefaultAsync(d => d.ID == id);

            if (entity == null) return NotFound();

            // Проверка: есть ли связанные МЛ?
            if (entity.МЛ.Any())
            {
                return BadRequest(new
                {
                    message = $"Невозможно удалить ДСЕ. Существуют связанные маршрутные листы: {entity.МЛ.Count}",
                    количествоМЛ = entity.МЛ.Count
                });
            }

            _context.ДСЕ.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ===== PROJECTION =====
        private static System.Linq.Expressions.Expression<Func<ДСЕ, ДсеDto>> ToDtoProjection()
        {
            return d => new ДсеDto
            {
                ID = d.ID,
                Код = d.Код,
                Наименование = d.Наименование,
                Чертеж = d.Чертеж,
                ИзделиеID = d.ИзделиеID,
                Изделие = d.Изделие != null ? d.Изделие.Наименование : null,
                КоличествоМЛ = d.МЛ != null ? d.МЛ.Count : 0
            };
        }
    }
}
