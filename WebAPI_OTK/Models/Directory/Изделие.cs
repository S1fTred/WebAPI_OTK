namespace WebAPI_OTK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class Изделие
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Изделие()
        {
            ДСЕ = new HashSet<ДСЕ>();
            МЛ = new HashSet<МЛ>();
            ПремиальныеКоэффициенты = new HashSet<ПремиальныеКоэффициенты>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Наименование { get; set; }

        [StringLength(1000)]
        public string Описание { get; set; }

        [StringLength(50)]
        public string Состояние { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ДатаСоздания { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ДСЕ> ДСЕ { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<МЛ> МЛ { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ПремиальныеКоэффициенты> ПремиальныеКоэффициенты { get; set; }
    }
}
