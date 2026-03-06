using System.Text.Json.Serialization;

namespace WebAPI_OTK.Dtos;

public class ВедомостьВыработкиResponseDto
{
    [JsonPropertyName("период")]
    public string Период { get; set; } = null!;

    [JsonPropertyName("подразделение")]
    public string Подразделение { get; set; } = null!;

    [JsonPropertyName("начало")]
    public DateTime Начало { get; set; }

    [JsonPropertyName("конец")]
    public DateTime Конец { get; set; }

    [JsonPropertyName("данные")]
    public List<ВедомостьВыработкиRowDto> Данные { get; set; } = new();
}

public class ВедомостьВыработкиRowDto
{
    [JsonPropertyName("сотрудникID")]
    public int СотрудникID { get; set; }

    [JsonPropertyName("фио")]
    public string ФИО { get; set; } = null!;

    [JsonPropertyName("всегоЧасов")]
    public decimal ВсегоЧасов { get; set; }

    [JsonPropertyName("количествоОпераций")]
    public int КоличествоОпераций { get; set; }

    [JsonPropertyName("суммаОплаты")]
    public decimal СуммаОплаты { get; set; }
}
