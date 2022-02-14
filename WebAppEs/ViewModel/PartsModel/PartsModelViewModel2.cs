using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAppEs.ViewModel.Supplier;

namespace WebAppEs.ViewModel.PartsModel
{
    public class PartsModelViewModel2
    {
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid SupplierId { get; set; }

        public string Supplier { get; set; }

        public string IsUpdate { get; set; }

        public List<MobileRNDSupplier_VM> supplier { get; set; }
    }
}
