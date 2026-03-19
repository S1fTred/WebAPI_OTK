namespace WebAPI_OTK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class Операция_МЛ
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Операция_МЛ()
        {
            Зарплата = new HashSet<Зарплата>();
        }

        public int ID { get; set; }

        public int МЛID { get; set; }

        public int ТипОперацииID { get; set; }

        public int? ОборудованиеID { get; set; }

        public int СотрудникID { get; set; }

        public int? Количество { get; set; }

        public decimal? НормаВремениЧас { get; set; }

        [StringLength(50)]
        public string? НазваниеТарифа { get; set; }

        public decimal? ЦенаЗаЧас { get; set; }

        public DateTime? ДатаИсполнения { get; set; }

        public DateTime? ДатаЗакрытия { get; set; }

        [StringLength(100)]
        public string? Подразделение { get; set; }

        [StringLength(50)]
        public string? Статус { get; set; }

        [StringLength(500)]
        public string? Примечание { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Зарплата> Зарплата { get; set; }

        public virtual МЛ? МЛ { get; set; }

        public virtual Оборудование? Оборудование { get; set; }

        public virtual Сотрудник? Сотрудник { get; set; }

        public virtual ТипОперации? ТипОперации { get; set; }
    }
}
