using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_OTK.Dtos;

namespace WebAPI_OTK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationRouteListController : ControllerBase
    {
        private readonly Model1 _context;

        public OperationRouteListController(Model1 context) => _context = context;

        // GET: api/ОперацияМЛ?статус=В%20работе&сотрудникId=5&млId=10&датаС=2026-02-01&датаПо=2026-02-29
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ОперацияМлDto>>> GetОперацииМЛ(
            [FromQuery] string? статус = null,
            [FromQuery] int? сотрудникId = null,
            [FromQuery] int? млId = null,
            [FromQuery] DateTime? датаС = null,
            [FromQuery] DateTime? датаПо = null)
        {
            var q = _context.Операция_МЛ.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(статус))
                q = q.Where(o => o.Статус == статус);

            if (сотрудникId.HasValue)
                q = q.Where(o => o.СотрудникID == сотрудникId.Value);

            if (млId.HasValue)
                q = q.Where(o => o.МЛID == млId.Value);

            if (датаС.HasValue)
                q = q.Where(o => o.ДатаНачала >= датаС.Value);

            if (датаПо.HasValue)
                q = q.Where(o => o.ДатаНачала <= датаПо.Value);

            var list = await q.Select(ToDtoProjection()).ToListAsync();
            return Ok(list);
        }

        // GET: api/ОперацияМЛ/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ОперацияМлDto>> GetОперацияМЛ(int id)
        {
            var item = await _context.Операция_МЛ
                .AsNoTracking()
                .Where(o => o.ID == id)
                .Select(ToDtoProjection())
                .FirstOrDefaultAsync();

            if (item == null) return NotFound();
            return Ok(item);
        }

        // GET: api/ОперацияМЛ/сотрудник/5  -> операции "В работе"
        [HttpGet("employee/{employeeId:int}")]
        public async Task<ActionResult<IEnumerable<ОперацияМлDto>>> GetОперацииПоСотруднику(int employeeId)
        {
            var list = await _context.Операция_МЛ
                .AsNoTracking()
                .Where(o => o.СотрудникID == employeeId && o.Статус == "В работе")
                .Select(ToDtoProjection())
                .ToListAsync();

            return Ok(list);
        }

        // POST: api/ОперацияМЛ
        [HttpPost]
        public async Task<ActionResult<ОперацияМлDto>> PostОперацияМЛ([FromBody] ОперацияМлCreateDto dto)
        {
            if (dto.МЛID <= 0) return BadRequest(new { message = "млID должен быть > 0" });
            if (dto.ТипОперацииID <= 0) return BadRequest(new { message = "типОперацииID должен быть > 0" });
            if (dto.СотрудникID <= 0) return BadRequest(new { message = "сотрудникID должен быть > 0" });

            var mlExists = await _context.МЛ.AsNoTracking().AnyAsync(m => m.ID == dto.МЛID);
            if (!mlExists) return BadRequest(new { message = $"МЛ с id={dto.МЛID} не найден" });

            var entity = new Операция_МЛ
            {
                МЛID = dto.МЛID,
                ТипОперацииID = dto.ТипОперацииID,
                СотрудникID = dto.СотрудникID,
                ОборудованиеID = dto.ОборудованиеID,
                ДатаНачала = dto.ДатаНачала,
                ДатаОкончания = dto.ДатаОкончания,
                ФактическаяДлительностьЧас = dto.ФактическаяДлительностьЧас,
                Подразделение = dto.Подразделение,
                Статус = string.IsNullOrWhiteSpace(dto.Статус) ? "В работе" : dto.Статус,
                Примечание = dto.Примечание
            };

            _context.Операция_МЛ.Add(entity);
            await _context.SaveChangesAsync();

            var result = await _context.Операция_МЛ
                .AsNoTracking()
                .Where(o => o.ID == entity.ID)
                .Select(ToDtoProjection())
                .FirstAsync();

            return CreatedAtAction(nameof(GetОперацияМЛ), new { id = entity.ID }, result);
        }

        // PUT: api/ОперацияМЛ/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutОперацияМЛ(int id, [FromBody] ОперацияМлUpdateDto dto)
        {
            if (dto.МЛID <= 0) return BadRequest(new { message = "млID должен быть > 0" });
            if (dto.ТипОперацииID <= 0) return BadRequest(new { message = "типОперацииID должен быть > 0" });
            if (dto.СотрудникID <= 0) return BadRequest(new { message = "сотрудникID должен быть > 0" });

            var entity = await _context.Операция_МЛ.FindAsync(id);
            if (entity == null) return NotFound();

            entity.МЛID = dto.МЛID;
            entity.ТипОперацииID = dto.ТипОперацииID;
            entity.СотрудникID = dto.СотрудникID;
            entity.ОборудованиеID = dto.ОборудованиеID;
            entity.ДатаНачала = dto.ДатаНачала;
            entity.ДатаОкончания = dto.ДатаОкончания;
            entity.ФактическаяДлительностьЧас = dto.ФактическаяДлительностьЧас;
            entity.Подразделение = dto.Подразделение;
            entity.Статус = dto.Статус;
            entity.Примечание = dto.Примечание;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ОперацияМЛ/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteОперацияМЛ(int id)
        {
            var entity = await _context.Операция_МЛ.FindAsync(id);
            if (entity == null) return NotFound();

            _context.Операция_МЛ.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/ОперацияМЛ/завершить/5
        [HttpPut("finish/{id:int}")]
        public async Task<IActionResult> ЗавершитьОперацию(int id)
        {
            var entity = await _context.Операция_МЛ.FindAsync(id);
            if (entity == null) return NotFound();

            entity.ДатаОкончания = DateTime.Now;
            entity.Статус = "Завершена";

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static System.Linq.Expressions.Expression<Func<Операция_МЛ, ОперацияМлDto>> ToDtoProjection()
        {
            return o => new ОперацияМлDto
            {
                ID = o.ID,
                МЛID = o.МЛID,
                НомерМЛ = o.МЛ != null ? o.МЛ.НомерМЛ : null,

                ТипОперацииID = o.ТипОперацииID,
                ТипОперации = o.ТипОперации != null ? o.ТипОперации.Наименование : null,

                СотрудникID = o.СотрудникID,
                Сотрудник = o.Сотрудник != null ? o.Сотрудник.ФИО : null,

                ОборудованиеID = o.ОборудованиеID,
                Оборудование = o.Оборудование != null ? o.Оборудование.Наименование : null,

                ДатаНачала = o.ДатаНачала,
                ДатаОкончания = o.ДатаОкончания,
                ФактическаяДлительностьЧас = o.ФактическаяДлительностьЧас,
                Подразделение = o.Подразделение,
                Статус = o.Статус,
                Примечание = o.Примечание
            };
        }
    }
}
