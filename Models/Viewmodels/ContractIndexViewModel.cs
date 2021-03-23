using _2021_dotnet_g_28.Models.Domain;
using System.Collections.Generic;

namespace _2021_dotnet_g_28.Models.Viewmodels
{
    public class ContractIndexViewModel
    {
        public List<StatusModel> CheckBoxItems { get; set; }
        public List<DuurModel> DuurCheckbox { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
    }
}
