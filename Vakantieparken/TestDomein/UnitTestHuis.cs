using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieparkenBL.Exceptions;
using VakantieparkenBL.Model;

namespace TestDomein
{
    public class UnitTestHuis
    {
        [Fact]
        public void TestZetStraat_ValidInput()
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            faciliteiten.Add(new Faciliteit(1, "Faciliteit 1"));

            Park park = new Park(1, "Park 1", "Locatie 1", faciliteiten);
            Huis huis = new Huis(1, "Straat 1", "1", true, 2, park);

            huis.ZetStraat("Straat 2");

            Assert.Equal("Straat 2", huis.Straat);
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("          ")]
        public void TestZetStraat_InvalidInput(string ongeldigStraat)
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            faciliteiten.Add(new Faciliteit(1, "Faciliteit 1"));

            Park park = new Park(1, "Park 1", "Locatie 1", faciliteiten);
            Huis huis = new Huis(1, "Straat 1", "1", true, 2, park);

            Assert.Throws<DomeinException>(() => huis.ZetStraat(ongeldigStraat));
        }
        [Fact]
        public void TestZetHuisNummer_ValidInput()
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            faciliteiten.Add(new Faciliteit(1, "Faciliteit 1"));

            Park park = new Park(1, "Park 1", "Locatie 1", faciliteiten);
            Huis huis = new Huis(1, "Straat 1", "1", true, 2, park);

            huis.ZetHuisNummer("2");

            Assert.Equal("2", huis.HuisNummer);
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("         ")]
        public void TestZetHuisNummer_InvalidInput(string ongeldigHuisNummer)
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            faciliteiten.Add(new Faciliteit(1, "Faciliteit 1"));

            Park park = new Park(1, "Park 1", "Locatie 1", faciliteiten);
            Huis huis = new Huis(1, "Straat 1", "1", true, 2, park);

            Assert.Throws<DomeinException>(() => huis.ZetHuisNummer(ongeldigHuisNummer));
        }
        [Fact]
        public void TestZetCapaciteit_ValidInput()
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            faciliteiten.Add(new Faciliteit(1, "Faciliteit 1"));

            Park park = new Park(1, "Park 1", "Locatie 1", faciliteiten);
            Huis huis = new Huis(1, "Straat 1", "1", true, 2, park);

            huis.ZetCapaciteit(3);

            Assert.Equal(3, huis.Capaciteit);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TestZetCapaciteit_InvalidInput(int ongeldigCapaciteit)
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            faciliteiten.Add(new Faciliteit(1, "Faciliteit 1"));

            Park park = new Park(1, "Park 1", "Locatie 1", faciliteiten);
            Huis huis = new Huis(1, "Straat 1", "1", true, 2, park);

            Assert.Throws<DomeinException>(() => huis.ZetCapaciteit(ongeldigCapaciteit));
        }

        [Fact]
        public void TestZetPark_ValidInput()
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            faciliteiten.Add(new Faciliteit(1, "Faciliteit 1"));

            Park park1 = new Park(1, "Park 1", "Locatie 1", faciliteiten);
            Huis huis = new Huis(1, "Straat 1", "1", true, 2, park1);

            Park park2 = new Park(2, "Park 2", "Locatie 2", faciliteiten);

            huis.ZetPark(park2);

            Assert.Equal(2, huis.Park.Id);
            Assert.Equal("Park 2", huis.Park.Naam);
            Assert.Equal("Locatie 2", huis.Park.Locatie);
        }

        [Fact]
        public void TestZetPark_InvalidInput()
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            faciliteiten.Add(new Faciliteit(1, "Faciliteit 1"));

            Park park = new Park(1, "Park 1", "Locatie 1", faciliteiten);
            Huis huis = new Huis(1, "Straat 1", "1", true, 2, park);

            Assert.Throws<DomeinException>(() => huis.ZetPark(null));
        }

    }
}
