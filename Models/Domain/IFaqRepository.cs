using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
   public interface IFaqRepository
    {
        IEnumerable<Faq> GetAll();
        IEnumerable<Faq> GetBySearchstring(string searchstring);
    }

   
}
