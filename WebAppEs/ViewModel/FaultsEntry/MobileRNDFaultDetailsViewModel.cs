using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppEs.ViewModel.FaultsEntry
{
    public class MobileRNDFaultDetailsViewModel
    {
        public Guid FaultEntryId { get; set; }
        public string EmployeeID { get; set; }

        public DateTime? Date { get; set; }

        public string FaultType { get; set; }

        public int FaultQty { get; set; }


        public string RootCause { get; set; }
        public string Solution { get; set; }
        public string Remarks { get; set; }
        public Guid UserID { get; set; }
    }
}
