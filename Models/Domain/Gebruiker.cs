using System;
using System.Collections.Generic;
using System.Text;

namespace _2021_dotnet_g_28
{
    public class Gebruiker
    {
        public IList<IList<String>> AanmeldPogingen;
        private readonly int MaxAantalAanmeldpogingen = 5;

        public String Wachtwoord
        {
            get => default;
            set
            {
            }
        }

        public String Gebruikersnaam
        {
            get => default;
            set
            {
            }
        }

        public bool IsGeblokkeerd
        {
            get => default;
            set
            {
            }
        }

        public void MeldAan(String Wachtwoord)
        {
            throw new System.NotImplementedException();
        }

        public void RegistreerAanmeldingsPoging()
        {
            throw new System.NotImplementedException();
        }

        public int GetAantalAanmeldingsPogingen()
        {
            return AanmeldPogingen.Count;
        } 
    }
}