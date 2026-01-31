using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieparkenBL.Model;

namespace VakantieparkenBL.DTO
{
    public class ReservatiesOpKlantInfo
    {
        public ReservatiesOpKlantInfo(int reservatieID, string startDatum, string eindDatum,
                          string parkNaam, string parkLocatie, int huisID, string adres, int huisCapaciteit)
        {
            ReservatieID = reservatieID;
            StartDatum = startDatum;
            EindDatum = eindDatum;
            ParkNaam = parkNaam;
            ParkLocatie = parkLocatie;
            HuisID = huisID;
            HuisAdres = adres;
            HuisCapaciteit = huisCapaciteit;
        }
        public int ReservatieID { get; set; }
        public string StartDatum { get; set; }
        public string EindDatum { get; set; }
        public string ParkNaam { get; set; }
        public string ParkLocatie { get; set; }
        public int HuisID { get; set; }
        public string HuisAdres { get; set; }
        public int HuisCapaciteit { get; set; }
    }
}
