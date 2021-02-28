using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class TicketEnum
    {
        public enum status { 
            Created=1,
            InProgress=2,
            Closed=3,
            Cancelled=4,
            WaitingCustomerResponse=5,
            ResponseReceived=6,
            InDevelopment=7
            
        }

        public enum type
        {
            ProductionStopped =1,
            ProductionWillStop =2,
            NoImpact =3
        }
    }
}
