namespace WebAPI_OTK.Dtos
{
    public class ТипОперацииDto
    {
        public int ID { get; set; }
        public string Наименование { get; set; } = string.Empty;
        public string? Описание { get; set; }
        public decimal? ДлительностьЧас { get; set; }
        public string? КодОперации { get; set; }
    }
}
