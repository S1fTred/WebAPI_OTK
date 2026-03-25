using System.Text.Json.Serialization;

namespace WebAPI_OTK.Dtos;

public class МлDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("номерМЛ")]
    public string? НомерМЛ { get; set; }

    [JsonPropertyName("датаСоздания")]
    public DateTime? ДатаСоздания { get; set; }

    [JsonPropertyName("закрыт")]
    public bool? Закрыт { get; set; }

    [JsonPropertyName("количествоОТК")]
    public int? КоличествоОТК { get; set; }

    [JsonPropertyName("количествоБрак")]
    public int? КоличествоБрак { get; set; }

    [JsonPropertyName("дсеID")]
    public int ДСЕID { get; set; }

    [JsonPropertyName("дсе")]
    public string? ДСЕ { get; set; }

    [JsonPropertyName("изделиеID")]
    public int ИзделиеID { get; set; }

    [JsonPropertyName("изделие")]
    public string? Изделие { get; set; }

    [JsonPropertyName("сотрудникОТКID")]
    public int? СотрудникОТКID { get; set; }

    [JsonPropertyName("сотрудникОТК")]
    public string? СотрудникОТК { get; set; }

    [JsonPropertyName("количествоОпераций")]
    public int КоличествоОпераций { get; set; }
}

public class МлCreateDto
{
    [JsonPropertyName("номерМЛ")]
    public string? НомерМЛ { get; set; }

    // int (обязательные)
    [JsonPropertyName("изделиеID")]
    public int ИзделиеID { get; set; }

    [JsonPropertyName("дсеID")]
    public int ДСЕID { get; set; }
}

public class МлUpdateDto : МлCreateDto
{
    [JsonPropertyName("закрыт")]
    public bool? Закрыт { get; set; }

    [JsonPropertyName("сотрудникОТКID")]
    public int? СотрудникОТКID { get; set; }

    [JsonPropertyName("количествоОТК")]
    public int? КоличествоОТК { get; set; }

    [JsonPropertyName("количествоБрак")]
    public int? КоличествоБрак { get; set; }
}

public class ЗакрытиеМлDto
{
    [JsonPropertyName("сотрудникОТКID")]
    public int СотрудникОТКID { get; set; }

    [JsonPropertyName("количествоОТК")]
    public int КоличествоОТК { get; set; }

    [JsonPropertyName("количествоБрак")]
    public int КоличествоБрак { get; set; }
}
