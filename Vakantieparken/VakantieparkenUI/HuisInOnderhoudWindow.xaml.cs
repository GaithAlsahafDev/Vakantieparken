using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using VakantieparkenBL.DTO;
using VakantieparkenBL.Interfaces;
using VakantieparkenBL.Managers;
using VakantieparkenBL.Model;
using VakantieparkenDL_SQL;

namespace VakantieparkenUI
{
    public partial class HuisInOnderhoudWindow : Window
    {
        private IVakantieparkenRepository vakantieparkenRepository;
        private VakantieparkenManager vakantieparkenManager;

        public HuisInOnderhoudWindow()
        {
            InitializeComponent();
            vakantieparkenRepository = new VakantieparkenRepository(ConfigurationManager.ConnectionStrings["DB_Vakantieparken_Connection"].ToString());
            vakantieparkenManager = new VakantieparkenManager(vakantieparkenRepository);

            ParkComboBox.ItemsSource = vakantieparkenManager.GeefParks();
        }
        private void ParkComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ParkComboBox.SelectedItem != null)
            {
                ParkInfo park = (ParkInfo) ParkComboBox.SelectedItem;
                var huizenVanPark = vakantieparkenManager.GeefActieveHuizenOpPark(park.Id);
                if (huizenVanPark != null || huizenVanPark.Count > 0)
                {
                    HuisComboBox.ItemsSource = huizenVanPark;
                    HuisComboBox.IsEnabled = true;

                    ReservatiesDataGrid.ItemsSource = null;
                    ReservatiesDataGrid.IsEnabled = false;
                }
                else
                {
                    MessageBox.Show("Er zijn momenteel geen actieve huizen in dit park.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReservatiesDataGrid.ItemsSource = null;
                }
            }
            else
            {
                HuisComboBox.SelectedItem = null;
                HuisComboBox.IsEnabled = false;
            }
        }
        private void HuisComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (HuisComboBox.SelectedItem != null)
            {
                int geselecteerdHuisID = ((HuisInfo)HuisComboBox.SelectedItem).Id;

                var reservatiesList = vakantieparkenManager.GeefReservatiesOpHuisID(geselecteerdHuisID);

                if (reservatiesList != null || reservatiesList.Count > 0)
                {
                    ReservatiesDataGrid.ItemsSource = reservatiesList;
                    ReservatiesDataGrid.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Gelukkig zijn er momenteel of in de toekomst geen reserveringen gekoppeld aan dit huis.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReservatiesDataGrid.ItemsSource = null;
                }                
            }
            else
            {
                ReservatiesDataGrid.ItemsSource = null;
                ReservatiesDataGrid.IsEnabled = false;
            }
        }
        private void PlaatsInOnderhoud_Click(object sender, RoutedEventArgs e)
        {
            HuisInfo geselecteerdHuis = (HuisInfo)HuisComboBox.SelectedItem;

            if (geselecteerdHuis == null || ParkComboBox.SelectedItem == null)
            {
                MessageBox.Show("Gelieve een park en een huis te selecteren!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                List<Reservatie> reservatiesList = (List<Reservatie>) ReservatiesDataGrid.ItemsSource;
                vakantieparkenManager.MaakReservatiesGeaffecteerdEnHuisNietActiefMaken(((HuisInfo)HuisComboBox.SelectedItem).Id, reservatiesList);

                MessageBox.Show($"Huis '{geselecteerdHuis.Straat} {geselecteerdHuis.HuisNummer}' succesvol in onderhoud geplaatst!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                var huisInOnderhoudWindow = new HuisInOnderhoudWindow();
                huisInOnderhoudWindow.Show();
                Close();
            }
        }
        private void TerugNaarStartpagina_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
