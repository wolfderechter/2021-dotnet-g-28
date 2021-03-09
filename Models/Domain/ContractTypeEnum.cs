using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class ContractTypeEnum
    {
        public enum CreationMethod
        {
            [Display(Name = "E-mail")]
            Email,
            [Display(Name = "Telephone")]
            phone,
            [Display(Name = "Application")]
            app
        }
    }
}
