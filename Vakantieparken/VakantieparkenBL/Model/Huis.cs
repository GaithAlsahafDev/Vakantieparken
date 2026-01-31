using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieparkenBL.Exceptions;

namespace VakantieparkenBL.Model
{
    public class Huis
    {
        public int Id { get; set; }
        public string Straat { get; private set; }
        public string HuisNummer { get; private set; }
        public bool IsActief { get; set; }
        public int Capaciteit { get; private set; }
        public Park Park { get; private set; }
        public List<Reservatie> Reservaties { get; set; }

        internal Huis (int id, string straat, string huisNummer, bool isActief, int capaciteit ,Park park)
        { 
            Id = id;
            ZetStraat(straat);
            ZetHuisNummer(huisNummer);
            IsActief = isActief;
            ZetCapaciteit(capaciteit);
            ZetPark(park);
            Reservaties = new List<Reservatie>();
        }
        public void ZetStraat(string straat)
        {
            if (string.IsNullOrWhiteSpace(straat))
            {
                throw new DomeinException(nameof(ZetStraat));
            }
            Straat = straat.Trim();
        }
        public void ZetHuisNummer(string huisNummer)
        {
            if (string.IsNullOrWhiteSpace(huisNummer))
            {
                throw new DomeinException(nameof(ZetHuisNummer));
            }
            HuisNummer = huisNummer.Trim();
        }
        public void ZetCapaciteit(int capaciteit)
        {
            if (capaciteit < 1)
            {
                throw new DomeinException(nameof(ZetCapaciteit));
            }
            Capaciteit = capaciteit;
        }
        public void ZetPark(Park park)
        {
            if (park == null)
            {
                throw new DomeinException(nameof(ZetPark));
            }
            Park = park;
        }
    }
}
