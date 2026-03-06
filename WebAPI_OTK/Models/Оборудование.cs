namespace WebAPI_OTK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class Оборудование
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Оборудование()
        {
            Операция_МЛ = new HashSet<Операция_МЛ>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Наименование { get; set; }

        [StringLength(100)]
        public string ИнвентарныйНомер { get; set; }

        [StringLength(200)]
        public string Местоположение { get; set; }

        [StringLength(50)]
        public string Статус { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ДатаПоследнегоОбслуживания { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Операция_МЛ> Операция_МЛ { get; set; }
    }
}
