using VakantieparkenBL.Exceptions;
using VakantieparkenBL.Interfaces;
using VakantieparkenBL.Model;

namespace VakantieparkenDL_File
{
    public class FileProcessor : IFileProcessor
    {
        public (List<Park>, List<Klant>) LeesBestanden(string faciliteiten_File, string huis_reservaties_File, string huizen_File, string klanten_park_File, string park_huizen_File, string parken_File, string parken_faciliteiten_File, string reservaties_File)
        {
            //------------------------------faciliteiten--------------------------------
            //(id en beschrijving).
            Dictionary<int,Faciliteit> faciliteiten_Dictionary = new();
            try
            {
                using (StreamReader sr = new StreamReader(faciliteiten_File))
                {
                    string lijn;
                    while ((lijn = sr.ReadLine()) != null)
                    {
                        string[] lijnen = lijn.Split(',');

                        if (!faciliteiten_Dictionary.ContainsKey(int.Parse(lijnen[0])))
                        {
                            faciliteiten_Dictionary.Add(int.Parse(lijnen[0]), new Faciliteit(int.Parse(lijnen[0]), lijnen[1]));
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
                throw new DomeinException($"{nameof(LeesBestanden)} - faciliteitenFilePath", ex);
            }
            //------------------------------parken_faciliteiten--------------------------------
            //(park_id en faciliteit_id).
            Dictionary<int, List<Faciliteit>> parkFaciliteit_Dictionary = new();
            try
            {
                using (StreamReader sr = new StreamReader(parken_faciliteiten_File))
                {
                    string lijn;
                    while ((lijn = sr.ReadLine()) != null)
                    {
                        string[] lijnen = lijn.Split(',');

                        if(!parkFaciliteit_Dictionary.ContainsKey(int.Parse(lijnen[0])) && faciliteiten_Dictionary.ContainsKey(int.Parse(lijnen[1])))
                        {
                            parkFaciliteit_Dictionary.Add(int.Parse(lijnen[0]), new List<Faciliteit>());
                        }
                        if (parkFaciliteit_Dictionary.ContainsKey(int.Parse(lijnen[0])) && faciliteiten_Dictionary.ContainsKey(int.Parse(lijnen[1])))
                        {
                            parkFaciliteit_Dictionary[int.Parse(lijnen[0])].Add(faciliteiten_Dictionary[int.Parse(lijnen[1])]);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            { 
                throw new DomeinException($"{nameof(LeesBestanden)} - parken_faciliteitenFilePath", ex);
            }
            //------------------------------Parken--------------------------------
            //(id, naam en locatie).
            Dictionary<int, Park> parken_Dictionary = new();
            try
            {
                using (StreamReader sr = new StreamReader(parken_File))
                {
                    string lijn;
                    while ((lijn = sr.ReadLine()) != null)
                    {
                        string[] lijnen = lijn.Split(',');

                        if (!parken_Dictionary.ContainsKey(int.Parse(lijnen[0])))
                        {
                            parken_Dictionary.Add(int.Parse(lijnen[0]), new Park(int.Parse(lijnen[0]), lijnen[1], lijnen[2], parkFaciliteit_Dictionary[int.Parse(lijnen[0])]));
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
                throw new DomeinException($"{nameof(LeesBestanden)} - parkenFilePath", ex); 
            }

            //------------------------------park_huizen--------------------------------
            //(park_id en huis_id).
            Dictionary<int, Park> Huis_Park_Dictionary = new(); // Park object in Huis
            Dictionary<int, List<int>> park_Huizen_Dictionary = new(); // Huizen list in Park

            try
            {
                using (StreamReader sr = new StreamReader(park_huizen_File))
                {
                    string lijn;
                    while ((lijn = sr.ReadLine()) != null)
                    {
                        string[] lijnen = lijn.Split(',');

                        Huis_Park_Dictionary.Add(int.Parse(lijnen[1]), parken_Dictionary[int.Parse(lijnen[0])]); // الهدف منه هو عندما ننشء منزل لازم يحتوي على كائن بارك

                        if (!park_Huizen_Dictionary.ContainsKey(int.Parse(lijnen[0])))
                        {
                            park_Huizen_Dictionary.Add(int.Parse(lijnen[0]), new List<int>());
                        }
                        park_Huizen_Dictionary[int.Parse(lijnen[0])].Add(int.Parse(lijnen[1]));
                    }
                }
            }
            catch (Exception ex)
            { 
                throw new DomeinException($"{nameof(LeesBestanden)} - park_huizenFilePath", ex); 
            }

            //------------------------------Huis--------------------------------
            //(id, straat, nummer, isActief en het aantal personen).

            Dictionary<int, Huis> huizen_Dictionary = new();
            try
            {
                using (StreamReader sr = new StreamReader(huizen_File))
                {
                    string lijn;
                    while ((lijn = sr.ReadLine()) != null)
                    {
                        string[] lijnen = lijn.Split(',');

                        if (!huizen_Dictionary.ContainsKey(int.Parse(lijnen[0])))
                        {
                            huizen_Dictionary.Add(int.Parse(lijnen[0]), new Huis(int.Parse(lijnen[0]), lijnen[1], lijnen[2], bool.Parse(lijnen[3]), int.Parse(lijnen[4]), Huis_Park_Dictionary[int.Parse(lijnen[0])]));
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                throw new DomeinException($"{nameof(LeesBestanden)} - huizenFilePath", ex); 
            }

            //------------------------------List van huizen in park--------------------------------
            foreach(var parkIDENListHuizen in park_Huizen_Dictionary)
            {
                foreach(var huizenList in parkIDENListHuizen.Value)
                {
                    parken_Dictionary[parkIDENListHuizen.Key].Huizen.Add(huizen_Dictionary[huizenList]);
                }
            }
            //------------------------------Klanten--------------------------------
            //(klant_id, naam en adres).
            Dictionary<int, Klant> klanten_Dictionary = new Dictionary<int, Klant>();
            try
            {
                using (StreamReader sr = new StreamReader(klanten_park_File))
                {
                    string lijn;
                    while ((lijn = sr.ReadLine()) != null)
                    {
                        string[] lijnen = lijn.Split('|');

                        if (!klanten_Dictionary.ContainsKey(int.Parse(lijnen[0])))
                        {
                            klanten_Dictionary.Add(int.Parse(lijnen[0]), new Klant(int.Parse(lijnen[0]), lijnen[1], lijnen[2]));
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
                throw new DomeinException($"{nameof(LeesBestanden)} - klanten_parkFile", ex);
            }

            //------------------------------Reservaties--------------------------------
            //(reservatie_id, startdatum, einddatum en klant_nummer).
            Dictionary<int,Reservatie> reservaties_Dictionary = new();
            try
            {
                using (StreamReader sr = new StreamReader(reservaties_File))
                {
                    string lijn;
                    while ((lijn = sr.ReadLine()) != null)
                    {
                        string[] lijnen = lijn.Split(',');
                        if (!reservaties_Dictionary.ContainsKey(int.Parse(lijnen[0])))
                        {
                            reservaties_Dictionary.Add(int.Parse(lijnen[0]), new Reservatie(int.Parse(lijnen[0]), DateTime.Parse(lijnen[1]), DateTime.Parse(lijnen[2]), klanten_Dictionary[int.Parse(lijnen[3])]));
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                throw new DomeinException($"{nameof(LeesBestanden)} - reservatiesFilePath", ex);
            }

            //------------------------------huis_reservaties--------------------------------
            //(huis_id en reservatie_id)
            try
            {
                using (StreamReader sr = new StreamReader(huis_reservaties_File))
                {
                    string lijn;
                    while ((lijn = sr.ReadLine()) != null)
                    {
                        string[] lijnen = lijn.Split(',');
                        if (huizen_Dictionary.ContainsKey(int.Parse(lijnen[0])) && reservaties_Dictionary.ContainsKey(int.Parse(lijnen[1])))
                        {
                            huizen_Dictionary[int.Parse(lijnen[0])].Reservaties.Add(reservaties_Dictionary[int.Parse(lijnen[1])]);
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
                throw new DomeinException($"{nameof(LeesBestanden)} - huis_reservatiesFilePath", ex);
            }

            //------------------------------GeaffecteerdeReservaties--------------------------------
            List<Park> parken = parken_Dictionary.Values.ToList();
            foreach (Park park in parken)
            {
                foreach (Huis huis in park.Huizen)
                {
                    if (!huis.IsActief && huis.Reservaties != null && huis.Reservaties.Count > 0)
                    {
                        foreach (Reservatie reservatie in huis.Reservaties)
                        {
                            if (reservatie.EindDatum.Date >= DateTime.Today)
                            {
                                reservatie.IsGeaffecteerd = true;
                            }
                        }
                    }
                }
            }

            return (parken, klanten_Dictionary.Values.ToList());
        }
    }
}