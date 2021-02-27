using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public enum TypeTicket
    {
        [Display(Name  = "1: productie geïmpacteerd, binnen 2u een oplossing")]
         type1
        //[Description("2: productie zal binnen een tijd stil vallen , binnen 4u oplossing")]
        //type2,
        //[Description("3: geen productie impact, binnen 3d een antwoord")]
        //type3
    }
}
