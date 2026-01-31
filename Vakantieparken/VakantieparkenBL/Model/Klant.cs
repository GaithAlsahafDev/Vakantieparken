using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VakantieparkenBL.Model
{
    public class Klant
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Adres { get; set; }
        internal Klant(int id, string naam, string adres)
        {
            Id = id;
            Naam = naam;
            Adres = adres;
        }
    }
}
