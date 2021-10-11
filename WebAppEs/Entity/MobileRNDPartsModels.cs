using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppEs.Entity
{
    [Table(name: "MobileRNDPartsModels")]
    public class MobileRNDPartsModels
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(150)]
        public string ModelName { get; set; }


        [StringLength(150)]
        public string Supplier { get; set; }
    }
}
