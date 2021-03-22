using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Viewmodels
{
    public class TypeModelTicket
    {
        public Domain.TicketEnum.Type Type { get; set; }
        public bool IsSelected { get; set; }
    }
}
