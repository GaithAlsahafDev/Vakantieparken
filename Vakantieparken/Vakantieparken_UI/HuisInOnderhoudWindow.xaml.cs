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
    /// Interaction logic for HuisInOnderhoudWindow.xaml
    /// </summary>
    public partial class HuisInOnderhoudWindow : Window
    {
        public HuisInOnderhoudWindow()
        {
            InitializeComponent();
            LaadData();
        }

        private void LaadData()
        {
            // تعبئة قائمة المتنزهات
            ParkComboBox.ItemsSource = new List<string> { "Park 1", "Park 2", "Park 3" };
        }

        private void ParkComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ParkComboBox.SelectedItem != null)
            {
                string geselecteerdPark = ParkComboBox.SelectedItem.ToString();

                // تعبئة قائمة المنازل بناءً على البارك
                HuisComboBox.ItemsSource = new List<string>
                {
                    $"{geselecteerdPark} - Huis 1",
                    $"{geselecteerdPark} - Huis 2"
                };
                HuisComboBox.IsEnabled = true; // تفعيل قائمة المنازل
            }
        }

        private void HuisComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HuisComboBox.SelectedItem != null)
            {
                string geselecteerdHuis = HuisComboBox.SelectedItem.ToString();

                // تعبئة قائمة الحجوزات بناءً على المنزل
                ReservatiesListBox.ItemsSource = new List<string>
                {
                    $"{geselecteerdHuis} - Reservatie 1 - 01/01/2024 - 03/01/2024",
                    $"{geselecteerdHuis} - Reservatie 2 - 05/01/2024 - 10/01/2024"
                };
                ReservatiesListBox.IsEnabled = true; // تفعيل قائمة الحجوزات
            }
        }

        private void PlaatsInOnderhoud_Click(object sender, RoutedEventArgs e)
        {
            string geselecteerdHuis = HuisComboBox.SelectedItem?.ToString();

            if (geselecteerdHuis == null)
            {
                MessageBox.Show("Gelieve een huis te selecteren!", "Fout", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // تنفيذ المهام المتعلقة بوضع المنزل تحت الصيانة (محاكاة)
            MessageBox.Show($"Huis {geselecteerdHuis} succesvol in onderhoud geplaatst!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            // فتح الصفحة الرئيسية
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // إغلاق نافذتي الحجز والصيانة
            this.Close();
        }
    }
}
