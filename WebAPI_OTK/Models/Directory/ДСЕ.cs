namespace WebAPI_OTK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class ДСЕ
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ДСЕ()
        {
            МЛ = new HashSet<МЛ>();
            ПремиальныеКоэффициенты = new HashSet<ПремиальныеКоэффициенты>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Код { get; set; }

        [StringLength(200)]
        public string Наименование { get; set; }

        [StringLength(500)]
        public string Чертеж { get; set; }

        public int? ИзделиеID { get; set; }

        public virtual Изделие Изделие { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<МЛ> МЛ { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ПремиальныеКоэффициенты> ПремиальныеКоэффициенты { get; set; }
    }
}
