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
            [Display(Name = "1: productie geïmpacteerd, binnen 2u een oplossing")]
            ProductionStopped = 1,
            [Display(Name = "2: productie zal binnen een tijd stil vallen , binnen 4u oplossing")]
            ProductionWillStop = 2,
            [Display(Name = "3: geen productie impact, binnen 3d een antwoord")]
            NoImpact = 3
        }
    }
}
