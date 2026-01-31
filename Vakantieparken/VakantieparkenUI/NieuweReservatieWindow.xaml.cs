using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VakantieparkenBL.DTO;
using VakantieparkenBL.Interfaces;
using VakantieparkenBL.Managers;
using VakantieparkenBL.Model;
using VakantieparkenDL_SQL;

namespace VakantieparkenUI
{
    public partial class NieuweReservatieWindow : Window
    {
        IVakantieparkenRepository vakantieparkenRepository;
        VakantieparkenManager vakantieparkenManager;

        ObservableCollection<Faciliteit> AlleFaciliteiten;
        ObservableCollection<Faciliteit> GeselecteerdeFaciliteiten;

        public NieuweReservatieWindow()
        {
            vakantieparkenRepository = new VakantieparkenRepository(ConfigurationManager.ConnectionStrings["DB_Vakantieparken_Connection"].ToString());
            vakantieparkenManager = new VakantieparkenManager(vakantieparkenRepository);
            InitializeComponent();
            
            AlleFaciliteiten = new ObservableCollection<Faciliteit>(vakantieparkenManager.GeefAlleFaciliteiten());
            AlleFaciliteitenListBox.ItemsSource = AlleFaciliteiten;
            GeselecteerdeFaciliteiten = new ObservableCollection<Faciliteit>();
            GeselecteerdeFaciliteitenListBox.ItemsSource = GeselecteerdeFaciliteiten;
        }

        private void ControleerKlant_Click(object sender, RoutedEventArgs e)
        {
            string klantNaam = KlantNaamTextBox.Text;

            if (string.IsNullOrEmpty(klantNaam))
            {
                MessageBox.Show("Voer een klantnaam in.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                KlantenDataGrid.ItemsSource = null;
                return;
            }
            else if (vakantieparkenManager.BestaatKlant(klantNaam))
            {
                KlantenDataGrid.ItemsSource = vakantieparkenManager.GeefKlantenOpNaam(klantNaam);
            }
            else
            {
                MessageBox.Show($"Klant '{klantNaam}' bestaat niet in het systeem!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                KlantenDataGrid.ItemsSource = null;
                LocatieComboBox.IsEnabled = false;
            }
        }
        private void KlantenDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KlantenDataGrid.SelectedItem != null)
            {
                LocatieComboBox.ItemsSource = vakantieparkenManager.GeefParks().Select(p => p.Locatie).ToList();
                LocatieComboBox.IsEnabled = true;
                AlleFaciliteitenListBox.IsEnabled = true;
                GeselecteerdeFaciliteitenListBox.IsEnabled = true;
                VoegenButtons.IsEnabled = true;
            }
            else
            {
                LocatieComboBox.IsEnabled = false;
                LocatieComboBox.SelectedItem = null;

                VerwijderAlleFaciliteitenButton_Click(null, null);
                AlleFaciliteitenListBox.IsEnabled = false;
                GeselecteerdeFaciliteitenListBox.IsEnabled = false;
                VoegenButtons.IsEnabled = false;
            }
        }


        private void LocatieComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LocatieComboBox.SelectedItem != null)
            {
                ParkComboBox1.IsEnabled = true;
                ParkComboBox1.ItemsSource = vakantieparkenManager.GeefParks()
                                                                  .Where(p => p.Locatie == LocatieComboBox.SelectedItem.ToString())
                                                                  .Select(p => p.Naam).ToList();
            }
            else
            {
                ParkComboBox1.IsEnabled = false;
                ParkComboBox1.SelectedItem = null;
            }
        }
        private void ParkComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParkComboBox1.SelectedItem != null)
            {
                AantalPersonenComboBox1.IsEnabled = true;
                int parkId = vakantieparkenManager.GeefParks()
                                .Where(p => p.Locatie == LocatieComboBox.SelectedItem.ToString()
                                         && p.Naam == ParkComboBox1.SelectedItem.ToString())
                                .Select(p => p.Id)
                                .FirstOrDefault();

                AantalPersonenComboBox1.ItemsSource = vakantieparkenManager.GeefMaxCapaciteitenVanPark(parkId);
            }
            else
            {
                AantalPersonenComboBox1.IsEnabled = false;
                AantalPersonenComboBox1.SelectedItem = null;

                BeginDatumPicker1.SelectedDate = null;
                EindDatumPicker1.SelectedDate = null;

                HuizenListBox1.ItemsSource = null;
            }
        }
        private void HaalHuizenOp1_Click(object sender, RoutedEventArgs e)
        {
            if (ParkComboBox1.SelectedItem == null || BeginDatumPicker1.SelectedDate == null || EindDatumPicker1.SelectedDate == null
                || AantalPersonenComboBox1.SelectedItem == null || KlantenDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vul alle verplichte velden in voordat u huizen ophaalt.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                if (BeginDatumPicker1.SelectedDate > EindDatumPicker1.SelectedDate)
                {
                    MessageBox.Show("De datum (van) moet vóór de datum (tot) liggen. Controleer de ingevoerde data.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    HuizenListBox1.ItemsSource = null;
                    return;
                }
                else if (BeginDatumPicker1.SelectedDate < DateTime.Today || EindDatumPicker1.SelectedDate < DateTime.Today)
                {
                    MessageBox.Show("Je kunt niet reserveren op een datum vóór vandaag.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    HuizenListBox1.ItemsSource = null;
                    return;
                }
                else
                {
                    int parkId = vakantieparkenManager.GeefParks()
                                                      .Where(p => p.Locatie == LocatieComboBox.SelectedItem.ToString()
                                                             && p.Naam == ParkComboBox1.SelectedItem.ToString())
                                                      .Select(p => p.Id)
                                                      .FirstOrDefault();

                    int aantalPersonen = (int)AantalPersonenComboBox1.SelectedItem;
                    DateTime startDatum = BeginDatumPicker1.SelectedDate.Value;
                    DateTime eindDatum = EindDatumPicker1.SelectedDate.Value;

                    HuizenListBox1.ItemsSource = vakantieparkenManager.GeefBeschikbareHuizen(parkId, aantalPersonen, startDatum, eindDatum);

                    if (HuizenListBox1.ItemsSource == null || (HuizenListBox1.ItemsSource as List<HuisInfo>).Count == 0)
                    {
                        MessageBox.Show("Helaas is er geen huis dat aan jouw huidige vereisten voldoet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
        private void ReservatieToevoegen1_Click(object sender, RoutedEventArgs e)
        {
            if (KlantenDataGrid.SelectedItem == null || ParkComboBox1.SelectedItem == null || BeginDatumPicker1.SelectedDate == null
                || EindDatumPicker1.SelectedDate == null || AantalPersonenComboBox1.SelectedItem == null)
            {
                MessageBox.Show("Vul alle verplichte velden in voordat u huizen ophaalt.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                if (HuizenListBox1.SelectedItem != null)
                {
                    Klant klant = (Klant)KlantenDataGrid.SelectedItem;
                    HuisInfo huis = (HuisInfo)HuizenListBox1.SelectedItem;
                    vakantieparkenManager.VoegReservatieToe(klant.Id, (DateTime)BeginDatumPicker1.SelectedDate, (DateTime)EindDatumPicker1.SelectedDate, huis.Id);
                    MessageBox.Show($"Succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                    var nieuweReservatieWindow = new NieuweReservatieWindow();
                    nieuweReservatieWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("U moet een huis kiezen", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
        }
        private void TerugNaarStartpagina1_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }


        private void VoegAlleFaciliteitenToeButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Faciliteit f in AlleFaciliteiten.ToList())
            {
                GeselecteerdeFaciliteiten.Add(f);
                AlleFaciliteiten.Remove(f);
            }

            GeselecteerdeFaciliteitenListBox_SelectionChanged(null, null);
        }
        private void VoegFaciliteitToeButton_Click(object sender, RoutedEventArgs e)
        {
            List<Faciliteit> faciliteiten = new();
            foreach (Faciliteit f in AlleFaciliteitenListBox.SelectedItems)
            {
                faciliteiten.Add(f);
            }

            foreach (Faciliteit f in faciliteiten)
            {
                GeselecteerdeFaciliteiten.Add(f);
                AlleFaciliteiten.Remove(f);
            }

            GeselecteerdeFaciliteitenListBox_SelectionChanged(null, null);
        }
        private void VerwijderFaciliteitButton_Click(object sender, RoutedEventArgs e)
        {
            List<Faciliteit> faciliteiten = new();

            foreach (Faciliteit f in GeselecteerdeFaciliteitenListBox.SelectedItems)
            {
                faciliteiten.Add(f);
            }

            foreach (Faciliteit f in faciliteiten)
            {
                GeselecteerdeFaciliteiten.Remove(f);
                AlleFaciliteiten.Add(f);
            }

            GeselecteerdeFaciliteitenListBox_SelectionChanged(null, null);
        }
        private void VerwijderAlleFaciliteitenButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Faciliteit f in GeselecteerdeFaciliteiten.ToList())
            {
                AlleFaciliteiten.Add(f);
            }

            GeselecteerdeFaciliteiten.Clear();

            GeselecteerdeFaciliteitenListBox_SelectionChanged(null, null);
        }
        private void GeselecteerdeFaciliteitenListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GeselecteerdeFaciliteiten != null && GeselecteerdeFaciliteiten.Count > 0)
            {
                ParkComboBox2.IsEnabled = true;
                var faciliteiten = GeselecteerdeFaciliteiten.ToList();
                ParkComboBox2.ItemsSource = vakantieparkenManager.GeefParkenOpFaciliteiten(faciliteiten);
            }
            else
            {
                ParkComboBox2.IsEnabled = false;
                ParkComboBox2.ItemsSource = null;
            }
        }
        private void ParkComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParkComboBox2.SelectedItem != null)
            {
                AantalPersonenComboBox2.IsEnabled = true;
                int parkID = ((ParkInfo)ParkComboBox2.SelectedItem).Id;
                AantalPersonenComboBox2.ItemsSource = vakantieparkenManager.GeefMaxCapaciteitenVanPark(parkID);
            }
            else
            {
                BeginDatumPicker2.SelectedDate = null;
                EindDatumPicker2.SelectedDate = null;

                AantalPersonenComboBox2.IsEnabled = false;
                AantalPersonenComboBox2.SelectedItem = null;

                HuizenListBox2.ItemsSource = null;
            }
        }
        private void HaalHuizenOp2_Click(object sender, RoutedEventArgs e)
        {
            if (ParkComboBox2.SelectedItem == null || BeginDatumPicker2.SelectedDate == null || EindDatumPicker2.SelectedDate == null
                || AantalPersonenComboBox2.SelectedItem == null || KlantenDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vul alle verplichte velden in voordat u huizen ophaalt.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                if (BeginDatumPicker2.SelectedDate > EindDatumPicker2.SelectedDate)
                {
                    MessageBox.Show("De datum (van) moet vóór de datum (tot) liggen. Controleer de ingevoerde data.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    HuizenListBox2.ItemsSource = null;
                    return;
                }
                else if (BeginDatumPicker2.SelectedDate < DateTime.Today || EindDatumPicker2.SelectedDate < DateTime.Today)
                {
                    MessageBox.Show("Je kunt niet reserveren op een datum vóór vandaag.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    HuizenListBox2.ItemsSource = null;
                    return;
                }
                else
                {
                    int parkId = (ParkComboBox2.SelectedItem as ParkInfo).Id;
                    int aantalPersonen = (int)AantalPersonenComboBox2.SelectedItem;
                    DateTime startDatum = BeginDatumPicker2.SelectedDate.Value;
                    DateTime eindDatum = EindDatumPicker2.SelectedDate.Value;

                    HuizenListBox2.ItemsSource = vakantieparkenManager.GeefBeschikbareHuizen(parkId, aantalPersonen, startDatum, eindDatum);

                    if (HuizenListBox2.ItemsSource == null || (HuizenListBox2.ItemsSource as List<HuisInfo>).Count == 0)
                    {
                        MessageBox.Show("Helaas is er geen huis dat aan jouw huidige vereisten voldoet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
        private void ReservatieToevoegen2_Click(object sender, RoutedEventArgs e)
        {
            if (KlantenDataGrid.SelectedItem == null || ParkComboBox2.SelectedItem == null || BeginDatumPicker2.SelectedDate == null
                || EindDatumPicker2.SelectedDate == null || AantalPersonenComboBox2.SelectedItem == null)
            {
                MessageBox.Show("Vul alle verplichte velden in voordat u huizen ophaalt.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                if (HuizenListBox2.SelectedItem != null)
                {
                    Klant klant = (Klant)KlantenDataGrid.SelectedItem;
                    HuisInfo huis = (HuisInfo)HuizenListBox2.SelectedItem;
                    vakantieparkenManager.VoegReservatieToe(klant.Id, (DateTime)BeginDatumPicker2.SelectedDate, (DateTime)EindDatumPicker2.SelectedDate, huis.Id);
                    MessageBox.Show($"Succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                    var nieuweReservatieWindow = new NieuweReservatieWindow();
                    nieuweReservatieWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("U moet een huis kiezen", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
        }
        private void TerugNaarStartpagina2_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
