using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class Reaction
    {
        public int ReactionId { get; set; }
        public string Text { get; set; }
        public bool IsSolution { get; set; }
        public int TicketId { get; set; }
        public string NameUserReaction { get; set; }
        public bool ReactionSup { get; set; }
        public Reaction()
        {
            
        }

        public Reaction(string text,string name,bool isSup,int ticketId)
        {
            Text = text;
            NameUserReaction = name;
            ReactionSup = isSup;
            IsSolution = false;
            TicketId = ticketId;
        }
    }
}
