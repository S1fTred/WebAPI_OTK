using System.Text.Json.Serialization;

namespace WebAPI_OTK.Dtos;

public class ОперацияМлDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("млID")]
    public int МЛID { get; set; }

    [JsonPropertyName("номерМЛ")]
    public string? НомерМЛ { get; set; }

    [JsonPropertyName("типОперацииID")]
    public int ТипОперацииID { get; set; }

    [JsonPropertyName("типОперации")]
    public string? ТипОперации { get; set; }

    [JsonPropertyName("сотрудникID")]
    public int СотрудникID { get; set; }

    [JsonPropertyName("сотрудник")]
    public string? Сотрудник { get; set; }

    [JsonPropertyName("табельныйНомер")]
    public string? ТабельныйНомер { get; set; }

    [JsonPropertyName("оборудованиеID")]
    public int? ОборудованиеID { get; set; }

    [JsonPropertyName("оборудование")]
    public string? Оборудование { get; set; }

    [JsonPropertyName("количество")]
    public int? Количество { get; set; }

    [JsonPropertyName("нормаВремениЧас")]
    public decimal? НормаВремениЧас { get; set; }

    [JsonPropertyName("названиеТарифа")]
    public string? НазваниеТарифа { get; set; }

    [JsonPropertyName("ценаЗаЧас")]
    public decimal? ЦенаЗаЧас { get; set; }

    [JsonPropertyName("базоваяСумма")]
    public decimal? БазоваяСумма { get; set; }

    [JsonPropertyName("коэффициентСделки")]
    public decimal? КоэффициентСделки { get; set; }

    [JsonPropertyName("суммаНадбавки")]
    public decimal? СуммаНадбавки { get; set; }

    [JsonPropertyName("коэффициентПремии")]
    public decimal? КоэффициентПремии { get; set; }

    [JsonPropertyName("суммаПремии")]
    public decimal? СуммаПремии { get; set; }

    [JsonPropertyName("итого")]
    public decimal? Итого { get; set; }

    [JsonPropertyName("датаИсполнения")]
    public DateTime? ДатаИсполнения { get; set; }

    [JsonPropertyName("датаЗакрытия")]
    public DateTime? ДатаЗакрытия { get; set; }

    [JsonPropertyName("подразделение")]
    public string? Подразделение { get; set; }

    [JsonPropertyName("статус")]
    public string? Статус { get; set; }

    [JsonPropertyName("примечание")]
    public string? Примечание { get; set; }
}

public class ОперацияМлCreateDto
{
    [JsonPropertyName("млID")]
    public int МЛID { get; set; }

    [JsonPropertyName("типОперацииID")]
    public int ТипОперацииID { get; set; }

    [JsonPropertyName("сотрудникID")]
    public int СотрудникID { get; set; }

    [JsonPropertyName("оборудованиеID")]
    public int? ОборудованиеID { get; set; }

    [JsonPropertyName("количество")]
    public int? Количество { get; set; }

    [JsonPropertyName("нормаВремениЧас")]
    public decimal? НормаВремениЧас { get; set; }

    [JsonPropertyName("названиеТарифа")]
    public string? НазваниеТарифа { get; set; }

    [JsonPropertyName("ценаЗаЧас")]
    public decimal? ЦенаЗаЧас { get; set; }

    [JsonPropertyName("датаИсполнения")]
    public DateTime? ДатаИсполнения { get; set; }

    [JsonPropertyName("датаЗакрытия")]
    public DateTime? ДатаЗакрытия { get; set; }

    [JsonPropertyName("подразделение")]
    public string? Подразделение { get; set; }

    [JsonPropertyName("статус")]
    public string? Статус { get; set; } = "В работе";

    [JsonPropertyName("примечание")]
    public string? Примечание { get; set; }
}

public class ОперацияМлUpdateDto : ОперацияМлCreateDto { }

public class РасчетОперацииDto
{
    [JsonPropertyName("операцияID")]
    public int ОперацияID { get; set; }

    [JsonPropertyName("формулы")]
    public ФормулыРасчетаDto Формулы { get; set; } = new();

    [JsonPropertyName("входныеДанные")]
    public ВходныеДанныеРасчетаDto ВходныеДанные { get; set; } = new();

    [JsonPropertyName("результаты")]
    public РезультатыРасчетаDto Результаты { get; set; } = new();

    [JsonPropertyName("источникКоэффициентаСделки")]
    public ИсточникКоэффициентаDto? ИсточникКоэффициентаСделки { get; set; }
}

public class ФормулыРасчетаDto
{
    [JsonPropertyName("базоваяСумма")]
    public string БазоваяСумма { get; set; } = "Базовая сумма = Количество × Норма времени (ч) × Цена за час (₽/ч)";

    [JsonPropertyName("суммаНадбавки")]
    public string СуммаНадбавки { get; set; } = "Сумма надбавки = (Коэффициент сделки - 1) × Базовая сумма";

    [JsonPropertyName("коэффициентПремии")]
    public string КоэффициентПремии { get; set; } = "Коэффициент премии = 0.8 (если в срок) или 0 (если просрочено)";

    [JsonPropertyName("суммаПремии")]
    public string СуммаПремии { get; set; } = "Сумма премии = Коэффициент премии × (Сумма надбавки + Базовая сумма)";

    [JsonPropertyName("итого")]
    public string Итого { get; set; } = "Итого = Базовая сумма + Сумма надбавки + Сумма премии";
}

public class ВходныеДанныеРасчетаDto
{
    [JsonPropertyName("количество")]
    public int? Количество { get; set; }

    [JsonPropertyName("нормаВремениЧас")]
    public decimal? НормаВремениЧас { get; set; }

    [JsonPropertyName("ценаЗаЧас")]
    public decimal? ЦенаЗаЧас { get; set; }

    [JsonPropertyName("коэффициентСделки")]
    public decimal КоэффициентСделки { get; set; }

    [JsonPropertyName("датаИсполнения")]
    public DateTime? ДатаИсполнения { get; set; }

    [JsonPropertyName("датаЗакрытия")]
    public DateTime? ДатаЗакрытия { get; set; }

    [JsonPropertyName("вСрок")]
    public bool ВСрок { get; set; }
}

public class РезультатыРасчетаDto
{
    [JsonPropertyName("базоваяСумма")]
    public decimal БазоваяСумма { get; set; }

    [JsonPropertyName("суммаНадбавки")]
    public decimal СуммаНадбавки { get; set; }

    [JsonPropertyName("коэффициентПремии")]
    public decimal КоэффициентПремии { get; set; }

    [JsonPropertyName("суммаПремии")]
    public decimal СуммаПремии { get; set; }

    [JsonPropertyName("итого")]
    public decimal Итого { get; set; }
}

public class ИсточникКоэффициентаDto
{
    [JsonPropertyName("найден")]
    public bool Найден { get; set; }

    [JsonPropertyName("коэффициент")]
    public decimal Коэффициент { get; set; }

    [JsonPropertyName("изделие")]
    public string? Изделие { get; set; }

    [JsonPropertyName("дсе")]
    public string? ДСЕ { get; set; }

    [JsonPropertyName("типОперации")]
    public string? ТипОперации { get; set; }

    [JsonPropertyName("датаНачала")]
    public DateTime? ДатаНачала { get; set; }

    [JsonPropertyName("датаОкончания")]
    public DateTime? ДатаОкончания { get; set; }
}
