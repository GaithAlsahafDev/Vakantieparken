using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieparkenBL.Model;

namespace VakantieparkenBL.DTO
{
    public class ParkInfo
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Locatie { get; set; }

        public ParkInfo(int id, string naam, string locatie)
        {
            Id = id;
            Naam = naam;
            Locatie = locatie;
        }
        public override string ToString() //NieuweReservatieWindow_Faciliteiten + ReservatiesZoekenWindow + HuisInOnderhoudWindow
        {
            return $"{Naam} - {Locatie}";
        }
    }
}
