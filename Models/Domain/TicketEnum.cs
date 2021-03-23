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
            Created = 1,
            [Display(Name = "In progress")]
            InProgress = 2,
            Closed = 3,
            Discontinued = 4,
            Cancelled = 5,
            [Display(Name = "Waiting for response customer")]
            WaitingCustomerResponse = 6,
            [Display(Name = "Received response")]
            ResponseReceived = 7,
            [Display(Name = "In development")]
            InDevelopment = 8


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