using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAppEs.ViewModel.Home
{
    public class DashboasrViewModel
    {
        //public DateTime Date { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public int TodayTF { get; set; }
        public int TodayTFFunc { get; set; }
        public int TodayTFAes { get; set; }
        public int LastDayTF { get; set; }
        public int LastDayTFFunc { get; set; }
        public int LastDayTFAes { get; set; }
        public int LastWeekTF { get; set; }
        public int LastWeekTFFunc { get; set; }
        public int LastWeekTFAes { get; set; }
        public int LastMonthTF { get; set; }
        public int LastMonthTFFunc { get; set; }
        public int LastMonthTFAes { get; set; }

        public int TodayTotalCheck { get; set; }
        public int LastDayTotalCheck { get; set; }
        public int LastWeekTotalCheck { get; set; }
        public int LastMonthTotalCheck { get; set; }

        public ChartLevelViewModel Lavel { get; set; }
        public FunctionalFaultsPercentageViewModel FunctionalFaultsPercentageViewModel { get; set; }
        public AestheticFaultsPercentageViewModel AestheticFaultsPercentageViewModel { get; set; }


        


    }
}
