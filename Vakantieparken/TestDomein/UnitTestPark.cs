using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieparkenBL.Exceptions;
using VakantieparkenBL.Model;

namespace TestDomein
{
    public class UnitTestPark
    {
        [Fact]
        public void TestZetFaciliteitenList_ValidInput()
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            faciliteiten.Add(new Faciliteit(1, "Faciliteit 1"));

            Park park1 = new Park(1, "Park 1", "Locatie 1", faciliteiten);
            Assert.Equal(faciliteiten.Count, park1.Faciliteiten.Count); //Ctor

            List<Faciliteit> faciliteiten1 = new List<Faciliteit>();
            faciliteiten1.Add(new Faciliteit(1, "Faciliteit 1"));
            faciliteiten1.Add(new Faciliteit(2, "Faciliteit 2"));

            park1.ZetFaciliteitenList(faciliteiten1);
            Assert.Equal(2, park1.Faciliteiten.Count);
        }
        [Fact]
        public void TestZetFaciliteitenList_InvalidInput()
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            faciliteiten.Add(new Faciliteit(1, "Faciliteit 1"));

            Park park1 = new Park(1, "Park 1", "Locatie 1", faciliteiten);
            List<Faciliteit> ongeldigeFaciliteiten1 = null;
            Assert.Throws<DomeinException>(() => park1.ZetFaciliteitenList(ongeldigeFaciliteiten1));

            List<Faciliteit> ongeldigeFaciliteiten2 = new();
            Park park2 = new Park(2, "Park 2", "Locatie 2", faciliteiten);
            Assert.Throws<DomeinException>(() => park2.ZetFaciliteitenList(ongeldigeFaciliteiten2));
        }

        [Fact]
        public void TestZetLocatie_ValidInput()
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            faciliteiten.Add(new Faciliteit(1, "Faciliteit 1"));

            Park park = new Park(1, "Park 1", "Locatie 1", faciliteiten);
            park.ZetLocatie("Locatie 2");
            Assert.Equal("Locatie 2", park.Locatie);
        }
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("     ")]
        [InlineData(null)]
        public void TestZetLocatie_InValidInput(string ongeldigLocatie)
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            faciliteiten.Add(new Faciliteit(1, "Faciliteit 1"));

            Park park1 = new Park(1, "Park 1", "Locatie 1", faciliteiten);

            Assert.Throws<DomeinException>(() => park1.ZetLocatie(ongeldigLocatie));
        }
    }

}

