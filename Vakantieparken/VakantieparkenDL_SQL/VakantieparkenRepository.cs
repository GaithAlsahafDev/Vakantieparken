using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using VakantieparkenBL.DTO;
using VakantieparkenBL.Exceptions;
using VakantieparkenBL.Interfaces;
using VakantieparkenBL.Model;

namespace VakantieparkenDL_SQL
{
    public class VakantieparkenRepository : IVakantieparkenRepository
    {
        private string _connectionString;
        public VakantieparkenRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Bestanden
        public void SchrijfBestanden((List<Park> parken, List<Klant> klanten) bestandenData)
        {
            //Bestaat?
            string sqlLeesParkIDs = "SELECT id FROM Park";
            string sqlLeesKlantIDs = "SELECT id FROM Klant";
            string sqlLeesHuisIDs = "SELECT id FROM Huis";
            string sqlLeesReservatieIDs = "SELECT id FROM Reservatie";
            string sqlLeesFaciliteitIDs = "SELECT id FROM Faciliteit";
            string sqlLeesParkFaciliteitIDs = "SELECT park_id, faciliteit_id FROM Park_Faciliteit";

            //Schrijf
            string sqlSchrijfPark = "INSERT INTO Park(id, naam, locatie) VALUES (@id, @naam, @locatie);";
            string sqlSchrijfKlant = "INSERT INTO Klant(id, naam, adres) VALUES (@id, @naam, @adres);";
            string sqlSchrijfHuis = "INSERT INTO Huis(id, straat, nummer, isActief, capaciteit, park_id) VALUES (@id, @straat, @nummer, @isActief, @capaciteit, @park_id);";
            string enableIdentityInsertSql = "SET IDENTITY_INSERT Reservatie ON;";
            string sqlSchrijfReservatie = "INSERT INTO Reservatie(id, klant_id, startDatum, eindDatum, isGeaffecteerd, huis_id) VALUES (@id, @klant_id, @startDatum, @eindDatum, @isGeaffecteerd, @huis_id);";
            string disableIdentityInsertSql = "SET IDENTITY_INSERT Reservatie OFF;";
            string sqlSchrijfFaciliteit = "INSERT INTO Faciliteit(id, beschrijving) VALUES (@id, @beschrijving);";
            string sqlSchrijfParkFaciliteit = "INSERT INTO Park_Faciliteit(park_id, faciliteit_id) VALUES (@park_id, @faciliteit_id);";

            List<int> bestaandeParken = new();
            List<int> bestaandeKlanten = new();
            List<int> bestaandeHuizen = new();
            List<int> bestaandeFaciliteiten = new();
            List<int> bestaandeReservaties = new();
            HashSet<(int, int)> bestaandeParkFaciliteiten = new ();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.Transaction = conn.BeginTransaction();


                    cmd.CommandText = sqlLeesParkIDs;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bestaandeParken.Add((int)reader["id"]);
                        }
                    }

                    cmd.CommandText = sqlLeesKlantIDs;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bestaandeKlanten.Add((int)reader["id"]);
                        }
                    }

                    cmd.CommandText = sqlLeesHuisIDs;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bestaandeHuizen.Add((int)reader["id"]);
                        }
                    }

                    cmd.CommandText = sqlLeesFaciliteitIDs;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bestaandeFaciliteiten.Add((int)reader["id"]);
                        }
                    }

                    cmd.CommandText = sqlLeesParkFaciliteitIDs;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bestaandeParkFaciliteiten.Add(((int)reader["park_id"], (int)reader["faciliteit_id"]));
                        }
                    }
                    cmd.CommandText = sqlLeesReservatieIDs;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bestaandeReservaties.Add((int)reader["id"]);
                        }
                    }

                    // schrijf Faciliteiten
                    cmd.CommandText = sqlSchrijfFaciliteit;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@beschrijving", SqlDbType.NVarChar));
                    foreach (Park park in bestandenData.parken)
                    {
                        foreach (Faciliteit faciliteit in park.Faciliteiten)
                        {
                            if (!bestaandeFaciliteiten.Contains(faciliteit.Id))
                            {
                                cmd.Parameters["@id"].Value = faciliteit.Id;
                                cmd.Parameters["@beschrijving"].Value = faciliteit.Beschrijving;
                                cmd.ExecuteNonQuery();
                                bestaandeFaciliteiten.Add(faciliteit.Id);  
                            }
                        }
                    }
                    // schrijf Parken
                    cmd.CommandText = sqlSchrijfPark;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@locatie", SqlDbType.NVarChar));
                    foreach (Park park in bestandenData.parken)
                    {
                        if (!bestaandeParken.Contains(park.Id))
                        {
                            cmd.Parameters["@id"].Value = park.Id;
                            cmd.Parameters["@naam"].Value = park.Naam;
                            cmd.Parameters["@locatie"].Value = park.Locatie;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    // schrijf Park_Faciliteit
                    cmd.CommandText = sqlSchrijfParkFaciliteit;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@park_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@faciliteit_id", SqlDbType.Int));
                    foreach (Park park in bestandenData.parken)
                    {
                        foreach (Faciliteit faciliteit in park.Faciliteiten)
                        {
                            if (!bestaandeParkFaciliteiten.Contains((park.Id, faciliteit.Id)))
                            {
                                cmd.Parameters["@park_id"].Value = park.Id;
                                cmd.Parameters["@faciliteit_id"].Value = faciliteit.Id;
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // schrijf Klanten
                    cmd.CommandText = sqlSchrijfKlant;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@adres", SqlDbType.NVarChar));
                    foreach (Klant klant in bestandenData.klanten)
                    {
                        if (!bestaandeKlanten.Contains(klant.Id))
                        {
                            cmd.Parameters["@id"].Value = klant.Id;
                            cmd.Parameters["@naam"].Value = klant.Naam;
                            cmd.Parameters["@adres"].Value = klant.Adres;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // schrijf Huizen
                    cmd.CommandText = sqlSchrijfHuis;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@straat", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@nummer", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@isActief", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@capaciteit", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@park_id", SqlDbType.Int));
                    foreach (Park park in bestandenData.parken)
                    {
                        foreach (Huis huis in park.Huizen)
                        {
                            if (!bestaandeHuizen.Contains(huis.Id))
                            {
                                cmd.Parameters["@id"].Value = huis.Id;
                                cmd.Parameters["@straat"].Value = huis.Straat;
                                cmd.Parameters["@nummer"].Value = huis.HuisNummer;
                                cmd.Parameters["@isActief"].Value = huis.IsActief;
                                cmd.Parameters["@capaciteit"].Value = huis.Capaciteit;
                                cmd.Parameters["@park_id"].Value = park.Id;
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // enable Identity Insert
                    cmd.CommandText = enableIdentityInsertSql;
                    cmd.Parameters.Clear();
                    cmd.ExecuteNonQuery();

                    // schrijf Reservaties
                    cmd.CommandText = sqlSchrijfReservatie;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@klant_id", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@startDatum", SqlDbType.Date));
                    cmd.Parameters.Add(new SqlParameter("@eindDatum", SqlDbType.Date));
                    cmd.Parameters.Add(new SqlParameter("@isGeaffecteerd", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@huis_id", SqlDbType.Int));
                    foreach (Park park in bestandenData.parken)
                    {
                        foreach (Huis huis in park.Huizen)
                        {
                            if (huis.Reservaties != null && huis.Reservaties.Count > 0)
                            {
                                foreach (Reservatie reservatie in huis.Reservaties)
                                {
                                    if (!bestaandeReservaties.Contains(reservatie.Id))
                                    {
                                        cmd.Parameters["@id"].Value = reservatie.Id;
                                        cmd.Parameters["@klant_id"].Value = reservatie.Klant.Id;
                                        cmd.Parameters["@startDatum"].Value = reservatie.StartDatum;
                                        cmd.Parameters["@eindDatum"].Value = reservatie.EindDatum;
                                        cmd.Parameters["@isGeaffecteerd"].Value = reservatie.IsGeaffecteerd;
                                        cmd.Parameters["@huis_id"].Value = huis.Id;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                    // disable Identity Insert
                    cmd.CommandText = disableIdentityInsertSql;
                    cmd.Parameters.Clear();
                    cmd.ExecuteNonQuery();

                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new DomeinException(nameof(SchrijfBestanden), ex);
                }
            }
        }
        #endregion

        #region Gemeenschapelijk Windows
        // NieuweReservatieWindow + ReservatiesZoekenWindowTabItem1
        public bool BestaatKlant(string klantNaam)
        {
            string sql = "SELECT COUNT(*) FROM Klant WHERE LOWER(naam) = LOWER(@naam)";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters["@naam"].Value = klantNaam;
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(BestaatKlant), ex);
                }
            }
        }
        public List<Klant> LeesKlantenOpNaam(string klantNaam)
        {
            string sql = "SELECT * FROM Klant WHERE LOWER(naam) = LOWER(@naam)";

            List<Klant> klanten = new List<Klant>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.NVarChar));
                    cmd.Parameters["@naam"].Value = klantNaam;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            klanten.Add(new Klant((int)reader["id"], (string)reader["naam"], (string)reader["adres"]));
                        }
                    }
                    return klanten;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesKlantenOpNaam), ex);
                }
            }
        }

        // NieuweReservatieWindow + ReservatiesZoekenWindowTabItem1 + HuisInOnderhoudWindow
        public List<ParkInfo> LeesParks()
        {
            var parken = new List<ParkInfo>();
            string sql = "SELECT * FROM Park";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    IDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        parken.Add(new ParkInfo((int)reader["id"], (string)reader["naam"], (string)reader["locatie"]));
                    }
                    return parken;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesParks), ex);
                }
            }
        }


        #endregion

        #region GeaffecteerdeReservatiesWindow
        public List<GeaffecteerdeReservatieInfo> LeesGeaffecteerdeReservaties()
        {
            string sql = @"
                            SELECT 
                                r.id AS ReservatieID, 
                                r.StartDatum, 
                                r.EindDatum, 
                                k.Naam AS KlantNaam, 
                                k.Adres AS KlantAdres, 
                                p.Naam AS ParkNaam, 
                                p.Locatie AS ParkLocatie, 
                                h.straat AS HuisAdres, 
                                h.nummer AS HuisNummer
                            FROM 
                                Reservatie r
                            JOIN 
                                Klant k ON r.klant_id = k.id
                            JOIN 
                                Huis h ON r.huis_id = h.id
                            JOIN 
                                Park p ON h.park_id = p.id
                            WHERE 
                                r.isGeaffecteerd = 1
                            ORDER BY
                                eindDatum";

            List<GeaffecteerdeReservatieInfo> reservaties = new List<GeaffecteerdeReservatieInfo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservaties.Add(new GeaffecteerdeReservatieInfo(
                                (int)reader["ReservatieID"],
                                (DateTime)reader["StartDatum"],
                                (DateTime)reader["EindDatum"],
                                (string)reader["KlantNaam"],
                                (string)reader["KlantAdres"],
                                (string)reader["ParkNaam"],
                                (string)reader["ParkLocatie"],
                                (string)reader["HuisAdres"],
                                (string)reader["HuisNummer"]
                            ));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesGeaffecteerdeReservaties), ex);
                }
            }
            return reservaties;
        }
        #endregion

        #region HuisInOnderhoudWindow
        public List<HuisInfo> LeesActieveHuizenOpPark(int parkId)
        {
            var huizen = new List<HuisInfo>();
            string sql = "SELECT * FROM Huis WHERE park_id = @parkId AND isActief = 1 ORDER BY straat, nummer;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@parkId", parkId);

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            huizen.Add(new HuisInfo(
                                (int)reader["id"],
                                (string)reader["straat"],
                                (string)reader["nummer"],
                                (bool)reader["isActief"],
                                (int)reader["capaciteit"]
                            ));
                        }
                    }

                    return huizen;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesActieveHuizenOpPark), ex);
                }
            }
        }
        public List<Reservatie> LeesReservatiesOpHuisID(int huisID)
        {
            var reservaties = new List<Reservatie>();
            string sql = @"
                            SELECT 
                                r.id AS ReservatieID, 
                                r.StartDatum, 
                                r.EindDatum, 
                                k.id AS KlantID, 
                                k.Naam AS KlantNaam, 
                                k.Adres AS KlantAdres
                            FROM 
                                Reservatie r
                            JOIN 
                                Klant k ON r.klant_id = k.id
                            WHERE 
                                r.huis_id = @huis_id AND 
                                r.EindDatum >= @vandaag
                            ORDER BY 
                                r.EindDatum, KlantNaam";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@huis_id", huisID);
                    cmd.Parameters.AddWithValue("@vandaag", DateTime.Today);

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservaties.Add(new Reservatie(
                                (int)reader["ReservatieID"],
                                (DateTime)reader["StartDatum"],
                                (DateTime)reader["EindDatum"],
                                new Klant(
                                    (int)reader["KlantID"],
                                    (string)reader["KlantNaam"],
                                    (string)reader["KlantAdres"]
                                )
                            ));
                        }
                    }
                    return reservaties;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesReservatiesOpHuisID), ex);
                }
            }
        }
        public void SchrijfGeaffecteerdeReservaiesEnHuisNietActief(int huisID, List<Reservatie> reservaties)
        {
            string updateHuisQuery = "UPDATE Huis SET isActief = 0 WHERE id = @huisID";
            string updateAffectedClientQuery = "UPDATE Reservatie SET isGeaffecteerd = 1 WHERE id = @reservatieID";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.Transaction = conn.BeginTransaction();

                    cmd.CommandText = updateHuisQuery;
                    cmd.Parameters.Add(new SqlParameter("@huisID", SqlDbType.Int)).Value = huisID;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = updateAffectedClientQuery;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@reservatieID", SqlDbType.Int));

                    foreach (var reservatie in reservaties)
                    {
                        cmd.Parameters["@reservatieID"].Value = reservatie.Id;
                        cmd.ExecuteNonQuery();
                    }
                    cmd.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new DomeinException(nameof(SchrijfGeaffecteerdeReservaiesEnHuisNietActief), ex);
                }
            }
        }
        #endregion

        #region ReservatiesZoekenWindow
        // TabItem 1 : Klant
        public List<ReservatiesOpKlantInfo> LeesReservatiesOpKlantId(int klantID)
        {
            string sql = @"
                            SELECT 
                                r.id AS 'Reservatie id',
                                r.startDatum AS 'Start datum',
                                r.eindDatum AS 'Eind datum',
                                p.naam AS 'Park naam',
                                p.locatie AS 'Park locatie',
                                h.id AS 'Huis id',
                                CONCAT(h.straat, ' ', h.nummer) AS 'Huis adres',
	                            h.capaciteit AS 'Huis capaciteit'
                            FROM 
                                Reservatie r
                            INNER JOIN 
                                Huis h ON r.huis_id = h.id
                            INNER JOIN 
                                Park p ON h.park_id = p.id
                            WHERE 
                                r.klant_id = @klantID
                            ORDER BY 
                                r.startDatum ASC, p.locatie ASC;";

            List<ReservatiesOpKlantInfo> reservatiesOpKlanten = new List<ReservatiesOpKlantInfo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;

                    cmd.Parameters.Add(new SqlParameter("@klantID", SqlDbType.Int));
                    cmd.Parameters["@klantID"].Value = klantID;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservatiesOpKlanten.Add(new ReservatiesOpKlantInfo
                            (
                                (int)reader["Reservatie id"],
                                Convert.ToDateTime(reader["Start datum"]).ToString("dd/MM/yyyy"),
                                Convert.ToDateTime(reader["Eind datum"]).ToString("dd/MM/yyyy"),
                                (string)reader["Park naam"],
                                (string)reader["Park locatie"],
                                (int)reader["Huis id"],
                                (string)reader["Huis adres"],
                                (int)reader["Huis capaciteit"]
                            ));
                        }
                    }
                    return reservatiesOpKlanten;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesReservatiesOpKlantId), ex);
                }
            }
        }
        // TabItem 2 : Park en periode
        public List<ReservatiesOpParkEnPeriodeInfo> LeesReservatiesOpParkEnPeriode(int parkID, DateTime startDatum, DateTime eindDatum)
        {
            string sql = @"
                            SELECT 
                                r.id AS 'Reservatie id',
                                r.startDatum AS 'Start datum',
	                            r.eindDatum AS 'Eind datum',
                                k.id AS 'Klant id',
                                k.naam AS 'Klant naam',
                                h.id AS 'Huis id',
                                CONCAT(h.straat, ' ', h.nummer) AS 'Huis Adres',
	                            h.capaciteit AS 'Huis capaciteit'
                            FROM 
                                Reservatie r
                            INNER JOIN 
                                Huis h ON r.huis_id = h.id
                            INNER JOIN 
                                Park p ON h.park_id = p.id
                            INNER JOIN 
                                Klant k ON r.klant_id = k.id
                            WHERE 
                                p.id = @parkID AND 
                                r.startDatum >= @startDatum AND 
                                r.eindDatum <= @eindDatum
                            ORDER BY 
                                r.startDatum, k.id;";

            List<ReservatiesOpParkEnPeriodeInfo> reservatiesOpParkEnPeriode = new List<ReservatiesOpParkEnPeriodeInfo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@parkID", SqlDbType.Int) { Value = parkID });
                    cmd.Parameters.Add(new SqlParameter("@startDatum", SqlDbType.Date) { Value = startDatum });
                    cmd.Parameters.Add(new SqlParameter("@eindDatum", SqlDbType.Date) { Value = eindDatum });

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservatiesOpParkEnPeriode.Add(new ReservatiesOpParkEnPeriodeInfo(
                                (int)reader["Reservatie id"],
                                Convert.ToDateTime(reader["Start datum"]).ToString("dd/MM/yyyy"),
                                Convert.ToDateTime(reader["Eind datum"]).ToString("dd/MM/yyyy"),
                                (int)reader["Klant id"],
                                (string)reader["Klant naam"],
                                (int)reader["Huis id"],
                                (string)reader["Huis Adres"],
                                (int)reader["Huis capaciteit"]
                            ));
                        }
                    }
                    return reservatiesOpParkEnPeriode;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesReservatiesOpParkEnPeriode), ex);
                }
            }
        }
        #endregion

        #region NieuweReservatieWindow

        // Gemeenschapelijk
        public List<int> LeesMaxCapaciteitenVanPark(int parkId) // Als nieuw park toevoegen met andere huizen capaciteiten.
        {
            List<int> capaciteitenList = new List<int>();
            string sql = @"SELECT MAX(h.capaciteit) AS MaxCapacity
                   FROM Huis h
                   WHERE h.park_id = @parkID";
            int maxCapacity = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add(new SqlParameter("@parkID", SqlDbType.Int));
                    cmd.Parameters["@parkID"].Value = parkId;

                    var result = cmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)//لو كانت القيمة مو نول ولا (لا شيء) في قاعدة البيانات
                    {
                        maxCapacity = (int)result;
                    }

                    // Adding capacities from 1 to maxCapacity
                    for (int i = 1; i <= maxCapacity; i++)
                    {
                        capaciteitenList.Add(i);
                    }

                    return capaciteitenList;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesMaxCapaciteitenVanPark), ex);
                }
            }
        }
        public List<HuisInfo> LeesBeschikbareHuizen(int parkId, int aantalPersonen, DateTime startDatum, DateTime eindDatum)
        {
            string sql = @"
                            SELECT h.*
                            FROM Huis h
                            WHERE h.park_id = @parkId
                                AND h.capaciteit >= @aantalPersonen
                                AND h.isActief = 1
                                AND NOT EXISTS (
                                    SELECT 1
                                    FROM Reservatie r
                                    WHERE r.huis_id = h.id
                                    AND r.startDatum <= @eindDatum
                                    AND r.eindDatum >= @startDatum
                                )
                            ORDER BY h.capaciteit, h.straat, h.nummer;";

            List<HuisInfo> beschikbareHuizen = new List<HuisInfo>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;

                    cmd.Parameters.Add(new SqlParameter("@parkId", SqlDbType.Int)).Value = parkId;
                    cmd.Parameters.Add(new SqlParameter("@aantalPersonen", SqlDbType.Int)).Value = aantalPersonen;
                    cmd.Parameters.Add(new SqlParameter("@startDatum", SqlDbType.Date)).Value = startDatum;
                    cmd.Parameters.Add(new SqlParameter("@eindDatum", SqlDbType.Date)).Value = eindDatum;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            beschikbareHuizen.Add(new HuisInfo(
                                (int)reader["id"],
                                (string)reader["straat"],
                                (string)reader["nummer"],
                                (bool)reader["isActief"],
                                (int)reader["capaciteit"]
                            ));
                        }
                    }
                    return beschikbareHuizen;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesBeschikbareHuizen), ex);
                }
            }
        }
        public void SchrijfReservatie(int klantId, DateTime startDatum, DateTime eindDatum, int huisId)
        {
            string insertReservatieSql = @"
                                            INSERT INTO Reservatie (klant_id, startDatum, eindDatum, huis_id, isGeaffecteerd)
                                            VALUES (@klantId, @startDatum, @eindDatum, @huisId, 0);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = insertReservatieSql;
                    cmd.Parameters.Add(new SqlParameter("@klantId", SqlDbType.Int) { Value = klantId });
                    cmd.Parameters.Add(new SqlParameter("@startDatum", SqlDbType.Date) { Value = startDatum });
                    cmd.Parameters.Add(new SqlParameter("@eindDatum", SqlDbType.Date) { Value = eindDatum });
                    cmd.Parameters.Add(new SqlParameter("@huisId", SqlDbType.Int) { Value = huisId });
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(SchrijfReservatie), ex);
                }
            }
        }

        // TabItem 2 : Faciliteiten
        public List<Faciliteit> LeesAlleFaciliteiten()
        {
            List<Faciliteit> faciliteiten = new List<Faciliteit>();
            string sql = "SELECT * FROM Faciliteit ORDER BY beschrijving;";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            faciliteiten.Add(new Faciliteit((int)reader["id"], (string)reader["beschrijving"]));
                        }
                    }
                    return faciliteiten;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesAlleFaciliteiten), ex);
                }
            }
        }
        public List<ParkInfo> LeesParkenOpFaciliteiten(List<Faciliteit> faciliteiten)
        {
            string faciliteitenIds = "";

            List<ParkInfo> parken = new List<ParkInfo>();

            for (int i = 0; i < faciliteiten.Count; i++)
            {
                faciliteitenIds += $"@pf{i},";
            }
            faciliteitenIds = faciliteitenIds.Remove(faciliteitenIds.Length - 1);
            string sql = $@"
                            SELECT p.id, p.naam, p.locatie
                            FROM Park p
                            INNER JOIN Park_Faciliteit pf ON p.id = pf.park_id
                            WHERE pf.faciliteit_id IN ({faciliteitenIds})
                            GROUP BY p.id, p.naam, p.locatie
                            HAVING COUNT(DISTINCT pf.faciliteit_id) = @aantalFaciliteiten";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;

                    cmd.Parameters.Add(new SqlParameter("@aantalFaciliteiten", SqlDbType.Int));
                    cmd.Parameters["@aantalFaciliteiten"].Value = faciliteiten.Count;

                    for (int i = 0; i < faciliteiten.Count; i++)
                    {
                        cmd.Parameters.AddWithValue($"@pf{i}", faciliteiten[i].Id);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            parken.Add(new ParkInfo((int)reader["id"], (string)reader["naam"], (string)reader["locatie"]));
                        }
                    }
                    return parken;
                }
                catch (Exception ex)
                {
                    throw new DomeinException(nameof(LeesParkenOpFaciliteiten), ex);
                }
            }
        }
        #endregion
    }
}