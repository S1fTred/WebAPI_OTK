using System.Text.Json.Serialization;

namespace WebAPI_OTK.Dtos;

public class СотрудникDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("фио")]
    public string ФИО { get; set; } = null!;

    [JsonPropertyName("табельныйНомер")]
    public string? ТабельныйНомер { get; set; }

    [JsonPropertyName("датаПриема")]
    public DateTime? ДатаПриема { get; set; }

    [JsonPropertyName("должностьID")]
    public int? ДолжностьID { get; set; }

    [JsonPropertyName("должность")]
    public string? Должность { get; set; }
}

// create/update — без навигаций
public class СотрудникCreateDto
{
    [JsonPropertyName("фио")]
    public string ФИО { get; set; } = null!;

    [JsonPropertyName("табельныйНомер")]
    public string? ТабельныйНомер { get; set; }

    [JsonPropertyName("датаПриема")]
    public DateTime? ДатаПриема { get; set; }

    [JsonPropertyName("должностьID")]
    public int? ДолжностьID { get; set; }
}

public class СотрудникUpdateDto : СотрудникCreateDto { }
