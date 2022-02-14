using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppEs.Entity
{
    [Table(name: "MobileRNDSupplier")]
    public class MobileRNDSupplier
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(150)]
        public string SupplierName { get; set; }
    }
}
