using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VakantieparkenBL.DTO;
using VakantieparkenBL.Exceptions;
using VakantieparkenBL.Interfaces;
using VakantieparkenBL.Model;

namespace VakantieparkenBL.Managers
{
    public class VakantieparkenManager
    {
        private IVakantieparkenRepository _vakantieparkenRepository;
        public VakantieparkenManager(IVakantieparkenRepository vakantieparkenRepository)
        {
            _vakantieparkenRepository = vakantieparkenRepository;
        }
        public bool BestaatKlant(string klantNaam)
        {
            try
            {
                return _vakantieparkenRepository.BestaatKlant(klantNaam);
            }
            catch(Exception ex)
            {
                throw new ManagerException(nameof(BestaatKlant), ex);
            }
        }
        public List<Klant> GeefKlantenOpNaam(string klantNaam)
        {
            try
            {
                return _vakantieparkenRepository.LeesKlantenOpNaam(klantNaam);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefKlantenOpNaam), ex);
            }
        }
        public List<ReservatiesOpKlantInfo> ZoekReservatiesOpKlantId(int klantID)
        {
            try
            {
                return _vakantieparkenRepository.LeesReservatiesOpKlantId(klantID);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(ZoekReservatiesOpKlantId), ex);
            }
        }
        public List<ParkInfo> GeefParks()
        {
            try
            {
                return _vakantieparkenRepository.LeesParks();
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefParks), ex);
            }
        }
        public List<ReservatiesOpParkEnPeriodeInfo> ZoekReservatiesOpParkEnPeriode(int parkID, DateTime startDatum, DateTime eindDatum)
        {
            try
            {
                return _vakantieparkenRepository.LeesReservatiesOpParkEnPeriode(parkID, startDatum, eindDatum);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(ZoekReservatiesOpParkEnPeriode), ex);
            }
        }
        public List<HuisInfo> GeefActieveHuizenOpPark(int parkId)
        {
            try
            {
                return _vakantieparkenRepository.LeesActieveHuizenOpPark(parkId);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefActieveHuizenOpPark), ex);
            }
        }
        public List<Reservatie> GeefReservatiesOpHuisID(int huisID)
        {
            try
            {
                return _vakantieparkenRepository.LeesReservatiesOpHuisID(huisID);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefReservatiesOpHuisID), ex);
            }
        }
        public void MaakReservatiesGeaffecteerdEnHuisNietActiefMaken(int huisID, List<Reservatie> reservaties)
        {
            try
            {
                _vakantieparkenRepository.SchrijfGeaffecteerdeReservaiesEnHuisNietActief(huisID, reservaties);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(MaakReservatiesGeaffecteerdEnHuisNietActiefMaken), ex);
            }
        }
        public List<GeaffecteerdeReservatieInfo> GeefGeaffecteerdeReservaties()
        {
            try
            {
                return _vakantieparkenRepository.LeesGeaffecteerdeReservaties();
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefGeaffecteerdeReservaties), ex);
            }
        }
        public List<Faciliteit> GeefAlleFaciliteiten()
        {
            try
            {
                return _vakantieparkenRepository.LeesAlleFaciliteiten();
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefAlleFaciliteiten), ex);
            }
        }

        public List<int> GeefMaxCapaciteitenVanPark(int parkId) // Als nieuw park toevoegen met andere huizen capaciteiten.
        {
            try
            {
                return _vakantieparkenRepository.LeesMaxCapaciteitenVanPark(parkId);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefMaxCapaciteitenVanPark), ex);
            }
        }
        public List<HuisInfo> GeefBeschikbareHuizen(int parkId, int aantalPersonen, DateTime startDatum, DateTime eindDatum)
        {
            try
            {

                return _vakantieparkenRepository.LeesBeschikbareHuizen(parkId, aantalPersonen, startDatum, eindDatum);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefBeschikbareHuizen), ex);
            }
        }
        public void VoegReservatieToe(int klantId, DateTime startDatum, DateTime eindDatum, int huisId)
        {
            try
            {
                _vakantieparkenRepository.SchrijfReservatie(klantId, startDatum, eindDatum, huisId);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(VoegReservatieToe), ex);
            }
        }
        public List<ParkInfo> GeefParkenOpFaciliteiten(List<Faciliteit> faciliteiten)
        {
            try
            {
                return _vakantieparkenRepository.LeesParkenOpFaciliteiten(faciliteiten);
            }
            catch (Exception ex)
            {
                throw new ManagerException(nameof(GeefParkenOpFaciliteiten), ex);
            }
        }
    }
}
