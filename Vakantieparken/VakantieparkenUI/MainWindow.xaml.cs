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
using VakantieparkenUI;

namespace VakantieparkenUI
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
            var nieuweReservatieWindow = new NieuweReservatieWindow();
            nieuweReservatieWindow.Show();
            this.Close();
        }

        private void ReservatiesZoeken_Click(object sender, RoutedEventArgs e)
        {
            var reservatiesZoekenWindow = new ReservatiesZoekenWindow();
            reservatiesZoekenWindow.Show();
            this.Close();
        }

        private void HuisInOnderhoudPlaatsen_Click(object sender, RoutedEventArgs e)
        {
            var huisInOnderhoudWindow = new HuisInOnderhoudWindow();
            huisInOnderhoudWindow.Show();
            this.Close();
        }

        private void GeaffecteerdeKlantenTonen_Click(object sender, RoutedEventArgs e)
        {
            var geaffecteerdeKlantenWindow = new GeaffecteerdeReservatiesWindow();
            geaffecteerdeKlantenWindow.Show();
            this.Close();
        }
    }
}