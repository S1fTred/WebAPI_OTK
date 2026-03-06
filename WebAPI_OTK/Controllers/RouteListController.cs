using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_OTK.Dtos;

namespace WebAPI_OTK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteListController : ControllerBase
    {
        private readonly Model1 _context;

        public RouteListController(Model1 context) => _context = context;

        // GET: api/МЛ
        // фильтры: ?открытые=true&изделиеId=1&дсеId=2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<МлDto>>> GetМЛ(
            [FromQuery] bool? открытые = null,
            [FromQuery] int? изделиеId = null,
            [FromQuery] int? дсеId = null)
        {
            var q = _context.МЛ.AsNoTracking().AsQueryable();

            if (открытые.HasValue)
                q = q.Where(m => m.Закрыт == !открытые.Value ? true : false); // открытые=true => Закрыт=false

            if (изделиеId.HasValue)
                q = q.Where(m => m.ИзделиеID == изделиеId.Value);

            if (дсеId.HasValue)
                q = q.Where(m => m.ДСЕID == дсеId.Value);

            var list = await q
                .Select(ToDtoProjection())
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/МЛ/открытые  
        [HttpGet("opened")]
        public async Task<ActionResult<IEnumerable<МлDto>>> GetОткрытыеМЛ()
        {
            var list = await _context.МЛ
                .AsNoTracking()
                .Where(m => m.Закрыт == false)
                .Select(ToDtoProjection())
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/МЛ/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<МлDto>> GetМЛ(int id)
        {
            var item = await _context.МЛ
                .AsNoTracking()
                .Where(m => m.ID == id)
                .Select(ToDtoProjection())
                .FirstOrDefaultAsync();

            if (item == null) return NotFound();
            return Ok(item);
        }

        // PUT: api/RouteList/close/5
        [HttpPut("close/{id:int}")]
        public async Task<IActionResult> ЗакрытьМЛ(int id, [FromBody] ЗакрытиеМлDto dto)
        {
            var мл = await _context.МЛ
                .Include(m => m.Операция_МЛ)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (мл == null) return NotFound();

            // Проверка: уже закрыт?
            if (мл.Закрыт == true)
                return BadRequest(new { message = "МЛ уже закрыт" });

            // Проверка: все операции завершены?
            var незавершенныеОперации = мл.Операция_МЛ.Where(o => o.Статус != "Завершена").ToList();
            if (незавершенныеОперации.Any())
            {
                return BadRequest(new
                {
                    message = $"Невозможно закрыть МЛ. Есть незавершенные операции: {незавершенныеОперации.Count}",
                    незавершенныеОперации = незавершенныеОперации.Select(o => new { o.ID, o.Статус }).ToList()
                });
            }

            // Закрываем МЛ
            мл.Закрыт = true;
            мл.СотрудникОТК = dto.СотрудникОТКID;
            мл.КоличествоОТК = dto.КоличествоОТК;
            мл.КоличествоБрак = dto.КоличествоБрак;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // =========================
        // PROJECTION 
        // =========================

        private static System.Linq.Expressions.Expression<Func<МЛ, МлDto>> ToDtoProjection()
        {
            return m => new МлDto
            {
                ID = m.ID,
                НомерМЛ = m.НомерМЛ,
                ДатаСоздания = m.ДатаСоздания,
                Закрыт = m.Закрыт,
                КоличествоОТК = m.КоличествоОТК,
                КоличествоБрак = m.КоличествоБрак,

                ДСЕID = m.ДСЕID,
                ДСЕ = m.ДСЕ != null ? m.ДСЕ.Наименование : null,

                ИзделиеID = m.ИзделиеID,
                Изделие = m.Изделие != null ? m.Изделие.Наименование : null,

                СотрудникОТКID = m.СотрудникОТК,
                СотрудникОТК = m.Сотрудник != null ? m.Сотрудник.ФИО : null,

                КоличествоОпераций = m.Операция_МЛ != null ? m.Операция_МЛ.Count : 0
            };
        }

    }
}
