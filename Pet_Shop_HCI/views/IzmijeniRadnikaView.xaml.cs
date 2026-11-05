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
    /// Interaction logic for IzmijeniRadnikaView.xaml
    /// </summary>
    public partial class IzmijeniRadnikaView : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        private bool _admin;
        public bool Admin
        {
            get => _admin;
            set { _admin = value; OnPropertyChanged(nameof(Admin)); }
        }

        private bool _status;
        public bool Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        private string _korisnicnoIme;
        public string KorisnickoIme
        {
            get => _korisnicnoIme;
            set { _korisnicnoIme = value; OnPropertyChanged(nameof(KorisnickoIme)); }
        }

        private string _lozinka;
        public string Lozinka
        {
            get => _lozinka;
            set { _lozinka = value; OnPropertyChanged(nameof(Lozinka)); }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public IzmijeniRadnikaView(string jmb)
        {
            InitializeComponent();
            Jmb = jmb;
            DataContext = this;
            UcitajRadnika();
        }

        private void UcitajRadnika()
        {
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand command = conn.CreateCommand();
                        command.CommandText = "SELECT Ime, Prezime, Admin, Status, KorisnickoIme FROM zaposleni WHERE JMB = @Jmb";
                        command.Parameters.AddWithValue("@Jmb", Jmb);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                Ime = reader["Ime"].ToString();
                                Prezime = reader["Prezime"].ToString();
                                Admin = Convert.ToInt32(reader["Admin"]) == 1; ;
                                Status = Convert.ToInt32(reader["Status"]) == 1; ;
                                KorisnickoIme = reader["KorisnickoIme"].ToString();
                            }
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

        private void ButtonSacuvajPromjene_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li želite sačuvati promjene?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                PregledRadnika pregledRadnika = new PregledRadnika();

                string jmb = JMBTxt.Text;
                string ime = ImeTxt.Text;
                string prezime = PrezimeTxt.Text;
                bool admin = AdministratorChk.IsChecked ?? false;
                bool status = StatusChk.IsChecked ?? false;

                int adminInt = admin ? 1 : 0;
                int statusInt = status ? 1 : 0;

                if (string.IsNullOrEmpty(jmb) || string.IsNullOrEmpty(ime) || string.IsNullOrEmpty(prezime))
                {
                    MessageBox.Show("Ispraznili ste polje koje mora biti popunjeno.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                pregledRadnika.SacuvajPromjeneRadnik(jmb, ime, prezime, adminInt, statusInt);
                DialogResult = true;
                this.Close();
            }
            else
            {
                return;
            }
            
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
