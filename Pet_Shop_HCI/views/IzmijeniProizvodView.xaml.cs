using MySql.Data.MySqlClient;
using Pet_Shop_HCI.classes;
using PetShopHCI.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pet_Shop_HCI.views
{
    /// <summary>
    /// Interaction logic for IzmijeniProizvodView.xaml
    /// </summary>
    /// 
    
    public partial class IzmijeniProizvodView : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private int _sifra;
        public int Sifra
        {
            get => _sifra;
            set { _sifra = value; OnPropertyChanged(nameof(Sifra)); }
        }

        private string _naziv;
        public string Naziv
        {
            get => _naziv;
            set { _naziv = value; OnPropertyChanged(nameof(Naziv)); }
        }

        private int _stanje;
        public int Stanje
        {
            get => _stanje;
            set { _stanje = value; OnPropertyChanged(nameof(Stanje)); }
        }

        private decimal _cijena;
        public decimal Cijena
        {
            get => _cijena;
            set { _cijena = value; OnPropertyChanged(nameof(Cijena)); }
        }

        private string _opis;
        public string Opis
        {
            get => _opis;
            set { _opis = value; OnPropertyChanged(nameof(Opis)); }
        }

        private string _tip;
        public string Tip
        {
            get => _tip;
            set { _tip = value; OnPropertyChanged(nameof(Tip)); }
        }

        //Ostalo - asesoar

        private string _vrsta;
        public string Vrsta
        {
            get => _vrsta;
            set { _vrsta = value; OnPropertyChanged(nameof(Vrsta)); }
        }

        private string _dodatniOpis;
        public string DodatniOpis
        {
            get => _dodatniOpis;
            set { _dodatniOpis = value; OnPropertyChanged(nameof(DodatniOpis)); }
        }

        //Ljubimac

        private string _vrstaZivotinje;
        public string VrstaZivotinje
        {
            get => _vrstaZivotinje;
            set { _vrstaZivotinje = value; OnPropertyChanged(nameof(VrstaZivotinje)); }
        }

        private string _rasa;
        public string Rasa
        {
            get => _rasa;
            set { _rasa = value; OnPropertyChanged(nameof(Rasa)); }
        }

        private decimal _uzrast;
        public decimal Uzrast
        {
            get => _uzrast;
            set { _uzrast = value; OnPropertyChanged(nameof(Uzrast)); }
        }

        private decimal _tezina;
        public decimal Tezina
        {
            get => _tezina;
            set { _tezina = value; OnPropertyChanged(nameof(Tezina)); }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public IzmijeniProizvodView(int sifra)
        {
            InitializeComponent();
            Sifra = sifra;
            DataContext = this;
            UcitajProizvod();
        }

        private void UcitajProizvod()
        {
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand command = conn.CreateCommand();
                        command.CommandText = "SELECT Naziv, Stanje, Cijena, Opis, Tip FROM proizvod WHERE Sifra = @Sifra";
                        command.Parameters.AddWithValue("@Sifra", Sifra);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                Naziv = reader["Naziv"].ToString();
                                Stanje = Convert.ToInt32(reader["Stanje"]);
                                Cijena = Convert.ToDecimal(reader["Cijena"]);
                                Opis = reader["Opis"].ToString();
                                Tip = reader["Tip"].ToString();
                                //MessageBox.Show($"Tip nakon učitavanja: {Tip}");
                            }
                        }
                        if (Tip == "Ostalo")
                        {
                            StackPanelOstalo.Visibility = Visibility.Visible;
                            UcitajProizvodOstalo(conn);
                        }
                        else if(Tip == "Ljubimac")
                        {
                            StackPanelLjubimac.Visibility = Visibility.Visible;
                            UcitajProizvodLjubimac(conn);
                        }
                        else
                        {
                            StackPanelOstalo.Visibility = Visibility.Collapsed;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        private void UcitajProizvodOstalo(MySqlConnection conn)
        {
            MySqlCommand commandOstalo = conn.CreateCommand();
            commandOstalo.CommandText = "SELECT Vrsta, DodatniOpis FROM ostalo WHERE PROIZVOD_Sifra = @Sifra";
            commandOstalo.Parameters.AddWithValue("@Sifra", Sifra);

            using (MySqlDataReader readerOstalo = commandOstalo.ExecuteReader())
            {
                if(readerOstalo.Read())
                {
                    Vrsta = readerOstalo["Vrsta"].ToString();
                    DodatniOpis = readerOstalo["DodatniOpis"].ToString();
                    this.DataContext = this;
                }
            }
        }

        private void UcitajProizvodLjubimac(MySqlConnection conn)
        {
            MySqlCommand commandLjubimac = conn.CreateCommand();
            commandLjubimac.CommandText = "SELECT VrstaZivotinje, Uzrast, Tezina, Rasa FROM Ljubimac WHERE PROIZVOD_Sifra = @Sifra";
            commandLjubimac.Parameters.AddWithValue("@Sifra", Sifra);

            using (MySqlDataReader readerLjubimac = commandLjubimac.ExecuteReader())
            {
                if (readerLjubimac.Read())
                {
                    VrstaZivotinje = readerLjubimac["VrstaZivotinje"].ToString();
                    Rasa = readerLjubimac["Rasa"].ToString();
                    Uzrast = Convert.ToDecimal(readerLjubimac["Uzrast"]);
                    Tezina = Convert.ToDecimal(readerLjubimac["Tezina"]);
                    this.DataContext = this;
                }
            }
        }

        private void ButtonSacuvajPromjene_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li želite sačuvati promjene?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                PregledProizvoda pregledProizvoda = new PregledProizvoda();
                string naziv = nazivTxt.Text;
                string cijena = cijenaTxt.Text;
                string stanje = stanjeTxt.Text;
                string opis = opisTxt.Text;
                if (Tip == "Ostalo")
                {
                    string vrsta = vrstaTxt.Text;
                    string dodatniOpis = dodatniOpisTxt.Text;

                    if (string.IsNullOrEmpty(naziv) || string.IsNullOrEmpty(cijena) || string.IsNullOrEmpty(stanje)
                        || string.IsNullOrEmpty(vrsta))
                    {
                        MessageBox.Show("Ispraznili ste polje koje mora biti popunjeno.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int stanjeInt = Convert.ToInt32(stanje);
                    decimal cijenaDecimal = Convert.ToDecimal(cijena);

                    pregledProizvoda.SacuvajPromjeneOstalo(Sifra, naziv, stanjeInt, cijenaDecimal, opis, vrsta, dodatniOpis);
                    this.DialogResult = true;
                    this.Close();
                }
                else if (Tip == "Ljubimac")
                {
                    string vrstaZivotinje = vrstaZivotinjeTxt.Text;
                    string uzrast = uzrastTxt.Text;
                    string tezina = tezinaTxt.Text;
                    string rasa = rasaTxt.Text;

                    if (string.IsNullOrEmpty(naziv) || string.IsNullOrEmpty(cijena) || string.IsNullOrEmpty(stanje)
                       || string.IsNullOrEmpty(uzrast) || string.IsNullOrEmpty(vrstaZivotinje))
                    {
                        MessageBox.Show("Ispraznili ste polje koje mora biti popunjeno.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int stanjeInt = Convert.ToInt32(stanje);
                    decimal cijenaDecimal = Convert.ToDecimal(cijena);
                    decimal uzrastDecimal = Convert.ToDecimal(uzrast);
                    decimal tezinaDecimal = Convert.ToDecimal(tezina);

                    pregledProizvoda.SacuvajPromjeneLjubimac(Sifra, naziv, stanjeInt, cijenaDecimal, opis, vrstaZivotinje, rasa, uzrastDecimal, tezinaDecimal);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Niste popunili obavezna polja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private static bool IsInteger(string currentText, string newText)
        {
            string fullText = currentText.Insert(currentText.Length, newText);
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(fullText);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInteger((sender as TextBox).Text, e.Text);
        }

        private static bool IsDecimal(string currentText, string newText)
        {
            string decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string fullText = currentText.Insert(currentText.Length, newText);
            Regex regex = new Regex(@"^\d*\" + decimalSeparator + @"?\d*$");
            return !regex.IsMatch(fullText);
        }

        private void TextBox_PreviewTextDecimal(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsDecimal((sender as TextBox).Text, e.Text);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
