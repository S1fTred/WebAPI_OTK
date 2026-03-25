namespace WebAPI_OTK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class ПремиальныеКоэффициенты
    {
        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Наименование { get; set; }

        public decimal Коэффициент { get; set; }

        [Column(TypeName = "date")]
        public DateTime ДатаНачала { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ДатаОкончания { get; set; }

        public int? ИзделиеID { get; set; }

        public int? ДСЕID { get; set; }

        public int? ТипОперацииID { get; set; }

        public virtual ДСЕ ДСЕ { get; set; }

        public virtual Изделие Изделие { get; set; }

        public virtual ТипОперации ТипОперации { get; set; }
    }
}
