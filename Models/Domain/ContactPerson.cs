using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{

    public class ContactPerson 
    {
        #region properties
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Company Company { get; set; }
        public IdentityUser User { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }
        #endregion

        public ContactPerson()
        {
            Notifications = new List<Notification>();
        }

    }
}
