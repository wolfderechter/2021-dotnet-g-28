using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class ContractEnum
    {
        public enum status
        {
            Running = 1,
            [Display(Name = "In Progress")]
            InProgress = 2,
            Ended = 3,
            Cancelled = 4,
            [Display(Name = "Not Active")]
            NotActive =5
        }
    }
}
