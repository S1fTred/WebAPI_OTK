namespace WebAPI_OTK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class Зарплата
    {
        public int ID { get; set; }

        public int? ОперацияМЛID { get; set; }

        public int? СотрудникID { get; set; }

        public decimal? ЧасыОтработано { get; set; }

        public decimal? СтавкаЧасовая { get; set; }

        public decimal? СуммаОклад { get; set; }

        public decimal? Премия { get; set; }

        public decimal? ИтогоКВыплате { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Период { get; set; }

        public virtual Операция_МЛ Операция_МЛ { get; set; }

        public virtual Сотрудник Сотрудник { get; set; }
    }
}
