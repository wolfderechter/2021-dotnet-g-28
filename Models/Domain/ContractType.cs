using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class ContractType
    {
        public string Name { get; set; }
        public ContractTypeEnum.CreationMethod CreationMethod { get; set; }
        public bool IsOutsideBusinessHours { get; set; }
        public bool IsActive { get; set; }
        public int MaxResponseTime { get; set; }
        public int MinDuration { get; set; }
        public decimal Price { get; set; }
        public ICollection<Contract> contracts { get; set; }

        public ContractType()
        {

        }
    }
    
}
