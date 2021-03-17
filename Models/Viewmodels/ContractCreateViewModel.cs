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
        [Range(1, 3, ErrorMessage = "The duration has to be 1,2 or 3 years")]
        [Display(Name ="Duration of contract in years (1,2 or 3)")]
        public int duration { get; set; }
        [Required]
        [Display(Name = "Choose one of the contractypes shown below")]
        public string TypeName { get; set; }
    public IEnumerable<ContractType> ContractTypes { get; set; }
    }
}
