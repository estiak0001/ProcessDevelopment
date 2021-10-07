using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAppEs.ViewModel.PartsModel;

namespace WebAppEs.ViewModel.FaultsEntry
{
    public class MobileRNDFaultsEntryViewModel
    {
        public Guid Id { get; set; }
        public string EmployeeID { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }


        public string DateString { get; set; }
        [Required]
        public string LineNo { get; set; }

        public string Line { get; set; }
        public string ModelName { get; set; }
        public string ModelNameWithLot { get; set; }
        [Required]
        public Guid PartsModelID { get; set; }
        [Required]
        public string LotNo { get; set; }
        public string Lot { get; set; }

        public int? FuncMaterialFault { get; set; }

        public int? FuncProductionFault { get; set; }

        public int? FuncSoftwareFault { get; set; }

        public int? AesthMaterialFault { get; set; }

        public int? AesthProductionFault { get; set; }

        public double? FuncMaterialFaultd { get; set; }

        public double? FuncProductionFaultd { get; set; }

        public double? FuncSoftwareFaultd { get; set; }

        public double? AesthMaterialFaultd { get; set; }

        public double? AesthProductionFaultd{ get; set; }

        public int TotalFunctionalFault { get; set; }

        public int TotalAestheticFault { get; set; }
        public double? TotalFunctionalFaultd { get; set; }

        public double? TotalAestheticFaultd { get; set; }
        public int? TotalCheckedQty { get; set; }

        public IEnumerable<PartsModelViewModel> PartsModelViewModel { get; set; }
        public IEnumerable<MobileRNDFaultsEntryViewModel> MobileRNDFaultsEntryViewModelList { get; set; }
        public string ButtonText { get; set; }
        public string Disabled { get; set; }
        public DateTime CreateDate { get; set; }

        public IEnumerable<MobileRNDFaultDetailsViewModel> MobileRNDFaultDetailsViewModel { get; set; }
        public int TotalFuncAes { get; set; }
        public bool StatusIsToday { get; set; }
        public Guid UserID { get; set; }
    }
}
