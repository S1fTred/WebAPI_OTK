using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_OTK.Dtos;

namespace WebAPI_OTK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobTitleController : ControllerBase
    {
        private readonly Model1 _context;

        public JobTitleController(Model1 context) => _context = context;

        // GET: api/Должность
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ДолжностьDto>>> GetДолжности()
        {
            var list = await _context.Должность
                .AsNoTracking()
                .Select(d => new ДолжностьDto
                {
                    ID = d.ID,
                    Наименование = d.Наименование,
                    Код = d.Код,
                    Сотрудник = d.Сотрудник
                        .Select(s => new СотрудникКороткоDto { ID = s.ID, ФИО = s.ФИО })
                        .ToList()
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/Должность/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ДолжностьDto>> GetДолжность(int id)
        {
            var item = await _context.Должность
                .AsNoTracking()
                .Where(d => d.ID == id)
                .Select(d => new ДолжностьDto
                {
                    ID = d.ID,
                    Наименование = d.Наименование,
                    Код = d.Код,
                    Сотрудник = d.Сотрудник
                        .Select(s => new СотрудникКороткоDto { ID = s.ID, ФИО = s.ФИО })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (item == null) return NotFound();
            return Ok(item);
        }
    }
}
