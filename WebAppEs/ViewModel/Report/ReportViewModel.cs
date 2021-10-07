using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAppEs.ViewModel.PartsModel;
using WebAppEs.ViewModel.Register;

namespace WebAppEs.ViewModel.Report
{
    public class ReportViewModel
    {
        public string EmployeeID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public string LineNo { get; set; }
        public Guid PartsModelID { get; set; }

        public string LotNo { get; set; }

        public IEnumerable<PartsModelViewModel> PartsModelViewModel { get; set; }
        public IEnumerable<EmployeeListVM> EmployeeListVM { get; set; }


        public bool WithQty { get; set; }

        public bool WithPercentage { get; set; }
    }
}
