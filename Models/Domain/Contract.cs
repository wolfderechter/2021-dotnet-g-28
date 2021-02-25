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
        public int Number { get; set; }
        public DateTime StartDate { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public Company company { get; set; }
        #endregion
    }
}
