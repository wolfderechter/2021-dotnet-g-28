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
            Running = 0,
            [Display(Name = "In Progress")]
            InProgress = 1,
            Ended = 2,
            Cancelled = 3,
        }
    }
}
