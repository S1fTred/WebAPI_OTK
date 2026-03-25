using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_OTK.Dtos;

namespace WebAPI_OTK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationTypeController : ControllerBase
    {
        private readonly OtkDbContext _context;

        public OperationTypeController(OtkDbContext context) => _context = context;

        // GET: api/OperationType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ТипОперацииDto>>> GetТипыОперации()
        {
            var list = await _context.ТипОперации
                .AsNoTracking()
                .Select(t => new ТипОперацииDto
                {
                    ID = t.ID,
                    Наименование = t.Наименование,
                    Описание = t.Описание,
                    ДлительностьЧас = t.ДлительностьЧас,
                    КодОперации = t.КодОперации
                })
                .OrderBy(t => t.Наименование)
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/OperationType/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ТипОперацииDto>> GetТипОперации(int id)
        {
            var item = await _context.ТипОперации
                .AsNoTracking()
                .Where(t => t.ID == id)
                .Select(t => new ТипОперацииDto
                {
                    ID = t.ID,
                    Наименование = t.Наименование,
                    Описание = t.Описание,
                    ДлительностьЧас = t.ДлительностьЧас,
                    КодОперации = t.КодОперации
                })
                .FirstOrDefaultAsync();

            if (item == null) return NotFound();
            return Ok(item);
        }
    }
}
