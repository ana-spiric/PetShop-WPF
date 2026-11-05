using MySql.Data.MySqlClient;
using Pet_Shop_HCI.classes;
using PetShopHCI.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Pet_Shop_HCI.views
{
    /// <summary>
    /// Interaction logic for NalogView.xaml
    /// </summary>
    public partial class NalogView : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static string staroIme;

        private string _jmb;
        public string Jmb
        {
            get => _jmb;
            set { _jmb = value; OnPropertyChanged(nameof(Jmb)); }
        }

        private string _ime;
        public string Ime
        {
            get => _ime;
            set { _ime = value; OnPropertyChanged(nameof(Ime)); }
        }

        private string _prezime;
        public string Prezime
        {
            get => _prezime;
            set { _prezime = value; OnPropertyChanged(nameof(Prezime)); }
        }

        private string _korisnicnoIme;
        public string KorisnickoIme
        {
            get => _korisnicnoIme;
            set { _korisnicnoIme = value; OnPropertyChanged(nameof(KorisnickoIme)); }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public NalogView(string jmb)
        {
            InitializeComponent();
            Jmb = jmb;
            DataContext = this;
            DohvatiNalog();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DohvatiNalog()
        {
            using(MySqlConnection conn = DBConnector.getConnection())
            {
                if(conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT Ime, Prezime, KorisnickoIme FROM zaposleni WHERE JMB = @Jmb";
                        cmd.Parameters.AddWithValue("@Jmb", Jmb);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                KorisnickoIme = reader["KorisnickoIme"].ToString();
                                Ime = reader["Ime"].ToString();
                                Prezime = reader["Prezime"].ToString();
                                staroIme = KorisnickoIme;
                            }
                        }

                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        DBConnector.closeConnection(conn);
                    }
                }
            }
        }


        private void ButtonSacuvajPromjene_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li želite sačuvati promjene?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) 
            {
                PregledRadnika pregled = new PregledRadnika();
                string korisnickoIme = KImeTxt.Text;
                string lozinka = LozinkaTxt.Text;



                if (!string.IsNullOrWhiteSpace(lozinka))
                {
                    if (!korisnickoIme.Equals(staroIme))
                    {
                        if (pregled.ProvjeraKorisnickogImena(korisnickoIme))
                        {
                            MessageBox.Show("Korisničko ime već postoji.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            SacuvajKorisnickoIme(korisnickoIme);
                        }
                    }
                    pregled.SacuvajNovuLozinku(lozinka, Jmb);
                    MessageBox.Show("Podaci su uspješno izmjenjeni.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else if (!korisnickoIme.Equals(staroIme) && string.IsNullOrWhiteSpace(lozinka))
                {
                    SacuvajKorisnickoIme(korisnickoIme);
                    MessageBox.Show("Korisničko ime promjenjeno.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Nema nikakvih promjena.", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                DialogResult = true;
                this.Close();
            }
            else
            {
                return;
            }
            
        }

        private void SacuvajKorisnickoIme(string korisnickoIme)
        {
            using(MySqlConnection conn = DBConnector.getConnection())
            {
                if(conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE zaposleni SET KorisnickoIme=@KorisnickoIme WHERE JMB=@Jmb";
                        cmd.Parameters.AddWithValue("@KorisnickoIme", korisnickoIme);
                        cmd.Parameters.AddWithValue("@Jmb", Jmb);
                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        DBConnector.closeConnection(conn);
                    }
                }
            }
        }

    }
}
