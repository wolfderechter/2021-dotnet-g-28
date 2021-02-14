using _2021_dotnet_g_28.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2021_dotnet_g_28
{
    public class Klant : Gebruiker
    {

        public int Klantnummer
        {
            get => default;
            set
            {
            }
        }

        public String Naam
        {
            get => default;
            set
            {
            }
        }

        public DateTime KlantSinds
        {
            get => default;
            set
            {
            }
        }

        public String Adres
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

        public String Status
        {
            get => default;
            set
            {
            }
        }

        public IList<Contract> Contracten
        {
            get => default;
            set
            {
            }
        }

        public Bedrijf Bedrijf
        {
            get => default;
            set
            {
            }
        }

        public void ticketAanmaken()
        {
            throw new System.NotImplementedException();
        }

        public void ticketWijzigen()
        {
            throw new System.NotImplementedException();
        }

        public void ticketStopzetten()
        {
            throw new System.NotImplementedException();
        }

        public void contractAanmaken()
        {
            throw new System.NotImplementedException();
        }

        public void contractStopzetten()
        {
            throw new System.NotImplementedException();
        }

        public void contractRaadplegen()
        {
            throw new System.NotImplementedException();
        }
    }
}