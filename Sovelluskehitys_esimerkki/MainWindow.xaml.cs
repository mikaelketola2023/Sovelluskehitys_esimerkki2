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
        private string solun_arvo;
        string polku = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\mikae\\OneDrive\\Tiedostot\\Tietokanta.mdf;Integrated Security=True;Connect Timeout=30";
        public MainWindow()
        {
            InitializeComponent();

            PaivitaComboBox();
            PaivitaAsiakasComboBox();

            PaivitaDataGrid("SELECT * FROM tuotteet", "tuotteet", tuote_lista);
            PaivitaDataGrid("SELECT * FROM asiakkaat", "asiakkaat", asiakas_lista);
            PaivitaDataGrid("SELECT ti.id AS id, a.nimi AS asiakas, tu.nimi AS tuote, ti.toimitettu AS toimitettu  FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='0'", "tilaukset", tilaukset_lista);
            PaivitaDataGrid("SELECT ti.id AS id, a.nimi AS asiakas, tu.nimi AS tuote, ti.toimitettu AS toimitettu  FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='1'", "toimitetut", toimitetut_lista);
        }
        private void Painike_hae_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PaivitaDataGrid("SELECT * FROM tuotteet", "tuotteet", tuote_lista);
            }
            catch 
            {
                tilaviesti.Text = "Tietojen haku epäonnistui";
            }
        }
        private void Painike_muokkaa_tuote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)tuote_lista.SelectedItem;
                if (selectedRow != null)
                {
                    int tuoteId = Convert.ToInt32(selectedRow["id"]);

                    // Voit avata uuden ikkunan tai käyttää nykyistä ikkunaa tuotteen muokkaamiseen.
                    // Tässä esimerkissä käytetään uutta ikkunaa (TuoteMuokkausWindow).

                    TuoteMuokkausWindow tuoteMuokkausIkkuna = new TuoteMuokkausWindow(tuoteId);
                    tuoteMuokkausIkkuna.ShowDialog();

                    // Voit päivittää DataGridin, kun ikkuna on suljettu (jos päivitys on tarpeen).
                    PaivitaDataGrid("SELECT * FROM tuotteet", "tuotteet", tuote_lista);
                    tilaviesti.Text = "Tuotteen muokkaus onnistui";
                }
                else
                {
                    tilaviesti.Text = "Valitse tuote, jonka haluat muokata.";
                }
            }
            catch (Exception ex)
            {
                tilaviesti.Text = "Tuotteen muokkaus ei onnistunut: " + ex.Message;
            }
        }

        private void Painike_lisaa_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection kanta = new SqlConnection(polku);
            kanta.Open();

            string sql = "INSERT INTO tuotteet (nimi, hinta, kuvaus) VALUES ('" + tuote_nimi.Text + "','" + tuote_hinta.Text + "','" + tuote_kuvaus.Text + "')";

            SqlCommand komento = new SqlCommand(sql, kanta);
            komento.ExecuteNonQuery();

            kanta.Close();

            PaivitaDataGrid("SELECT * FROM tuotteet", "tuotteet", tuote_lista);
            PaivitaComboBox();
        }

        private void PaivitaDataGrid(string kysely, string taulu, DataGrid grid)
        {
            SqlConnection kanta = new SqlConnection(polku);
            kanta.Open();

            /*tehdään sql komento*/
            SqlCommand komento = kanta.CreateCommand();
            komento.CommandText = kysely; // kysely

            /*tehdään data adapteri ja taulu johon tiedot haetaan*/
            SqlDataAdapter adapteri = new SqlDataAdapter(komento);
            DataTable dt = new DataTable(taulu);
            adapteri.Fill(dt);

            /*sijoitetaan data-taulun tiedot DataGridiin*/
            grid.ItemsSource = dt.DefaultView;

            kanta.Close();
        }

        private void PaivitaComboBox()
        {
            SqlConnection kanta = new SqlConnection(polku);
            kanta.Open();

            SqlCommand komento = new SqlCommand("SELECT * FROM tuotteet", kanta);
            SqlDataReader lukija = komento.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("TUOTE", typeof(string));

            combo_tuotteet.ItemsSource = dt.DefaultView;
            combo_tuotteet.DisplayMemberPath = "TUOTE";
            combo_tuotteet.SelectedValuePath = "ID";

            combo_tuotteet_2.ItemsSource = dt.DefaultView;
            combo_tuotteet_2.DisplayMemberPath = "TUOTE";
            combo_tuotteet_2.SelectedValuePath = "ID";

            while (lukija.Read()) 
            {
                int id = lukija.GetInt32(0);
                string tuote = lukija.GetString(1);
                dt.Rows.Add(id, tuote);
            }

            lukija.Close();
            kanta.Close();
        }
        private void PaivitaAsiakasComboBox()
        {
            SqlConnection kanta = new SqlConnection(polku);
            kanta.Open();

            SqlCommand komento = new SqlCommand("SELECT * FROM asiakkaat", kanta);
            SqlDataReader lukija = komento.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NIMI", typeof(string));

            combo_asiakkaat.ItemsSource = dt.DefaultView;
            combo_asiakkaat.DisplayMemberPath = "NIMI";
            combo_asiakkaat.SelectedValuePath = "ID";

            while (lukija.Read())
            {
                int id = lukija.GetInt32(0);
                string nimi = lukija.GetString(1);
                dt.Rows.Add(id, nimi);
            }

            lukija.Close();
            kanta.Close();
        }

            private void Painike_poista_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection kanta = new SqlConnection(polku);
            kanta.Open();

            string id = combo_tuotteet.SelectedValue.ToString();
            SqlCommand komento = new SqlCommand("DELETE FROM tuotteet WHERE id =" + id + ";", kanta);
            komento.ExecuteNonQuery();
            kanta.Close() ;

            PaivitaDataGrid("SELECT * FROM tuotteet", "tuotteet", tuote_lista);
            PaivitaComboBox();
        }

        private string alkuperaisen_arvon_teksti = "";

        private void tuote_lista_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Column is DataGridTextColumn)
            {
                var solu = ((DataRowView)e.Row.DataContext).Row.ItemArray[e.Column.DisplayIndex];
                alkuperaisen_arvon_teksti = solu.ToString();
            }
        }

        private void tuote_lista_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                int sarake = tuote_lista.CurrentCell.Column.DisplayIndex;
                string uusi_arvo = ((TextBox)e.EditingElement).Text;

                int tuote_id = int.Parse((e.Row.Item as DataRowView).Row[0].ToString());

                if (alkuperaisen_arvon_teksti != uusi_arvo)
                {
                    string kysely = "";
                    string kanta_sarake = "";
                    SqlConnection kanta = new SqlConnection(polku);
                    kanta.Open();

                    if (sarake == 1) kanta_sarake = "nimi";
                    else if (sarake == 2) kanta_sarake = "hinta";

                    kysely = "UPDATE tuotteet SET " + kanta_sarake + "='" + uusi_arvo + "' WHERE id=" + tuote_id;

                    SqlCommand komento = new SqlCommand(kysely, kanta);
                    komento.ExecuteNonQuery();

                    kanta.Close();

                    tilaviesti.Text = "Uusi arvo: " + uusi_arvo;

                    PaivitaComboBox();
                }
                else
                {
                    tilaviesti.Text = "Arvo ei muuttunut";
                }
            }
            catch
            {
                tilaviesti.Text = "Muokkaus ei onnistunut";
            }
        }



        private void Painike_asiakas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection kanta = new SqlConnection(polku);
                kanta.Open();

                string sql = "INSERT INTO asiakkaat (nimi, puhelinnumero, email) VALUES ('" + asiakas_nimi.Text + "','" + asiakas_puhelin.Text + "','" + asiakas_email.Text + "')";

                SqlCommand komento = new SqlCommand(sql, kanta);
                komento.ExecuteNonQuery();
                kanta.Close();

                PaivitaDataGrid("SELECT * FROM asiakkaat", "asiakkaat", asiakas_lista);
                tilaviesti.Text = "Asiakkaan lisääminen onnistui";
            }
            catch
            {
                tilaviesti.Text = "Asiakkaan lisääminen ei onnistunut";
            }
        }
        private void Asiakas_lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (asiakas_lista.SelectedItem != null)
            {
                DataRowView row = (DataRowView)asiakas_lista.SelectedItem;
                string nimi = row["nimi"].ToString();
                string puhelin = row["puhelinnumero"].ToString();
                string email = row["email"].ToString();

                // Näytä tiedot tekstikentissä
                asiakas_nimi.Text = nimi;
                asiakas_puhelin.Text = puhelin;
                asiakas_email.Text = email;
            }
        }
        private void Painike_muokkaa_asiakas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection kanta = new SqlConnection(polku);
                kanta.Open();

                DataRowView selectedRow = (DataRowView)asiakas_lista.SelectedItem;
                if (selectedRow != null)
                {
                    int asiakasId = Convert.ToInt32(selectedRow["id"]);

                    string sql = "UPDATE asiakkaat SET nimi=@nimi, puhelinnumero=@puhelin, email=@email WHERE id=@id";

                    SqlCommand komento = new SqlCommand(sql, kanta);
                    komento.Parameters.AddWithValue("@nimi", asiakas_nimi.Text);
                    komento.Parameters.AddWithValue("@puhelin", asiakas_puhelin.Text);
                    komento.Parameters.AddWithValue("@email", asiakas_email.Text);
                    komento.Parameters.AddWithValue("@id", asiakasId);

                    komento.ExecuteNonQuery();
                    kanta.Close();

                    PaivitaDataGrid("SELECT * FROM asiakkaat", "asiakkaat", asiakas_lista);
                    tilaviesti.Text = "Asiakkaan muokkaus onnistui";
                }
                else
                {
                    tilaviesti.Text = "Valitse asiakas, jota haluat muokata.";
                }
            }
            catch (Exception ex)
            {
                tilaviesti.Text = "Asiakkaan muokkaus ei onnistunut: " + ex.Message;
            }
        }
        private void Painike_poista_asiakas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)asiakas_lista.SelectedItem;
                if (selectedRow != null)
                {
                    int asiakasId = Convert.ToInt32(selectedRow["id"]);

                    SqlConnection kanta = new SqlConnection(polku);
                    kanta.Open();

                    string sql = "DELETE FROM asiakkaat WHERE id=@id";

                    SqlCommand komento = new SqlCommand(sql, kanta);
                    komento.Parameters.AddWithValue("@id", asiakasId);

                    komento.ExecuteNonQuery();
                    kanta.Close();

                    PaivitaDataGrid("SELECT * FROM asiakkaat", "asiakkaat", asiakas_lista);
                    tilaviesti.Text = "Asiakkaan poistaminen onnistui";
                }
                else
                {
                    tilaviesti.Text = "Valitse asiakas, jonka haluat poistaa.";
                }
            }
            catch (Exception ex)
            {
                tilaviesti.Text = "Asiakkaan poistaminen ei onnistunut: " + ex.Message;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            SqlConnection kanta = new SqlConnection(polku);
            kanta.Open();

            string asiakasID = combo_asiakkaat.SelectedValue.ToString();
            string tuoteID = combo_tuotteet_2.SelectedValue.ToString();

            string sql = "INSERT INTO tilaukset (asiakas_id, tuote_id, paivays) VALUES ('" + asiakasID + "', '" + tuoteID + "', GETDATE())"; 

            SqlCommand komento = new SqlCommand(sql, kanta);
            komento.ExecuteNonQuery();
            kanta.Close();

            PaivitaDataGrid("SELECT ti.id AS id, a.nimi AS asiakas, tu.nimi AS tuote, ti.toimitettu AS toimitettu, ti.paivays AS tilaus_paivays FROM tilaukset ti JOIN asiakkaat a ON a.id = ti.asiakas_id JOIN tuotteet tu ON tu.id = ti.tuote_id AND ti.toimitettu='0'", "tilaukset", tilaukset_lista);
            

        }

        private void Painike_toimita_Click(object sender, RoutedEventArgs e)
        {
            DataRowView rivinakyma = (DataRowView)((Button)e.Source).DataContext;
            String tilaus_id = rivinakyma[0].ToString();

            SqlConnection kanta = new SqlConnection(polku);
            kanta.Open();

            string sql = "UPDATE tilaukset SET toimitettu=1 WHERE id='" + tilaus_id + "';";

            SqlCommand komento = new SqlCommand(sql, kanta);
            komento.ExecuteNonQuery();
            kanta.Close();

            PaivitaDataGrid("SELECT ti.id AS id, a.nimi AS asiakas, tu.nimi AS tuote, ti.toimitettu AS toimitettu  FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='0'", "tilaukset", tilaukset_lista);
            PaivitaDataGrid("SELECT ti.id AS id, a.nimi AS asiakas, tu.nimi AS tuote, ti.toimitettu AS toimitettu  FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id AND ti.toimitettu='1'", "toimitetut", toimitetut_lista);
        }
        private void AvaaTietojaIkkuna(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tämän ohjelman tehnyt Mikael Ketola vuonna 2023");
        }

        private void Painike_hae_kaikki_tilaukset(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection kanta = new SqlConnection(polku);
                kanta.Open();

                Console.WriteLine("Ennen paivitaDataGrid-metodia");

                PaivitaDataGrid("SELECT ti.id AS id, a.nimi AS asiakas, tu.nimi AS tuote, ti.toimitettu AS toimitettu FROM tilaukset ti, asiakkaat a, tuotteet tu WHERE a.id=ti.asiakas_id AND tu.id=ti.tuote_id", "tilaustiedot", tilaustiedot);

                Console.WriteLine("Jälkeen paivitaDataGrid-metodia");

                MessageBox.Show("Haku tehty onnistuneesti", "Onnistunut haku", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                tilaviesti.Text = "Tietojen haku epäonnistui" + ex.Message;
            }
        }
    }
}
