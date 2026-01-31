using System;
using System.Collections.Generic;
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
    public partial class ReservatiesZoekenWindow : Window
    {
        IVakantieparkenRepository vakantieparkenRepository;
        VakantieparkenManager vakantieparkenManager;

        public ReservatiesZoekenWindow()
        {
            InitializeComponent();
            vakantieparkenRepository = new VakantieparkenRepository(ConfigurationManager.ConnectionStrings["DB_Vakantieparken_Connection"].ToString());
            vakantieparkenManager = new VakantieparkenManager(vakantieparkenRepository);

            ParkComboBox.ItemsSource = vakantieparkenManager.GeefParks();
        }

        //TabItemKlant
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
                ReservatiesDataGrid.ItemsSource = null;
            }
        }
        private void KlantenDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KlantenDataGrid.SelectedItem != null)
            {
                
                int klantID = ((Klant)KlantenDataGrid.SelectedItem).Id;
                var reservaties = vakantieparkenManager.ZoekReservatiesOpKlantId(klantID);
                if (reservaties != null && reservaties.Count > 0)
                {
                    ReservatiesDataGrid.IsEnabled = true;
                    ReservatiesDataGrid.ItemsSource = reservaties;
                }
                else
                {
                    MessageBox.Show("Geen reservaties gevonden voor deze klant!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReservatiesDataGrid.ItemsSource = null;
                    ReservatiesDataGrid.IsEnabled = false;
                }
            }
            else
            {
                ReservatiesDataGrid.ItemsSource = null;
                ReservatiesDataGrid.IsEnabled = false;
            }
        }
        private void TerugNaarStartpagina_TabItemKlant_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        //TabItemParkEnPeriode
        private void ReservatieZoekenOpParkEnPeriode_Click(object sender, RoutedEventArgs e)
        {
            ReservatiesOpParkEnPeriodeDataGrid.ItemsSource = null;
            if (ParkComboBox.SelectedItem != null && StartDatumPicker.SelectedDate.HasValue && EindDatumPicker.SelectedDate.HasValue)
            {
                int parkID = ((ParkInfo)ParkComboBox.SelectedItem).Id;
                DateTime startDatum = StartDatumPicker.SelectedDate.Value;
                DateTime eindDatum = EindDatumPicker.SelectedDate.Value;

                var reservaties = vakantieparkenManager.ZoekReservatiesOpParkEnPeriode(parkID, startDatum, eindDatum);
                if (reservaties != null && reservaties.Count > 0)
                {
                    ReservatiesOpParkEnPeriodeDataGrid.ItemsSource = reservaties;
                }
                else if (StartDatumPicker.SelectedDate > EindDatumPicker.SelectedDate)
                {
                    MessageBox.Show("De datum (van) moet vóór de datum (tot) liggen. Controleer de ingevoerde data.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReservatiesOpParkEnPeriodeDataGrid.ItemsSource = null;
                }
                else
                {
                    MessageBox.Show("Geen reservaties gevonden voor deze selectie!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReservatiesOpParkEnPeriodeDataGrid.ItemsSource = null;
                }
            }
            else
            {
                MessageBox.Show("Gelieve een park en periode in te vullen!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void TerugNaarStartpagina_TabItemParkEnPeriode_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
