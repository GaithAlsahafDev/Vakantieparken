using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vakantieparken_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NieuweReservatieMaken_Click(object sender, RoutedEventArgs e)
        {
            // Open een nieuw venster voor het maken van een reservatie
            var nieuweReservatieWindow = new NieuweReservatieWindow();
            nieuweReservatieWindow.Show();
        }

        private void ReservatiesZoeken_Click(object sender, RoutedEventArgs e)
        {
            // Open een nieuw venster voor het zoeken van reservaties
            var reservatiesZoekenWindow = new ReservatiesZoekenWindow();
            reservatiesZoekenWindow.Show();
        }

        private void HuisInOnderhoudPlaatsen_Click(object sender, RoutedEventArgs e)
        {
            // Open een nieuw venster om een huis in onderhoud te plaatsen
            var huisInOnderhoudWindow = new HuisInOnderhoudWindow();
            huisInOnderhoudWindow.Show();
        }

        private void GeaffecteerdeKlantenTonen_Click(object sender, RoutedEventArgs e)
        {
            // Open een nieuw venster om geaffecteerde klanten te tonen
            var geaffecteerdeKlantenWindow = new GeaffecteerdeKlantenWindow();
            geaffecteerdeKlantenWindow.Show();
        }
    }
}