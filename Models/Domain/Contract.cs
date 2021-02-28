using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class Contract
    {
        #region properties
        public DateTime EndDate { get; set; }
        public int ContractNr { get; set; }
        public DateTime StartDate { get; set; }
        public ContractEnum.status Status { get; set; }
        public ContractEnum.type Type { get; set; }
        public Company Company { get; set; }
        #endregion
    }
}
