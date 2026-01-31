using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VakantieparkenBL.DTO
{
    public class ReservatiesOpParkEnPeriodeInfo
    {
        public ReservatiesOpParkEnPeriodeInfo(int reservatieID, string startDatum, string eindDatum, int klantID, string klantNaam, int huisID, string huisAdres, int huisCapaciteit)
        {
            ReservatieID = reservatieID;
            StartDatum = startDatum;
            EindDatum = eindDatum;
            KlantID = klantID;
            KlantNaam = klantNaam;
            HuisID = huisID;
            HuisAdres = huisAdres;
            HuisCapaciteit = huisCapaciteit;
        }
        public int ReservatieID { get; set; }
        public string StartDatum { get; set; }
        public string EindDatum { get; set; }
        public int KlantID { get; set; }
        public string KlantNaam { get; set; }
        public int HuisID { get; set; }
        public string HuisAdres { get; set; }
        public int HuisCapaciteit { get; set; }
    }
}
