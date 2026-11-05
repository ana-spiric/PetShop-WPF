using MySql.Data.MySqlClient;
using PetShopHCI.classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pet_Shop_HCI.classes
{
    public class Narudzba : ViewModelBase
    {
        public ICommand Search { get; set; }
        public ICommand Dodaj { get; set; }
        public ICommand Ukloni { get; set; }
        public ICommand KreirajNarudzbu { get; set; }

        public Narudzba()
        {
            loadDobavljaci();
            StavkeNarudzbe = tabela();
            Search = new RelayCommand(ExecuteSearch);
            ExecuteSearch(null);

            Dodaj = new RelayCommand(ExecuteDodaj);
            Ukloni = new RelayCommand(ExecuteUkloni);
            KreirajNarudzbu = new RelayCommand(ExecuteKreirajNarudzbu);
        }

        private DataRowView _selectedProduct;
        public DataRowView SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; onPropertyChanged(nameof(SelectedProduct)); Kolicina = "1"; }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; onPropertyChanged(nameof(SearchText)); ExecuteSearch(null); }
        }

        private DataView _proizvodiView;
        public DataView ProizvodiView
        {
            get { return _proizvodiView; }
            set { _proizvodiView = value; onPropertyChanged(nameof(ProizvodiView)); }
        }

        private string _kolicina;
        public string Kolicina
        {
            get { return _kolicina; }
            set { _kolicina = value; onPropertyChanged(nameof(Kolicina)); }
        }

        private string _jib;
        public string Jib
        {
            get { return _jib; }
            set { _jib = value; onPropertyChanged(nameof(Jib)); }
        }

        private string _nazivDobavljaca;
        public string NazivDobavljaca
        {
            get { return _nazivDobavljaca; }
            set { _nazivDobavljaca = value; onPropertyChanged(nameof(NazivDobavljaca)); }
        }

        private string _selectedDobavljac;
        public string SelectedDobavljac
        {
            get { return _selectedDobavljac; }
            set { _selectedDobavljac = value; onPropertyChanged(nameof(SelectedDobavljac)); ParseSelectedDobavljac(); }
        }

        private ObservableCollection<string> _dobavljaci;
        public ObservableCollection<string> Dobavljaci
        {
            get { return _dobavljaci; }
            set { _dobavljaci = value; onPropertyChanged(nameof(Dobavljaci)); }
        }

        private DataView _stavkeNarudzbe;
        public DataView StavkeNarudzbe
        {
            get { return _stavkeNarudzbe; }
            set { _stavkeNarudzbe = value; onPropertyChanged(nameof(StavkeNarudzbe)); }
        }

        private string _ukupnaCijena;
        public string UkupnaCijena
        {
            get { return _ukupnaCijena; }
            set { _ukupnaCijena = value; onPropertyChanged(nameof(UkupnaCijena)); }
        }

        private DataView tabela()
        {
            DataTable stavke = new DataTable();
            stavke.Columns.Add("Šifra", typeof(int));
            stavke.Columns.Add("Naziv", typeof(string));
            stavke.Columns.Add("Količina", typeof(int));
            stavke.Columns.Add("Cijena", typeof(decimal));
            DataView stavkeNarudzbe = new DataView(stavke);

            return stavkeNarudzbe;
        }
        private void ExecuteSearch(object obj)
        {
            DataTable proizvodi = new DataTable();
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("PretraziProizvod", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@searchTerm", SearchText ?? string.Empty);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(proizvodi);
                        proizvodi.Columns.Remove("Stanje");
                        proizvodi.Columns["Sifra"].ColumnName = "Šifra";
                        ProizvodiView = new DataView(proizvodi);
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

        private void ExecuteDodaj(object obj)
        {
            if (SelectedProduct != null && !string.IsNullOrEmpty(Kolicina))
            {
                
                DataRowView newRow = StavkeNarudzbe.AddNew();
                newRow["Šifra"] = SelectedProduct["Šifra"];
                newRow["Naziv"] = SelectedProduct["Naziv"];
                newRow["Količina"] = Convert.ToInt32(Kolicina);
                newRow["Cijena"] = SelectedProduct["Cijena"];
                newRow.EndEdit();

                IzracunajUkupnuCijenu();
                SelectedProduct = null;
                Kolicina = "";
               
            }
            else
            {
                return;
            }
        }

        private void ExecuteUkloni(object obj)
        {
            if (SelectedProduct == null)
            {
                return;
            }
            StavkeNarudzbe.Table.Rows.Remove(SelectedProduct.Row);
            IzracunajUkupnuCijenu();
            SelectedProduct = null;
        }

        private void ExecuteKreirajNarudzbu(object obj)
        {
            if(StavkeNarudzbe != null && StavkeNarudzbe.Count == 0)
            {
                return;
            }

            if(SelectedDobavljac == null)
            {
                MessageBox.Show("Obavezno je izabrati dobavljača.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string sifraNarudzbe = SifraGen();
            DateTime datumKreiranja = DateTime.Now;

            MessageBoxResult result = MessageBox.Show("Da li želite da kreirate narudžbu?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if(result == MessageBoxResult.Yes)
            {
                using (MySqlConnection conn = DBConnector.getConnection())
                {
                    if(conn != null)
                    {
                        try
                        {
                            conn.Open();
                            MySqlCommand cmd = new MySqlCommand("DodajNabavku", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("pSifra", sifraNarudzbe);
                            cmd.Parameters.AddWithValue("pDatumKreiranja", datumKreiranja);
                            cmd.Parameters.AddWithValue("pJMB", Sesija.JMB);
                            cmd.Parameters.AddWithValue("pJIB", Jib);
                            cmd.Parameters.AddWithValue("pUkupnaCijena", UkupnaCijena);
                            int uspjesnoKreiranaNarudzba = cmd.ExecuteNonQuery();

                            if(uspjesnoKreiranaNarudzba > 0)
                            {
                                foreach(DataRowView stavka in StavkeNarudzbe)
                                {
                                    string query = "INSERT INTO proizvod_nabavka (PROIZVOD_Sifra, NABAVKA_SifraNabavke, NabavnaCijena, Kolicina) VALUES (@prSifra, @nSifra, @cijena, @kolicina)";
                                    MySqlCommand cmd1 = new MySqlCommand(query, conn);
                                    cmd1.Parameters.AddWithValue("@prSifra", Convert.ToInt32(stavka["Šifra"]));
                                    cmd1.Parameters.AddWithValue("@nSifra", sifraNarudzbe);
                                    cmd1.Parameters.AddWithValue("@cijena", Convert.ToDecimal(stavka["Cijena"]));
                                    cmd1.Parameters.AddWithValue("@kolicina", Convert.ToInt32(stavka["Količina"]));
                                    cmd1.ExecuteNonQuery();
                                }

                                MessageBox.Show("Narudžba uspješno kreirana.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);

                                ExecuteSearch(null);
                                StavkeNarudzbe = tabela();
                                UkupnaCijena = null;
                                Kolicina = null;
                                SelectedDobavljac = null;
                            }
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
        }

        private static string SifraGenerator(int length = 10)
        {
            const string characters = "0123456789";
            StringBuilder result = new StringBuilder(length);
            Random random = new Random();
            for(int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);
                result.Append(characters[index]);
            }
            return result.ToString();
        }

        private bool ProvjeraSifre(string sifra)
        {
            int broj = 0;
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT COUNT(*) FROM nabavka WHERE SifraNabavke = @sifra";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@sifra", sifra);
                        broj = Convert.ToInt32(cmd.ExecuteScalar());
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
            return broj > 0;
        }

        private string SifraGen()
        {
            string sifra;
            do
            {
                sifra = SifraGenerator(9);
            } while (ProvjeraSifre(sifra));
            return sifra;
        }

        private void IzracunajUkupnuCijenu()
        {
            double ukupnaCijena = 0;
            foreach (DataRowView row in StavkeNarudzbe)
                ukupnaCijena += Convert.ToDouble(row["Cijena"]) * Convert.ToDouble(row["Količina"]);
            UkupnaCijena = string.Format("{0:F2}", ukupnaCijena);
        }

        public void loadDobavljaci()
        {
            Dobavljaci = new ObservableCollection<string>();
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT JIB, Naziv FROM dobavljac";
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string dobavljac = $"{reader["Naziv"]} ({reader["JIB"]})";
                                Dobavljaci.Add(dobavljac);
                            }
                        }
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

        private void ParseSelectedDobavljac()
        {
            if (!string.IsNullOrEmpty(SelectedDobavljac))
            {
                string[] parts = SelectedDobavljac.Split('(');

                if (parts.Length == 2)
                {
                    NazivDobavljaca = parts[0].Trim();
                    Jib = parts[1].Replace(")", "").Trim();
                }
                else
                {
                    NazivDobavljaca = string.Empty;
                    Jib = string.Empty;
                }
            }
            else
            {
                NazivDobavljaca = string.Empty;
                Jib = string.Empty;
            }
        }

        public bool ProvjeraJIB(string jib)
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
                        cmd.CommandText = "SELECT COUNT(*) FROM dobavljac WHERE JIB = @Jib";
                        cmd.Parameters.AddWithValue("@Jib", jib);
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

        public void DodajDobavljaca(string jib, string naziv, string adresa)
        {
            using (MySqlConnection conn = DBConnector.getConnection()) 
            {
                if(conn != null)
                {
                    try
                    {
                        conn.Open();
                        string query = "INSERT INTO dobavljac (JIB, Naziv, Adresa) VALUES (@jib, @naziv, @adresa)";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@jib", jib);
                        cmd.Parameters.AddWithValue("@naziv", naziv);
                        cmd.Parameters.AddWithValue("@adresa", adresa);
                        cmd.ExecuteNonQuery();
                        loadDobavljaci();
                        MessageBox.Show("Novi dobavljač je uspješno sačuvan.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
