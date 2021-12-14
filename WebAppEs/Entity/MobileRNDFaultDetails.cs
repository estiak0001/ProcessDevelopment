using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebAppEs.Models;

namespace WebAppEs.Entity
{
    public class MobileRNDFaultDetails
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeID { get; set; }

        [ForeignKey("MobileRNDFaultsEntry")]
        public Guid FaultEntryID { get; set; }
        public virtual MobileRNDFaultsEntry MobileRNDFaultsEntry { get; set; }
        public Guid  ApplicationUserID { get; set; }

        [Required]
        [StringLength(50)]
        public DateTime? Date { get; set; }

        //[Required]
        //[StringLength(50)]
        //public string LineNo { get; set; }

        //[Required]
        //[StringLength(50)]
        //[ForeignKey("MobileRNDPartsModels")]
        //public Guid PartsModelID { get; set; }
        //public virtual MobileRNDPartsModels MobileRNDPartsModels { get; set; }

        //[Required]
        //[StringLength(50)]
        //public string LotNo { get; set; }

        [Required]
        [StringLength(250)]
        public string FaultType { get; set; }

        [Required]
        [StringLength(50)]
        public int FaultQty { get; set; }

        
        public string RootCause { get; set; }
        public string Solution { get; set; }
        public string Remarks { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }
    }
}
