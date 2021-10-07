using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppEs.ViewModel.Home
{
    public class FunctionalFaultsPercentageViewModel
    {
        public double[] Production { get; set; }
        public double[] Material { get; set; }
        public double[] Software { get; set; }

        public double[] TotalFuncProductionFaultPercent { get; set; }

        public double[] TotalFuncMaterialFaultPercent { get; set; }

        public double[] TotalFuncSoftwareFaultPercent { get; set; }
    }
}
