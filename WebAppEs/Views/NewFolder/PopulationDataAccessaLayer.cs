using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppEs.Views.NewFolder
{
    public class PopulationDataAccessaLayer
    {
        public static List<ChartViewModel> GetCityPopulationList()
        {
            var list = new List<ChartViewModel>();
            list.Add(new ChartViewModel { CityName = "PURI", PopulationYear2020 = 28000, PopulationYear2010 = 45000, PopulationYear2000 = 22000, PopulationYear1990 = 50000 });
            list.Add(new ChartViewModel { CityName = "Bhubaneswar", PopulationYear2020 = 30000, PopulationYear2010 = 49000, PopulationYear2000 = 24000, PopulationYear1990 = 39000 });
            list.Add(new ChartViewModel { CityName = "Cuttack", PopulationYear2020 = 35000, PopulationYear2010 = 56000, PopulationYear2000 = 26000, PopulationYear1990 = 41000 });
            list.Add(new ChartViewModel { CityName = "Berhampur", PopulationYear2020 = 37000, PopulationYear2010 = 44000, PopulationYear2000 = 28000, PopulationYear1990 = 48000 });
            list.Add(new ChartViewModel { CityName = "Odisha", PopulationYear2020 = 40000, PopulationYear2010 = 38000, PopulationYear2000 = 30000, PopulationYear1990 = 54000 });

            return list;

        }
    }
}
