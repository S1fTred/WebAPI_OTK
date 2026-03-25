using System.Text.Json.Serialization;

namespace WebAPI_OTK.Dtos;

public class ДолжностьDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("наименование")]
    public string Наименование { get; set; } = null!;

    [JsonPropertyName("код")]
    public string? Код { get; set; }

    [JsonPropertyName("сотрудник")]
    public List<СотрудникКороткоDto> Сотрудник { get; set; } = new();
}

public class СотрудникКороткоDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("фио")]
    public string ФИО { get; set; } = null!;
}

public class ДолжностьCreateDto
{
    [JsonPropertyName("наименование")]
    public string Наименование { get; set; } = null!;

    [JsonPropertyName("код")]
    public string? Код { get; set; }
}

public class ДолжностьUpdateDto : ДолжностьCreateDto { }
