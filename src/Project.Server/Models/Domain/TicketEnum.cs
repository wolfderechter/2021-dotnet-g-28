using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class TicketEnum
    {

        public enum Status
        {
            Created = 0,
            [Display(Name = "In progress")]
            InProgress = 1,
            Closed = 2,
            Discontinued = 3,
            Cancelled = 4,
            [Display(Name = "Waiting for response customer")]
            WaitingCustomerResponse = 5,
            [Display(Name = "Received response")]
            ResponseReceived = 6,
            [Display(Name = "In development")]
            InDevelopment = 7


        }

        public enum Type
        {

            [Display(Name = "Production impacted (<2h solution)")]
            ProductionStopped = 1,
            [Display(Name = "Production will stop (<4h solution)")]
            ProductionWillStop = 2,
            [Display(Name = "No production impact (<3d solution)")]  
            NoImpact = 3
        }
    }
}