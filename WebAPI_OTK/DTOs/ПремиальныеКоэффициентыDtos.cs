using System.Text.Json.Serialization;

namespace WebAPI_OTK.Dtos;

// -------- READ --------
public class ПремиальныйКоэффициентDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("наименование")]
    public string Наименование { get; set; } = null!;

    [JsonPropertyName("коэффициент")]
    public decimal Коэффициент { get; set; }

    [JsonPropertyName("датаНачала")]
    public DateTime ДатаНачала { get; set; }

    [JsonPropertyName("датаОкончания")]
    public DateTime? ДатаОкончания { get; set; }

    [JsonPropertyName("активный")]
    public bool Активный { get; set; }

    [JsonPropertyName("изделиеID")]
    public int? ИзделиеID { get; set; }

    [JsonPropertyName("изделие")]
    public string? Изделие { get; set; }

    [JsonPropertyName("дсеID")]
    public int? ДСЕID { get; set; }

    [JsonPropertyName("дсе")]
    public string? ДСЕ { get; set; }

    [JsonPropertyName("типОперацииID")]
    public int? ТипОперацииID { get; set; }

    [JsonPropertyName("типОперации")]
    public string? ТипОперации { get; set; }
}

// Для GET /активные (можно тем же, но оставлю единый DTO)
public class ПремиальныйКоэффициентShortDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("наименование")]
    public string Наименование { get; set; } = null!;

    [JsonPropertyName("коэффициент")]
    public decimal Коэффициент { get; set; }

    [JsonPropertyName("датаНачала")]
    public DateTime ДатаНачала { get; set; }

    [JsonPropertyName("датаОкончания")]
    public DateTime? ДатаОкончания { get; set; }

    [JsonPropertyName("изделие")]
    public string? Изделие { get; set; }

    [JsonPropertyName("дсе")]
    public string? ДСЕ { get; set; }

    [JsonPropertyName("типОперации")]
    public string? ТипОперации { get; set; }
}

// -------- CREATE / UPDATE --------
public class ПремиальныйКоэффициентCreateDto
{
    [JsonPropertyName("наименование")]
    public string Наименование { get; set; } = null!;

    [JsonPropertyName("коэффициент")]
    public decimal Коэффициент { get; set; }

    [JsonPropertyName("датаНачала")]
    public DateTime? ДатаНачала { get; set; }

    [JsonPropertyName("датаОкончания")]
    public DateTime? ДатаОкончания { get; set; }

    [JsonPropertyName("изделиеID")]
    public int? ИзделиеID { get; set; }

    [JsonPropertyName("дсеID")]
    public int? ДСЕID { get; set; }

    [JsonPropertyName("типОперацииID")]
    public int? ТипОперацииID { get; set; }
}

public class ПремиальныйКоэффициентUpdateDto : ПремиальныйКоэффициентCreateDto { }

// -------- “для-операции” --------
public class КоэффициентДляОперацииResponseDto
{
    [JsonPropertyName("коэффициент")]
    public decimal Коэффициент { get; set; }

    [JsonPropertyName("наименование")]
    public string Наименование { get; set; } = null!;

    [JsonPropertyName("применяетсяС")]
    public DateTime ПрименяетсяС { get; set; }
}

// -------- поиск / массовое обновление --------
public class ПоискКоэффициентовDto
{
    [JsonPropertyName("толькоАктивные")]
    public bool ТолькоАктивные { get; set; } = true;

    [JsonPropertyName("изделиеID")]
    public int? ИзделиеID { get; set; }

    [JsonPropertyName("дсеID")]
    public int? ДСЕID { get; set; }

    [JsonPropertyName("типОперацииID")]
    public int? ТипОперацииID { get; set; }

    [JsonPropertyName("коэффициентОт")]
    public decimal? КоэффициентОт { get; set; }

    [JsonPropertyName("коэффициентДо")]
    public decimal? КоэффициентДо { get; set; }
}

public class МассовоеОбновлениеDto
{
    [JsonPropertyName("изделиеID")]
    public int? ИзделиеID { get; set; }

    [JsonPropertyName("дсеID")]
    public int? ДСЕID { get; set; }

    [JsonPropertyName("типОперацииID")]
    public int? ТипОперацииID { get; set; }

    [JsonPropertyName("датаНачала")]
    public DateTime? ДатаНачала { get; set; }

    [JsonPropertyName("датаОкончания")]
    public DateTime? ДатаОкончания { get; set; }

    [JsonPropertyName("коэффициенты")]
    public List<НовыйКоэффициентDto> Коэффициенты { get; set; } = new();
}

public class НовыйКоэффициентDto
{
    [JsonPropertyName("наименование")]
    public string Наименование { get; set; } = null!;

    [JsonPropertyName("коэффициент")]
    public decimal Коэффициент { get; set; }
}
