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

        public TuoteMuokkausWindow(int tuoteId)
        {
            InitializeComponent();
            this.tuoteId = tuoteId;

            // Voit tehdä tarvittavat toimet, kuten ladata tietoja annetun tuoteId:n perusteella
            // ja päivittää sitten käyttöliittymän vastaavasti.
        }
        private void Painike_tallenna_Click(object sender, RoutedEventArgs e)
        {
            // Tässä voit lisätä koodin tallentamaan muutokset tietokantaan
            // Voit käyttää tuote_nimi.Text, tuote_hinta.Text ja tuote_kuvaus.Text saadaksesi käyttäjän syötteen
            // Toteuta logiikka tallentamiseksi tietokantaan
        }
    }
}