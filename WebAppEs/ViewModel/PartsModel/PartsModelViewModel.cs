using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppEs.ViewModel.PartsModel
{
    public class PartsModelViewModel
    {
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string IsUpdate { get; set; }
    }
}
