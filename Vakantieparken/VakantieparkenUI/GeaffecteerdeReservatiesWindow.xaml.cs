using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using VakantieparkenBL.Interfaces;
using VakantieparkenBL.Managers;
using VakantieparkenBL.Model;
using VakantieparkenDL_SQL;

namespace VakantieparkenUI
{
    public partial class GeaffecteerdeReservatiesWindow : Window
    {
        private IVakantieparkenRepository vakantieparkenRepository;
        private VakantieparkenManager vakantieparkenManager;
        public GeaffecteerdeReservatiesWindow()
        {
            InitializeComponent();
            vakantieparkenRepository = new VakantieparkenRepository(ConfigurationManager.ConnectionStrings["DB_Vakantieparken_Connection"].ToString());
            vakantieparkenManager = new VakantieparkenManager(vakantieparkenRepository);

            ReservatiesDataGrid.ItemsSource = vakantieparkenManager.GeefGeaffecteerdeReservaties();
        }
        private void TerugNaarStartpagina_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
