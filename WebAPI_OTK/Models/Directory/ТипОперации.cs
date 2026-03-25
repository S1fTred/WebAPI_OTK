namespace WebAPI_OTK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
 

    public partial class ТипОперации
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ТипОперации()
        {
            Операция_МЛ = new HashSet<Операция_МЛ>();
            ПремиальныеКоэффициенты = new HashSet<ПремиальныеКоэффициенты>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Наименование { get; set; }

        [StringLength(500)]
        public string Описание { get; set; }

        public decimal? ДлительностьЧас { get; set; }

        [StringLength(50)]
        public string КодОперации { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Операция_МЛ> Операция_МЛ { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ПремиальныеКоэффициенты> ПремиальныеКоэффициенты { get; set; }
    }
}
