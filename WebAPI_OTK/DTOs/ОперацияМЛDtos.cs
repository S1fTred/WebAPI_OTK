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
