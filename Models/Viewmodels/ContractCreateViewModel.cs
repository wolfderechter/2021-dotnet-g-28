using _2021_dotnet_g_28.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _2021_dotnet_g_28.Models.Viewmodels
{
    public class ContractCreateViewModel
    {
        [Required]
      //  [Range(1, 3, ErrorMessage = "The duration has to be 1,2 or 3 years")]
       [Display(Name = "duration", ResourceType  = typeof(Resources.Models.Viewmodels.ContractCreateViewModel))]
        public int duration { get; set; }
        [Required]
        // [Display(Name = "typeName", ResourceType = typeof(Resources))]
        public string TypeName { get; set; }
        
        public IEnumerable<ContractType> ContractTypes { get; set; }
    }
}
