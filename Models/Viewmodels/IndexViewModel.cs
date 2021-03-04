using _2021_dotnet_g_28.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Viewmodels
{
    public class IndexViewModel
    { 
        public List<StatusModel> CheckBoxItems { get; set; }
        public List<DuurModel> DuurCheckbox { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
    }
}
