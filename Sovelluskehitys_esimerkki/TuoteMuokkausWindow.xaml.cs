using Microsoft.Data.SqlClient;
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

namespace Sovelluskehitys_esimerkki
{
    /// <summary>
    /// Interaction logic for TuoteMuokkausWindow.xaml
    /// </summary>
    public partial class TuoteMuokkausWindow : Window
    {
        private int tuoteId;
        private SqlConnection paaikkunanKanta;

        public TuoteMuokkausWindow(int tuoteId, SqlConnection kanta)
        {
            InitializeComponent();
            this.tuoteId = tuoteId;
            this.paaikkunanKanta = kanta;

            // Voit tehdä tarvittavat toimet, kuten ladata tietoja annetun tuoteId:n perusteella
            // ja päivittää sitten käyttöliittymän vastaavasti.
        }
        public TuoteMuokkausWindow(int tuoteId)
        {
            InitializeComponent();
            this.tuoteId = tuoteId;

            // Lataa tiedot annetun tuoteId:n perusteella
            LataaTuotetiedot();
        }

        private void LataaTuotetiedot()
        {
            try
            {
                // Tässä voit käyttää tietokantayhteyttä ja suorittaa kyselyn
                
                string polku = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\mikae\\OneDrive\\Tiedostot\\Tietokanta.mdf;Integrated Security=True;Connect Timeout=30"; // Korvaa oikealla tietokantayhteydellä
                using (SqlConnection kanta = new SqlConnection(polku))
                {
                    kanta.Open();
                    string sql = "SELECT * FROM tuotteet WHERE id = @tuoteId";
                    SqlCommand komento = new SqlCommand(sql, kanta);
                    komento.Parameters.AddWithValue("@tuoteId", tuoteId);

                    using (SqlDataReader lukija = komento.ExecuteReader())
                     
                    {
                        if (lukija.Read())
                        {
                            tuote_nimi.Text = lukija["nimi"].ToString();
                            tuote_hinta.Text = lukija["hinta"].ToString();
                            tuote_kuvaus.Text = lukija["kuvaus"].ToString();
                        }
                        kanta.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Tietojen lataaminen epäonnistui: " + ex.Message);
            }
        }

        private void Painike_tallenna_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Tässä voit käyttää tietokantayhteyttä ja suorittaa päivityksen
                // Esimerkiksi:
                string polku = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\mikae\\OneDrive\\Tiedostot\\Tietokanta.mdf;Integrated Security=True;Connect Timeout=30"; // Korvaa oikealla tietokantayhteydellä
                using (SqlConnection kanta = new SqlConnection(polku))
                {
                    kanta.Open();
                    string sql = "UPDATE tuotteet SET nimi = @nimi, hinta = @hinta, kuvaus = @kuvaus WHERE id = @tuoteId";
                    SqlCommand komento = new SqlCommand(sql, kanta);
                    komento.Parameters.AddWithValue("@nimi", tuote_nimi.Text);
                    komento.Parameters.AddWithValue("@hinta", tuote_hinta.Text);
                    komento.Parameters.AddWithValue("@kuvaus", tuote_kuvaus.Text);
                    komento.Parameters.AddWithValue("@tuoteId", tuoteId);

                    komento.ExecuteNonQuery();

                    kanta.Close();

                    MessageBox.Show("Tiedot tallennettu onnistuneesti", "Tallennus onnistui", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tietojen tallentaminen epäonnistui: " + ex.Message, "Tallennus epäonnistui", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}