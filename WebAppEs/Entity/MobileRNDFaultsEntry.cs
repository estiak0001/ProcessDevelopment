using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebAppEs.Models;

namespace WebAppEs.Entity
{
    [Table(name: "MobileRNDFaultsEntry")]
    public class MobileRNDFaultsEntry
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeID { get; set; }

        //[Required]
        //[StringLength(50)]
        //[ForeignKey("ApplicationUser")]
        //public Guid UserID { get; set; }

        public Guid UserID { get; set; }

        [Required]
        [StringLength(50)]
        public DateTime? Date { get; set; }
        [Required]
        [StringLength(50)]
        public string LineNo { get; set; }
        [Required]
        [StringLength(50)]
        [ForeignKey("MobileRNDPartsModels")]
        public Guid PartsModelID { get; set; }
        public virtual MobileRNDPartsModels MobileRNDPartsModels { get; set; }

        [Required]
        [StringLength(50)]
        public string LotNo { get; set; }

        [Required]
        [StringLength(50)]
        public int FuncMaterialFault { get; set; }
        [Required]
        [StringLength(50)]
        public int FuncProductionFault { get; set; }
        [Required]
        [StringLength(50)]
        public int FuncSoftwareFault { get; set; }
        [Required]
        [StringLength(50)]
        public int AesthMaterialFault { get; set; }
        [Required]
        [StringLength(50)]
        public int AesthProductionFault { get; set; }
        [Required]
        [StringLength(50)]
        public int TotalFunctionalFault { get; set; }
        [Required]
        [StringLength(50)]
        public int TotalAestheticFault { get; set; }
        [Required]
        [StringLength(50)]
        public int TotaCheckedQty { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }
    }
}