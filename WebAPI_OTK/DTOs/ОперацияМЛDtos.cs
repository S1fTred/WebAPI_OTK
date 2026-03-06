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

    [JsonPropertyName("оборудованиеID")]
    public int? ОборудованиеID { get; set; }

    [JsonPropertyName("оборудование")]
    public string? Оборудование { get; set; }

    [JsonPropertyName("датаНачала")]
    public DateTime ДатаНачала { get; set; }

    [JsonPropertyName("датаОкончания")]
    public DateTime? ДатаОкончания { get; set; }

    [JsonPropertyName("фактическаяДлительностьЧас")]
    public decimal? ФактическаяДлительностьЧас { get; set; }

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

    [JsonPropertyName("датаНачала")]
    public DateTime ДатаНачала { get; set; }

    [JsonPropertyName("датаОкончания")]
    public DateTime? ДатаОкончания { get; set; }

    [JsonPropertyName("фактическаяДлительностьЧас")]
    public decimal? ФактическаяДлительностьЧас { get; set; }

    [JsonPropertyName("подразделение")]
    public string? Подразделение { get; set; }

    [JsonPropertyName("статус")]
    public string? Статус { get; set; } = "В работе";

    [JsonPropertyName("примечание")]
    public string? Примечание { get; set; }
}

public class ОперацияМлUpdateDto : ОперацияМлCreateDto { }
