using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class ContactPerson : IdentityUser
    {
        #region properties
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Company Company { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        #endregion
    }
}
