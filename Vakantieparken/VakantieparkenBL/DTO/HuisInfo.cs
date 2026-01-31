using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieparkenBL.Model;

namespace VakantieparkenBL.DTO
{
    public class HuisInfo
    {
        public int Id { get; set; }
        public string Straat { get; set; }
        public string HuisNummer { get; set; }
        public bool IsActief { get; set; }
        public int Capaciteit { get; set; }

        public HuisInfo(int id, string straat, string huisNummer, bool isActief, int capaciteit)
        {
            Id = id;
            Straat = straat;
            HuisNummer = huisNummer;
            IsActief = isActief;
            Capaciteit = capaciteit;
        }
        public override string ToString()
        {
            return $"{Straat} {HuisNummer}";
        }
    }
}
