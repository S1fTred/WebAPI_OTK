namespace WebAPI_OTK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class МЛ
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public МЛ()
        {
            Операция_МЛ = new HashSet<Операция_МЛ>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string НомерМЛ { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ДатаСоздания { get; set; }

        public bool? Закрыт { get; set; }

        public int? СотрудникОТК { get; set; }

        public int? КоличествоОТК { get; set; }

        public int? КоличествоБрак { get; set; }

        public int ИзделиеID { get; set; }

        public int ДСЕID { get; set; }

        public virtual ДСЕ ДСЕ { get; set; }

        public virtual Изделие Изделие { get; set; }

        public virtual Сотрудник Сотрудник { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Операция_МЛ> Операция_МЛ { get; set; }
    }
}
