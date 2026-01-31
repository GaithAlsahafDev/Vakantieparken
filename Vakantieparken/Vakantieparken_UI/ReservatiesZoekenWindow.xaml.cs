using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vakantieparken_UI
{
    /// <summary>
    /// Interaction logic for ReservatiesZoekenWindow.xaml
    /// </summary>
    public partial class ReservatiesZoekenWindow : Window
    {
        public ReservatiesZoekenWindow()
        {
            InitializeComponent();
            LaadData();
        }

        private void LaadData()
        {
            // تعبئة قائمة أسماء العملاء
            KlantNaamComboBox.ItemsSource = new List<string> { "Klant 1", "Klant 2", "Klant 3" };

            // تعبئة قائمة المتنزهات
            ParkComboBox.ItemsSource = new List<string> { "Park 1", "Park 2", "Park 3" };
        }

        private void Zoeken_Click(object sender, RoutedEventArgs e)
        {
            // التحقق من طريقة البحث
            if (KlantNaamComboBox.SelectedItem != null)
            {
                // البحث بناءً على اسم العميل
                string klantNaam = KlantNaamComboBox.SelectedItem.ToString();
                ReservatiesListBox.ItemsSource = ZoekReservatiesOpKlant(klantNaam);
            }
            else if (ParkComboBox.SelectedItem != null && StartDatumPicker.SelectedDate.HasValue && EindDatumPicker.SelectedDate.HasValue)
            {
                // البحث بناءً على المتنزه والفترة الزمنية
                string parkNaam = ParkComboBox.SelectedItem.ToString();
                var startDatum = StartDatumPicker.SelectedDate.Value;
                var eindDatum = EindDatumPicker.SelectedDate.Value;
                ReservatiesListBox.ItemsSource = ZoekReservatiesOpParkEnPeriode(parkNaam, startDatum, eindDatum);
            }
            else
            {
                MessageBox.Show("Gelieve een klantnaam te kiezen of een park en periode op te geven!", "Fout", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private List<string> ZoekReservatiesOpKlant(string klantNaam)
        {
            // محاكاة البحث بناءً على اسم العميل
            return new List<string>
            {
                $"Reservatie 1 - {klantNaam} - 01/01/2024 - 03/01/2024",
                $"Reservatie 2 - {klantNaam} - 05/01/2024 - 10/01/2024"
            };
        }

        private List<string> ZoekReservatiesOpParkEnPeriode(string parkNaam, System.DateTime startDatum, System.DateTime eindDatum)
        {
            // محاكاة البحث بناءً على المتنزه والفترة الزمنية
            return new List<string>
            {
                $"{parkNaam} - Reservatie 1 - 01/01/2024 - 03/01/2024",
                $"{parkNaam} - Reservatie 2 - 05/01/2024 - 10/01/2024"
            };
        }

        private void TerugNaarStartpagina_Click(object sender, RoutedEventArgs e)
        {
            // فتح الصفحة الرئيسية
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // إغلاق الصفحة الحالية
            this.Close();
        }
    }
}
