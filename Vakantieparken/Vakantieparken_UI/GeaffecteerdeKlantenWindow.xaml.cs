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
    /// Interaction logic for GeaffecteerdeKlantenWindow.xaml
    /// </summary>
    public partial class GeaffecteerdeKlantenWindow : Window
    {
        private List<string> klantenLijst;

        public GeaffecteerdeKlantenWindow()
        {
            InitializeComponent();
            LaadKlanten();
        }

        private void LaadKlanten()
        {
            // تحميل العملاء المتأثرين مع تفاصيل منازلهم (محاكاة)
            klantenLijst = new List<string>
            {
                "Klant 1, Adres: Straat 1, Huis: Huis 1 - 4 personen",
                "Klant 2, Adres: Straat 2, Huis: Huis 2 - 6 personen",
                "Klant 3, Adres: Straat 3, Huis: Huis 3 - 3 personen"
            };

            KlantenListBox.ItemsSource = klantenLijst;
        }

        private void VerwijderKlant_Click(object sender, RoutedEventArgs e)
        {
            // التحقق من العميل المحدد
            var geselecteerdeKlant = (sender as FrameworkElement)?.DataContext as string;

            if (geselecteerdeKlant != null && klantenLijst.Contains(geselecteerdeKlant))
            {
                // إزالة العميل من القائمة
                klantenLijst.Remove(geselecteerdeKlant);
                KlantenListBox.ItemsSource = null; // إعادة تحميل البيانات
                KlantenListBox.ItemsSource = klantenLijst;
            }
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
