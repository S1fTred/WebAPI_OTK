namespace WebAPI_OTK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class Сотрудник
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Сотрудник()
        {
            Зарплата = new HashSet<Зарплата>();
            МЛ = new HashSet<МЛ>();
            Операция_МЛ = new HashSet<Операция_МЛ>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string ФИО { get; set; }

        public int? ДолжностьID { get; set; }

        [StringLength(50)]
        public string ТабельныйНомер { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ДатаПриема { get; set; }

        public virtual Должность Должность { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Зарплата> Зарплата { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<МЛ> МЛ { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Операция_МЛ> Операция_МЛ { get; set; }
    }
}
