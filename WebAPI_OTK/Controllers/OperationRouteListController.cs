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
                q = q.Where(o => (o.ДатаИсполнения.HasValue && o.ДатаИсполнения >= датаС.Value) || 
                                 (o.ДатаЗакрытия.HasValue && o.ДатаЗакрытия >= датаС.Value));

            if (датаПо.HasValue)
                q = q.Where(o => (o.ДатаИсполнения.HasValue && o.ДатаИсполнения <= датаПо.Value) || 
                                 (o.ДатаЗакрытия.HasValue && o.ДатаЗакрытия <= датаПо.Value));

            var list = await q.Select(ToDtoProjection()).ToListAsync();
            
            // Пост-обработка: расчёт коэффициентов и сумм
            await CalculateOperationFinancials(list);
            
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
                Количество = dto.Количество,
                НормаВремениЧас = dto.НормаВремениЧас,
                НазваниеТарифа = dto.НазваниеТарифа,
                ЦенаЗаЧас = dto.ЦенаЗаЧас,
                ДатаИсполнения = dto.ДатаИсполнения,
                ДатаЗакрытия = dto.ДатаЗакрытия,
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
            entity.Количество = dto.Количество;
            entity.НормаВремениЧас = dto.НормаВремениЧас;
            entity.НазваниеТарифа = dto.НазваниеТарифа;
            entity.ЦенаЗаЧас = dto.ЦенаЗаЧас;
            entity.ДатаИсполнения = dto.ДатаИсполнения;
            entity.ДатаЗакрытия = dto.ДатаЗакрытия;
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

            entity.ДатаЗакрытия = DateTime.Now;
            entity.Статус = "Завершена";

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/ОперацияМЛ/отменить/5
        [HttpPut("cancel/{id:int}")]
        public async Task<IActionResult> ОтменитьОперацию(int id)
        {
            var entity = await _context.Операция_МЛ.FindAsync(id);
            if (entity == null) return NotFound(new { message = "Операция не найдена" });

            // Проверяем, что операция завершена (можно отменить только завершенные операции)
            if (entity.Статус != "Завершена")
            {
                return BadRequest(new { message = "Можно отменить только завершенные операции" });
            }

            // Помечаем операцию как отмененную (не удаляем для сохранения истории)
            entity.Статус = "Отменена";
            entity.Примечание = string.IsNullOrEmpty(entity.Примечание) 
                ? $"Отменена {DateTime.Now:dd.MM.yyyy HH:mm}" 
                : $"{entity.Примечание} | Отменена {DateTime.Now:dd.MM.yyyy HH:mm}";

            await _context.SaveChangesAsync();
            return Ok(new { message = "Операция отменена" });
        }

        // GET: api/ОперацияМЛ/расчет/5
        [HttpGet("calculation/{id:int}")]
        public async Task<ActionResult<РасчетОперацииDto>> GetРасчетОперации(int id)
        {
            var operation = await _context.Операция_МЛ
                .AsNoTracking()
                .Include(o => o.МЛ!)
                    .ThenInclude(m => m.Изделие)
                .Include(o => o.МЛ!)
                    .ThenInclude(m => m.ДСЕ)
                .Include(o => o.ТипОперации)
                .FirstOrDefaultAsync(o => o.ID == id);

            if (operation == null) return NotFound();

            // Входные данные
            var количество = operation.Количество ?? 0;
            var нормаВремени = operation.НормаВремениЧас ?? 0;
            var ценаЗаЧас = operation.ЦенаЗаЧас ?? 0;
            var датаИсполнения = operation.ДатаИсполнения;
            var датаЗакрытия = operation.ДатаЗакрытия;

            // Расчет базовой суммы
            var базоваяСумма = количество * нормаВремени * ценаЗаЧас;

            // Получение коэффициента сделки
            var коэфСделкиInfo = await GetКоэффициентСделкиInfo(operation.МЛID, operation.ТипОперацииID, датаЗакрытия);
            var коэфСделки = коэфСделкиInfo.Коэффициент;

            // Расчет суммы надбавки
            var суммаНадбавки = (коэфСделки - 1) * базоваяСумма;

            // Определение коэффициента премии
            var вСрок = !(датаЗакрытия.HasValue && датаИсполнения.HasValue && датаЗакрытия > датаИсполнения);
            var коэфПремии = вСрок ? 0.8m : 0m;

            // Расчет суммы премии
            var суммаПремии = коэфПремии * (суммаНадбавки + базоваяСумма);

            // Итого
            var итого = базоваяСумма + суммаНадбавки + суммаПремии;

            var result = new РасчетОперацииDto
            {
                ОперацияID = id,
                Формулы = new ФормулыРасчетаDto(),
                ВходныеДанные = new ВходныеДанныеРасчетаDto
                {
                    Количество = количество,
                    НормаВремениЧас = нормаВремени,
                    ЦенаЗаЧас = ценаЗаЧас,
                    КоэффициентСделки = коэфСделки,
                    ДатаИсполнения = датаИсполнения,
                    ДатаЗакрытия = датаЗакрытия,
                    ВСрок = вСрок
                },
                Результаты = new РезультатыРасчетаDto
                {
                    БазоваяСумма = базоваяСумма,
                    СуммаНадбавки = суммаНадбавки,
                    КоэффициентПремии = коэфПремии,
                    СуммаПремии = суммаПремии,
                    Итого = итого
                },
                ИсточникКоэффициентаСделки = коэфСделкиInfo
            };

            return Ok(result);
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
                ТабельныйНомер = o.Сотрудник != null ? o.Сотрудник.ТабельныйНомер : null,

                ОборудованиеID = o.ОборудованиеID,
                Оборудование = o.Оборудование != null ? o.Оборудование.Наименование : null,

                Количество = o.Количество,
                НормаВремениЧас = o.НормаВремениЧас,
                НазваниеТарифа = o.НазваниеТарифа,
                ЦенаЗаЧас = o.ЦенаЗаЧас,
                
                // Расчётные поля будут добавлены в сервисе
                БазоваяСумма = (o.Количество ?? 0) * (o.НормаВремениЧас ?? 0) * (o.ЦенаЗаЧас ?? 0),
                КоэффициентСделки = 1.0m, // Будет рассчитан из премиальных коэффициентов
                СуммаНадбавки = 0m,
                КоэффициентПремии = (o.ДатаЗакрытия.HasValue && o.ДатаИсполнения.HasValue && o.ДатаЗакрытия > o.ДатаИсполнения) ? 0m : 0.8m,
                СуммаПремии = 0m,
                Итого = 0m,

                ДатаИсполнения = o.ДатаИсполнения,
                ДатаЗакрытия = o.ДатаЗакрытия,
                Подразделение = o.Подразделение,
                Статус = o.Статус,
                Примечание = o.Примечание
            };
        }

        private async Task CalculateOperationFinancials(List<ОперацияМлDto> operations)
        {
            foreach (var op in operations)
            {
                // 1. Базовая сумма уже рассчитана в проекции
                var базовая = op.БазоваяСумма ?? 0m;

                // 2. Получить коэффициент сделки из премиальных коэффициентов
                var коэфСделки = await GetКоэффициентСделки(op.МЛID, op.ТипОперацииID, op.ДатаЗакрытия);
                op.КоэффициентСделки = коэфСделки;

                // 3. Сумма надбавки = (Коэффициент сделки - 1) × Базовая сумма
                op.СуммаНадбавки = (коэфСделки - 1) * базовая;

                // 4. Коэффициент премии уже рассчитан в проекции
                var коэфПремии = op.КоэффициентПремии ?? 0m;

                // 5. Сумма премии = Коэффициент премии × (Сумма надбавки + Базовая сумма)
                op.СуммаПремии = коэфПремии * (op.СуммаНадбавки + базовая);

                // 6. Итого = Базовая сумма + Сумма надбавки + Сумма премии
                op.Итого = базовая + op.СуммаНадбавки + op.СуммаПремии;
            }
        }

        private async Task<decimal> GetКоэффициентСделки(int млId, int типОперацииId, DateTime? датаЗакрытия)
        {
            var дата = датаЗакрытия ?? DateTime.Now;

            // Получаем МЛ для доступа к ИзделиеID и ДСЕID
            var мл = await _context.МЛ.AsNoTracking().FirstOrDefaultAsync(m => m.ID == млId);
            if (мл == null) return 1.0m;

            // Ищем премиальный коэффициент
            var коэф = await _context.ПремиальныеКоэффициенты
                .AsNoTracking()
                .Where(k =>
                    k.ИзделиеID == мл.ИзделиеID &&
                    k.ДСЕID == мл.ДСЕID &&
                    k.ТипОперацииID == типОперацииId &&
                    дата >= k.ДатаНачала &&
                    (k.ДатаОкончания == null || дата <= k.ДатаОкончания))
                .FirstOrDefaultAsync();

            return коэф?.Коэффициент ?? 1.0m;
        }

        private async Task<ИсточникКоэффициентаDto> GetКоэффициентСделкиInfo(int млId, int типОперацииId, DateTime? датаЗакрытия)
        {
            var дата = датаЗакрытия ?? DateTime.Now;

            // Получаем МЛ для доступа к ИзделиеID и ДСЕID
            var мл = await _context.МЛ
                .AsNoTracking()
                .Include(m => m.Изделие)
                .Include(m => m.ДСЕ)
                .FirstOrDefaultAsync(m => m.ID == млId);

            if (мл == null)
            {
                return new ИсточникКоэффициентаDto
                {
                    Найден = false,
                    Коэффициент = 1.0m
                };
            }

            // Ищем премиальный коэффициент
            var коэф = await _context.ПремиальныеКоэффициенты
                .AsNoTracking()
                .Include(k => k.ТипОперации)
                .Where(k =>
                    k.ИзделиеID == мл.ИзделиеID &&
                    k.ДСЕID == мл.ДСЕID &&
                    k.ТипОперацииID == типОперацииId &&
                    дата >= k.ДатаНачала &&
                    (k.ДатаОкончания == null || дата <= k.ДатаОкончания))
                .FirstOrDefaultAsync();

            if (коэф == null)
            {
                return new ИсточникКоэффициентаDto
                {
                    Найден = false,
                    Коэффициент = 1.0m,
                    Изделие = мл.Изделие?.Наименование,
                    ДСЕ = мл.ДСЕ?.Наименование
                };
            }

            return new ИсточникКоэффициентаDto
            {
                Найден = true,
                Коэффициент = коэф.Коэффициент,
                Изделие = мл.Изделие?.Наименование,
                ДСЕ = мл.ДСЕ?.Наименование,
                ТипОперации = коэф.ТипОперации?.Наименование,
                ДатаНачала = коэф.ДатаНачала,
                ДатаОкончания = коэф.ДатаОкончания
            };
        }
    }
}
