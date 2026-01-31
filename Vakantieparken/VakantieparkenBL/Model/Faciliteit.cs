using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VakantieparkenBL.Model
{
    public class Faciliteit
    {
        public int Id { get; set; }
        public string Beschrijving { get; set; }
        internal Faciliteit(int id, string beschrijving)
        {
            Id = id;
            Beschrijving = beschrijving;
        }
        public override string ToString() //NieuweReservatieWindow TabItem 2
        {
            return Beschrijving;
        }
    }
}
