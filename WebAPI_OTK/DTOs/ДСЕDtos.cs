using System.Text.Json.Serialization;

namespace WebAPI_OTK.Dtos;

// -------- READ --------
public class ДсеDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("код")]
    public string Код { get; set; } = null!;

    [JsonPropertyName("наименование")]
    public string? Наименование { get; set; }

    [JsonPropertyName("чертеж")]
    public string? Чертеж { get; set; }

    [JsonPropertyName("изделиеID")]
    public int? ИзделиеID { get; set; }

    [JsonPropertyName("изделие")]
    public string? Изделие { get; set; }

    [JsonPropertyName("количествоМЛ")]
    public int КоличествоМЛ { get; set; }
}

// -------- CREATE / UPDATE --------
public class ДсеCreateDto
{
    [JsonPropertyName("код")]
    public string Код { get; set; } = null!;

    [JsonPropertyName("наименование")]
    public string? Наименование { get; set; }

    [JsonPropertyName("чертеж")]
    public string? Чертеж { get; set; }

    [JsonPropertyName("изделиеID")]
    public int? ИзделиеID { get; set; }
}

public class ДсеUpdateDto : ДсеCreateDto { }

// -------- ПАГИНАЦИЯ --------
public class ДсеPagedResponseDto
{
    [JsonPropertyName("данные")]
    public List<ДсеDto> Данные { get; set; } = new();

    [JsonPropertyName("страница")]
    public int Страница { get; set; }

    [JsonPropertyName("размерСтраницы")]
    public int РазмерСтраницы { get; set; }

    [JsonPropertyName("всегоЗаписей")]
    public int ВсегоЗаписей { get; set; }

    [JsonPropertyName("всегоСтраниц")]
    public int ВсегоСтраниц { get; set; }
}
