using System.Text.Json.Serialization;

namespace WebAPI_OTK.Dtos;

// ---------- READ DTO ----------
public class ИзделиеDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("наименование")]
    public string Наименование { get; set; } = null!;

    [JsonPropertyName("описание")]
    public string? Описание { get; set; }

    [JsonPropertyName("состояние")]
    public string? Состояние { get; set; }

    [JsonPropertyName("датаСоздания")]
    public DateTime? ДатаСоздания { get; set; }

    [JsonPropertyName("количествоДСЕ")]
    public int КоличествоДСЕ { get; set; }

    [JsonPropertyName("количествоМЛ")]
    public int КоличествоМЛ { get; set; }
}

// Детальная карточка (для GET /{id})
public class ИзделиеDetailDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("наименование")]
    public string Наименование { get; set; } = null!;

    [JsonPropertyName("описание")]
    public string? Описание { get; set; }

    [JsonPropertyName("состояние")]
    public string? Состояние { get; set; }

    [JsonPropertyName("датаСоздания")]
    public DateTime? ДатаСоздания { get; set; }

    [JsonPropertyName("дсе")]
    public List<ДсеShortDto> ДСЕ { get; set; } = new();
}

public class ДсеShortDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("код")]
    public string? Код { get; set; }

    [JsonPropertyName("наименование")]
    public string? Наименование { get; set; }

    [JsonPropertyName("чертеж")]
    public string? Чертеж { get; set; }
}

// Для GET /{id}/дсе (с количеством МЛ)
public class ДсеListItemDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("код")]
    public string? Код { get; set; }

    [JsonPropertyName("наименование")]
    public string? Наименование { get; set; }

    [JsonPropertyName("чертеж")]
    public string? Чертеж { get; set; }

    [JsonPropertyName("количествоМЛ")]
    public int КоличествоМЛ { get; set; }
}

// Для GET /{id}/мл
public class МлListItemDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("номерМЛ")]
    public string? НомерМЛ { get; set; }

    [JsonPropertyName("датаСоздания")]
    public DateTime? ДатаСоздания { get; set; }

    [JsonPropertyName("закрыт")]
    public bool? Закрыт { get; set; }

    [JsonPropertyName("дсе")]
    public string? ДСЕ { get; set; }

    [JsonPropertyName("сотрудникОТК")]
    public string? СотрудникОТК { get; set; }

    [JsonPropertyName("количествоОпераций")]
    public int КоличествоОпераций { get; set; }
}

// Для карты техпланирования
public class КартаТехпланированияDto
{
    [JsonPropertyName("изделие")]
    public string Изделие { get; set; } = null!;

    [JsonPropertyName("дсе")]
    public List<КартаДсеDto> ДСЕ { get; set; } = new();
}

public class КартаДсеDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("код")]
    public string? Код { get; set; }

    [JsonPropertyName("наименование")]
    public string? Наименование { get; set; }

    [JsonPropertyName("операции")]
    public List<КартаОперацияDto> Операции { get; set; } = new();
}

public class КартаОперацияDto
{
    [JsonPropertyName("типОперации")]
    public string? ТипОперации { get; set; }

    [JsonPropertyName("датаНачала")]
    public DateTime? ДатаНачала { get; set; }

    [JsonPropertyName("датаОкончания")]
    public DateTime? ДатаОкончания { get; set; }

    [JsonPropertyName("длительностьЧас")]
    public decimal? ДлительностьЧас { get; set; }
}

// ---------- CREATE / UPDATE ----------
public class ИзделиеCreateDto
{
    [JsonPropertyName("наименование")]
    public string Наименование { get; set; } = null!;

    [JsonPropertyName("описание")]
    public string? Описание { get; set; }

    [JsonPropertyName("состояние")]
    public string? Состояние { get; set; }
}

public class ИзделиеUpdateDto : ИзделиеCreateDto { }

// ---------- Поиск / Импорт ----------
public class ПоискИзделийDto
{
    [JsonPropertyName("наименование")]
    public string? Наименование { get; set; }

    [JsonPropertyName("состояние")]
    public string? Состояние { get; set; }

    [JsonPropertyName("датаС")]
    public DateTime? ДатаС { get; set; }

    [JsonPropertyName("датаПо")]
    public DateTime? ДатаПо { get; set; }
}

public class ИзделиеSGDDto
{
    [JsonPropertyName("кодИзделия")]
    public string? КодИзделия { get; set; }

    [JsonPropertyName("наименование")]
    public string Наименование { get; set; } = null!;

    [JsonPropertyName("описание")]
    public string? Описание { get; set; }

    [JsonPropertyName("статус")]
    public string? Статус { get; set; }
}
