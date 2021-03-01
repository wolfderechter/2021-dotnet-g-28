using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class TicketEnum
    {
        public enum status
        {
            Created = 1,
            InProgress = 2,
            Closed = 3,
            Cancelled = 4,
            WaitingCustomerResponse = 5,
            ResponseReceived = 6,
            InDevelopment = 7

        }

        public enum type
        {
            [Display(Name = "1: production impacted, within 2h a solution")]
            ProductionStopped = 1,
            [Display(Name = "2: production will stop for a while, within 4h a solution")]
            ProductionWillStop = 2,
            [Display(Name = "3: no production impact, within 3 days a solution")]
            NoImpact = 3
        }
    }
}
