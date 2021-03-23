using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Viewmodels
{
    public class StatusModel
    {
        public Domain.ContractEnum.status Status { get; set; }
        public bool IsSelected { get; set; }
    }
}
