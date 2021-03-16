using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021_dotnet_g_28.Models.Domain
{
    public class Faq
    {
        public int Id { get; set; }
        public string Problem { get; set; }

        public List<string> Solution { get; set; }

        public Faq()
        {

        }

        public Faq(string probleem, List<string> oplossing)
        {
            Problem = probleem;
            Solution = oplossing;
        }
    }
}
