using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieparkenBL.Exceptions;

namespace VakantieparkenBL.Model
{
    public class Park
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Locatie { get; private set; }
        public List<Huis> Huizen { get; set; } 
        public List<Faciliteit> Faciliteiten { get; private set; }

        internal Park(int id, string naam, string locatie, List<Faciliteit> faciliteiten)
        {
            Id = id;
            Naam = naam;
            ZetLocatie(locatie);
            ZetFaciliteitenList(faciliteiten);
            Huizen = new List<Huis>(); 
        }
        public void ZetFaciliteitenList(List<Faciliteit> faciliteiten)
        {
            if (faciliteiten == null || faciliteiten.Count == 0)
            {
                throw new DomeinException(nameof(ZetFaciliteitenList));
            }
            Faciliteiten = faciliteiten;
        }
        public void ZetLocatie(string locatie)
        {
            if (string.IsNullOrWhiteSpace(locatie))
            {
                throw new DomeinException(nameof(ZetLocatie));
            }
            Locatie = locatie.Trim();
        }
    }
}
