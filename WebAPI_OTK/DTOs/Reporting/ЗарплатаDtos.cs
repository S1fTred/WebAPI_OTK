using System.Text.Json.Serialization;

namespace WebAPI_OTK.Dtos;

// ----------- Read DTO (то, что отдаём наружу) -----------
public class ЗарплатаDto
{
    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("операцияМЛID")]
    public int? ОперацияМЛID { get; set; }

    [JsonPropertyName("сотрудникID")]
    public int? СотрудникID { get; set; }

    [JsonPropertyName("период")]
    public DateTime? Период { get; set; }

    [JsonPropertyName("часыОтработано")]
    public decimal? ЧасыОтработано { get; set; }

    [JsonPropertyName("ставкаЧасовая")]
    public decimal? СтавкаЧасовая { get; set; }

    [JsonPropertyName("суммаОклад")]
    public decimal? СуммаОклад { get; set; }

    [JsonPropertyName("премия")]
    public decimal? Премия { get; set; }

    [JsonPropertyName("итогоКВыплате")]
    public decimal? ИтогоКВыплате { get; set; }

    // удобные поля
    [JsonPropertyName("сотрудник")]
    public string? Сотрудник { get; set; }

    [JsonPropertyName("номерМЛ")]
    public string? НомерМЛ { get; set; }
}

// ----------- Create / Update DTO -----------
public class ЗарплатаCreateDto
{
    [JsonPropertyName("операцияМЛID")]
    public int? ОперацияМЛID { get; set; }

    [JsonPropertyName("сотрудникID")]
    public int? СотрудникID { get; set; }

    [JsonPropertyName("период")]
    public DateTime? Период { get; set; }

    [JsonPropertyName("часыОтработано")]
    public decimal? ЧасыОтработано { get; set; }

    [JsonPropertyName("ставкаЧасовая")]
    public decimal? СтавкаЧасовая { get; set; }

    [JsonPropertyName("суммаОклад")]
    public decimal? СуммаОклад { get; set; }

    [JsonPropertyName("премия")]
    public decimal? Премия { get; set; }

    [JsonPropertyName("итогоКВыплате")]
    public decimal? ИтогоКВыплате { get; set; }
}

public class ЗарплатаUpdateDto : ЗарплатаCreateDto { }

// ----------- Спец DTO для расчёта -----------
public class РасчетЗарплатыDto
{
    [JsonPropertyName("началоПериода")]
    public DateTime НачалоПериода { get; set; }

    [JsonPropertyName("конецПериода")]
    public DateTime КонецПериода { get; set; }

    [JsonPropertyName("включатьПремии")]
    public bool ВключатьПремии { get; set; } = true;

    [JsonPropertyName("включатьНадбавки")]
    public bool ВключатьНадбавки { get; set; } = true;
}

// ----------- DTO для экспорта в Комтех -----------
public class КомтехExportRowDto
{
    [JsonPropertyName("табельныйНомер")]
    public string? ТабельныйНомер { get; set; }

    [JsonPropertyName("видОплаты")]
    public string ВидОплаты { get; set; } = "011";

    [JsonPropertyName("сумма")]
    public decimal Сумма { get; set; }

    [JsonPropertyName("датаНачисления")]
    public DateTime? ДатаНачисления { get; set; }
}
