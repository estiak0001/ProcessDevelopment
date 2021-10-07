using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppEs.ViewModel.Report
{
    public class DetailsReportViewModel
    {
        public string LineWithModel { get; set; }
        public string FaultType { get; set; }
        public int FaultQty { get; set; }
        public string RootCause { get; set; }
        public string Solution { get; set; }
        public string Remarks { get; set; }
        public string LineNo { get; set; }
    }
}
