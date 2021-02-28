using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
        public interface IContactPersonRepository
        {
            IEnumerable<ContactPerson> getAll();

            ContactPerson getById(String userId);

        }
}
