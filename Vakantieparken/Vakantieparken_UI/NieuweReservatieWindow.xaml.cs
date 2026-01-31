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
    /// Interaction logic for NieuweReservatieWindow.xaml
    /// </summary>
    public partial class NieuweReservatieWindow : Window
    {
        private bool isLocatieSelected = false;
        private bool isFaciliteitSelected = false;

        public NieuweReservatieWindow()
        {
            InitializeComponent();
            LaadData();
        }

        private void LaadData()
        {
            // تعبئة قائمة العملاء والمواقع وعدد الأشخاص
            KlantComboBox.ItemsSource = new List<string> { "Klant 1", "Klant 2", "Klant 3" };
            LocatieComboBox.ItemsSource = new List<string> { "Locatie 1", "Locatie 2", "Locatie 3" };
            AantalPersonenComboBox.ItemsSource = new List<int> { 1, 2, 3, 4, 5, 6 };
        }

        private void LocatieComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LocatieComboBox.SelectedItem != null)
            {
                isLocatieSelected = true;
                string geselecteerdeLocatie = LocatieComboBox.SelectedItem.ToString();

                // تعبئة قائمة المرافق بناءً على الموقع
                FaciliteitComboBox.ItemsSource = new List<string>
                {
                    $"{geselecteerdeLocatie} - Zwembad",
                    $"{geselecteerdeLocatie} - Minigolf",
                    $"{geselecteerdeLocatie} - Restaurant"
                };
                FaciliteitComboBox.IsEnabled = true;
            }
        }

        private void FaciliteitComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FaciliteitComboBox.SelectedItem != null)
            {
                isFaciliteitSelected = true;
                string geselecteerdeLocatie = LocatieComboBox.SelectedItem?.ToString();
                string geselecteerdeFaciliteit = FaciliteitComboBox.SelectedItem?.ToString();

                // تعبئة قائمة الباركات بناءً على الموقع والمرفق
                ParkComboBox.ItemsSource = new List<string>
                {
                    $"{geselecteerdeLocatie} - {geselecteerdeFaciliteit} Park 1",
                    $"{geselecteerdeLocatie} - {geselecteerdeFaciliteit} Park 2"
                };
                ParkComboBox.IsEnabled = true;
            }
        }

        private void ParkComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ParkComboBox.SelectedItem != null)
            {
                string geselecteerdPark = ParkComboBox.SelectedItem.ToString();
                int? aantalPersonen = AantalPersonenComboBox.SelectedItem as int?;
                var startDatum = StartDatumPicker.SelectedDate;
                var eindDatum = EindDatumPicker.SelectedDate;

                if (aantalPersonen != null && startDatum.HasValue && eindDatum.HasValue)
                {
                    // تعبئة قائمة المنازل بناءً على البارك، التواريخ، وعدد الأشخاص
                    HuizenListBox.ItemsSource = new List<string>
                    {
                        $"{geselecteerdPark} - Huis 1 - {aantalPersonen} personen",
                        $"{geselecteerdPark} - Huis 2 - {aantalPersonen} personen"
                    };
                    HuizenListBox.IsEnabled = true;
                }
            }
        }

        private void NieuweReservatieToevoegen_Click(object sender, RoutedEventArgs e)
        {
            string geselecteerdeKlant = KlantComboBox.SelectedItem?.ToString();
            string geselecteerdHuis = HuizenListBox.SelectedItem?.ToString();
            var startDatum = StartDatumPicker.SelectedDate;
            var eindDatum = EindDatumPicker.SelectedDate;

            if (geselecteerdeKlant == null || geselecteerdHuis == null || !startDatum.HasValue || !eindDatum.HasValue)
            {
                MessageBox.Show("Vul alle velden in!");
                return;
            }

            MessageBox.Show("Nieuwe reservatie succesvol toegevoegd!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            // فتح الصفحة الرئيسية
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // إغلاق الصفحة الحالية
            this.Close();
        }
    }
}
