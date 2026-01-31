using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieparkenBL.DTO;
using VakantieparkenBL.Model;

namespace VakantieparkenBL.Interfaces
{
    public interface IVakantieparkenRepository
    {
        void SchrijfBestanden((List<Park> parken, List<Klant> klanten) bestandenData);


        bool BestaatKlant(string klantNaam);
        List<Klant> LeesKlantenOpNaam(string klantNaam);
        List<ReservatiesOpKlantInfo> LeesReservatiesOpKlantId(int klantID);
        List<ParkInfo> LeesParks();
        List<ReservatiesOpParkEnPeriodeInfo> LeesReservatiesOpParkEnPeriode(int parkID, DateTime startDatum, DateTime eindDatum);
        List<HuisInfo> LeesActieveHuizenOpPark(int parkId);
        List<Reservatie> LeesReservatiesOpHuisID(int huisID);
        void SchrijfGeaffecteerdeReservaiesEnHuisNietActief(int huisID, List<Reservatie> reservaties);
        List<GeaffecteerdeReservatieInfo> LeesGeaffecteerdeReservaties();
        List<Faciliteit> LeesAlleFaciliteiten();
        List<int> LeesMaxCapaciteitenVanPark(int parkId); // Als nieuw park toevoegen met andere huizen capaciteiten.
        List<HuisInfo> LeesBeschikbareHuizen(int parkId, int aantalPersonen, DateTime startDatum, DateTime eindDatum);
        void SchrijfReservatie(int klantId, DateTime startDatum, DateTime eindDatum, int huisId);
        List<ParkInfo> LeesParkenOpFaciliteiten(List<Faciliteit> faciliteiten);

    }
}
