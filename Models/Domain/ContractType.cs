using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class ContractType
    {
        [Display(Name = "Name Contract type")]
        public string Name { get; set; }
        [Display(Name = "Creation method")]
        public ContractTypeEnum.CreationMethod CreationMethod { get; set; }
        [Display(Name = "Availability")]
        public bool IsOutsideBusinessHours { get; set; }
        [Display(Name = "Creation method")]
        public bool IsActive { get; set; }
        [Display(Name = "Max. respons time")]
        public int MaxResponseTime { get; set; }
        [Display(Name = "Min. duration")]
        public int MinDuration { get; set; }
        [Display(Name = "Price/Month")]
        public decimal Price { get; set; }
        public ICollection<Contract> contracts { get; set; }

        public ContractType()
        {

        }
    }
    
}
