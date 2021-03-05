using _2021_dotnet_g_28.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Viewmodels
{
    public class ContractCreateViewModel
    {
        [Required]
        public int duration { get; set; }
        [Required]
        public string TypeName { get; set; }
    public IEnumerable<ContractType> ContractTypes { get; set; }
    }
}
