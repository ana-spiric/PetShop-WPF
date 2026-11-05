using MySql.Data.MySqlClient;
using PetShopHCI.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using Pet_Shop_HCI.views;

namespace Pet_Shop_HCI.classes
{
    public class PregledRadnika : ViewModelBase
    {
        private DataView radnici;

        public ICommand Izmijeni { get; set; }
        public ICommand Nalog { get; set; }
        public ICommand Search { get; set; }

        public DataView radniciView
        {
            get { return radnici; }
            set
            {
                radnici = value;
                onPropertyChanged(nameof(radniciView));
            }
        }

        public PregledRadnika()
        {
            Izmijeni = new RelayCommand(ExecuteIzmijeni);
            Nalog = new RelayCommand(ExecuteNalog);
            Search = new RelayCommand(ExecuteSearch);
            loadRadnici();
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; onPropertyChanged(nameof(SearchText)); ExecuteSearch(null); }
        }

        private void loadRadnici()
        {
            DataTable radnici = new DataTable();
            using(MySqlConnection conn = DBConnector.getConnection())
            {
                if(conn != null)
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT JMB, Ime, Prezime, Admin, Status, KorisnickoIme FROM zaposleni ORDER BY Status=1 DESC";
                        MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                        adapter.Fill(radnici);

                        radnici.Columns["JMB"].ColumnName = "Matični broj";
                        radnici.Columns["KorisnickoIme"].ColumnName = "Korisničko ime";

                        radniciView = radnici.DefaultView;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error fetching employees: {e.Message}");
                    }
                    finally
                    {
                        DBConnector.closeConnection(conn);
                    }
                }
            }
        }

        private void ExecuteSearch(object obj)
        {
            DataTable radnici = new DataTable();
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("PretraziRadnika", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@searchTerm", SearchText ?? string.Empty);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(radnici);
                        radnici.Columns["JMB"].ColumnName = "Matični broj";
                        radnici.Columns["KorisnickoIme"].ColumnName = "Korisničko ime";
                        radniciView = new DataView(radnici);
                    }
                    catch (Exception e)
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

        private string sha256hash(string password)
        {
            using (SHA256 hash = SHA256.Create())
            {
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringBuilder.Append(bytes[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        public void DodajRadnikaUBazu(string jmb, string ime, string prezime, int admin, int aktivan, string korisnickoIme, string lozinka)
        {
            using(MySqlConnection conn = DBConnector.getConnection())
            {
                if(conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("DodajRadnika", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("pJMB", jmb);
                        cmd.Parameters.AddWithValue("pIme", ime);
                        cmd.Parameters.AddWithValue("pPrezime", prezime);
                        cmd.Parameters.AddWithValue("pAdmin", admin);
                        cmd.Parameters.AddWithValue("pStatus", aktivan);
                        cmd.Parameters.AddWithValue("pKorisnickoIme", korisnickoIme);
                        cmd.Parameters.AddWithValue("pLozinka", sha256hash(lozinka));

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Novi zaposleni je uspješno sačuvan.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show($"Greška prilikom dodavanja radnika: {e.Message}");
                    }
                    finally
                    {
                        DBConnector.closeConnection(conn);
                    }
                }
            }
        }

        public bool ProvjeraJMB(string jmb)
        {
            bool postoji = false;
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT COUNT(*) FROM zaposleni WHERE JMB = @Jmb";
                        cmd.Parameters.AddWithValue("@Jmb", jmb);
                        int result = Convert.ToInt32(cmd.ExecuteScalar());
                        if (result > 0)
                            postoji = true;
                        else
                            postoji = false;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        DBConnector.closeConnection(conn);
                    }
                }
            }
            return postoji;
        }

        private void ExecuteIzmijeni(object obj)
        {
            var radnik = obj as DataRowView;
            if (radnik != null)
            {
                //var jmb = radnik["JMB"].ToString();
                var jmb = radnik["Matični broj"].ToString();
                var dialog = new IzmijeniRadnikaView(jmb);
                bool? result = dialog.ShowDialog();
                if (result == true)
                {
                    loadRadnici();
                }
            }
            else
            {
                Console.WriteLine("Radnik nije validan");
            }
        }

        private void ExecuteNalog(object obj)
        {
            var radnik = obj as DataRowView;
            if(radnik != null)
            {
                //var jmb = radnik["JMB"].ToString();
                var jmb = radnik["Matični broj"].ToString();
                var dialog = new NalogView(jmb);
                bool? result = dialog.ShowDialog();
                if (result == true)
                {
                    loadRadnici();
                }
            }
            else
            {
                Console.WriteLine("Radnik nije validan");
            }
        }

        public void SacuvajNovuLozinku(string lozinka, string jmb)
        {
            string otisak = sha256hash(lozinka);
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if(conn != null)
                {
                    try { 
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE zaposleni SET Lozinka = @Lozinka WHERE JMB = @jmb";
                        cmd.Parameters.AddWithValue("@Lozinka", otisak);
                        cmd.Parameters.AddWithValue("@jmb", jmb);
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

        public bool ProvjeraKorisnickogImena(string korisnickoIme)
        {
            bool postoji = false;
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try { 
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT COUNT(*) FROM zaposleni WHERE KorisnickoIme=@KorisnickoIme";
                        cmd.Parameters.AddWithValue("@KorisnickoIme", korisnickoIme);
                        int result = Convert.ToInt32(cmd.ExecuteScalar());
                        if (result > 0)
                            postoji = true;
                        else 
                            postoji = false;
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
            return postoji;
        }

        public void SacuvajPromjeneRadnik(string jmb, string ime, string prezime, int admin, int status)
        {
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("IzmijeniRadnika", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("p_JMB", jmb);
                        cmd.Parameters.AddWithValue("p_ime", ime);
                        cmd.Parameters.AddWithValue("p_prezime", prezime);
                        cmd.Parameters.AddWithValue("p_admin", admin);
                        cmd.Parameters.AddWithValue("p_status", status);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Podaci su uspješno izmjenjeni.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Greška prilikom izmjene podataka: {e.Message}");
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
