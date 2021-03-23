using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Action { get; set; }
        public  string  TicketName { get; set; }
        public bool IsRead { get; set; }
        public int ContactPersonId { get; set; }

        public Notification()
        {

        }

        public Notification(string action,string ticket,int ContactPersonId)
        {
            Action = action;
            TicketName = ticket;
            IsRead = false;
        }
    }

    
}
