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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Data.SqlClient;
using System.Data;

namespace Sovelluskehitys_esimerkki
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

        private void painike_hae_Click(object sender, RoutedEventArgs e)
        {
            string polku = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\k5000833\\source\\repos\\Sovelluskehitys_esimerkki\\tuotekanta.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection kanta = new SqlConnection(polku);
            kanta.Open();

            /*tehdään sql komento*/
            SqlCommand komento = kanta.CreateCommand();
            komento.CommandText = "SELECT * FROM tuotteet"; // kysely

            /*tehdään data adapteri ja taulu johon tiedot haetaan*/
            SqlDataAdapter adapteri = new SqlDataAdapter(komento);
            DataTable dt = new DataTable("tuotteet");
            adapteri.Fill(dt);

            /*sijoitetaan data-taulun tiedot DataGridiin*/
            tuote_lista.ItemsSource = dt.DefaultView;

            kanta.Close();
        }
    }
}
