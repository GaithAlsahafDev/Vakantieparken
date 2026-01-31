using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VakantieparkenBL.Model
{
    public class Reservatie
    {
        public int Id { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public Klant Klant { get; set; }
        public bool IsGeaffecteerd { get; set; }
        internal Reservatie(int id, DateTime startDatum, DateTime eindDatum, Klant klant)
        {
            Id = id;
            StartDatum = startDatum;
            EindDatum = eindDatum;
            Klant = klant;
            IsGeaffecteerd = false;
        }
    }
}
