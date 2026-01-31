using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VakantieparkenBL.DTO
{
    public class GeaffecteerdeReservatieInfo
    {
        public int ReservatieID { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public string KlantNaam { get; set; }
        public string KlantAdres { get; set; }
        public string ParkNaam { get; set; }
        public string ParkLocatie { get; set; }
        public string HuisAdres { get; set; }
        public string HuisNummer { get; set; }

        public GeaffecteerdeReservatieInfo(int reservatieID, DateTime startDatum, DateTime eindDatum,
                                           string klantNaam, string klantAdres,
                                           string parkNaam, string parkLocatie,
                                           string huisAdres, string huisNummer)
        {
            ReservatieID = reservatieID;
            StartDatum = startDatum;
            EindDatum = eindDatum;
            KlantNaam = klantNaam;
            KlantAdres = klantAdres;
            ParkNaam = parkNaam;
            ParkLocatie = parkLocatie;
            HuisAdres = huisAdres;
            HuisNummer = huisNummer;
        }
    }

}
