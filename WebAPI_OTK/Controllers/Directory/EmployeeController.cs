using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_OTK.Dtos;

namespace WebAPI_OTK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly OtkDbContext _context;

        public EmployeeController(OtkDbContext context) => _context = context;

        // GET: api/Сотрудник
        // фильтры: ?должностьId=5&поиск=иван
        [HttpGet]
        public async Task<ActionResult<IEnumerable<СотрудникDto>>> GetСотрудники(
            [FromQuery] int? должностьId = null,
            [FromQuery] string? поиск = null)
        {
            var q = _context.Сотрудник.AsNoTracking().AsQueryable();

            if (должностьId.HasValue)
                q = q.Where(s => s.ДолжностьID == должностьId.Value);

            if (!string.IsNullOrWhiteSpace(поиск))
                q = q.Where(s => s.ФИО.Contains(поиск) || (s.ТабельныйНомер != null && s.ТабельныйНомер.Contains(поиск)));

            var list = await q
                .Select(ToDtoProjection())
                .OrderBy(s => s.ФИО)
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/Сотрудник/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<СотрудникDto>> GetСотрудник(int id)
        {
            var item = await _context.Сотрудник
                .AsNoTracking()
                .Where(s => s.ID == id)
                .Select(ToDtoProjection())
                .FirstOrDefaultAsync();

            if (item == null) return NotFound();
            return Ok(item);
        }

        // ===== projection =====
        private static System.Linq.Expressions.Expression<Func<Сотрудник, СотрудникDto>> ToDtoProjection()
        {
            return s => new СотрудникDto
            {
                ID = s.ID,
                ФИО = s.ФИО,
                ТабельныйНомер = s.ТабельныйНомер,
                ДатаПриема = s.ДатаПриема,
                ДолжностьID = s.ДолжностьID,
                Должность = s.Должность != null ? s.Должность.Наименование : null
            };
        }
    }
}
