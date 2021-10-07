using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppEs.ViewModel.Home
{
    public class AestheticFaultsPercentageViewModel
    {
        public double[] Production { get; set; }
        public double[] Material { get; set; }

        public double[] TotalAestheticProductionFaultPercent { get; set; }

        public double[] TotalAestheticMaterialFaultPercent { get; set; }
    }
}
