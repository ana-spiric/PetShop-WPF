using MySql.Data.MySqlClient;
using PetShopHCI.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pet_Shop_HCI.classes
{
    public class Prodaja : ViewModelBase
    {
        public ICommand Search { get; set; }
        public ICommand Dodaj { get; set; }
        public ICommand Ukloni { get; set; }
        public ICommand KreirajRacun { get; set; }

        //konstruktor
        public Prodaja()
        {
            StavkeRacuna = tabela();

            Search = new RelayCommand(ExecuteSearch);
            ExecuteSearch(null);

            Dodaj = new RelayCommand(ExecuteDodaj);
            Ukloni = new RelayCommand(ExecuteUkloni);
            KreirajRacun = new RelayCommand(ExecuteKreirajRacun);
        }

        //racun tabela
        private DataView _stavkeRacuna;
        public DataView StavkeRacuna
        {
            get { return _stavkeRacuna; }
            set { _stavkeRacuna = value; onPropertyChanged(nameof(StavkeRacuna)); }
        }

        private DataView tabela()
        {
            DataTable stavke = new DataTable();
            stavke.Columns.Add("Šifra", typeof(int));
            stavke.Columns.Add("Naziv", typeof(string));
            stavke.Columns.Add("Količina", typeof(int));
            stavke.Columns.Add("Cijena", typeof(decimal));
            DataView stavkeRacuna = new DataView(stavke);
           
            return stavkeRacuna;
        }

        //selektovani porizvod
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

        //ukupna cijena
        private string _ukupnaCijena;
        public string UkupnaCijena
        {
            get { return _ukupnaCijena; }
            set { _ukupnaCijena = value; onPropertyChanged(nameof(UkupnaCijena)); }
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
                        ProizvodiView = new DataView(proizvodi);
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

        private void ExecuteDodaj(object obj)
        {
            if(SelectedProduct != null && !string.IsNullOrEmpty(Kolicina))
            {
                if(Convert.ToInt32(Kolicina) <= Convert.ToInt32(SelectedProduct["Stanje"]))
                {
                    DataRowView newRow = StavkeRacuna.AddNew();
                    newRow["Šifra"] = SelectedProduct["Sifra"];
                    newRow["Naziv"] = SelectedProduct["Naziv"];
                    newRow["Količina"] = Convert.ToInt32(Kolicina);
                    newRow["Cijena"] = SelectedProduct["Cijena"];
                    newRow.EndEdit();

                    int staroStanje = Convert.ToInt32(SelectedProduct["Stanje"]);
                    SelectedProduct["Stanje"] = staroStanje - Convert.ToInt32(Kolicina);
                    IzracunajUkupnuCijenu();
                    SelectedProduct = null;
                    Kolicina = "";
                }
                else
                {
                    MessageBox.Show("Nema dovoljno proizvoda na stanju.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                return;
            }
        }

        private void IzracunajUkupnuCijenu()
        {
            double ukupnaCijena = 0;
            foreach (DataRowView row in StavkeRacuna)
                ukupnaCijena += Convert.ToDouble(row["Cijena"]) * Convert.ToDouble(row["Količina"]);
            UkupnaCijena = string.Format("{0:F2}", ukupnaCijena);
        }

        //GENERISANJE SIFRE 
        private static string SifraGenerator(int length = 10)
        {
            const string characters = "abcdefghijklmnopqrstuvwxyz0123456789"; // samo mala slova i brojevi
            StringBuilder result = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
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
                if(conn != null)
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT COUNT(*) FROM racun WHERE Sifra = @sifra";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@sifra", sifra);
                        broj = Convert.ToInt32(cmd.ExecuteScalar());
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
            return broj > 0;
        }

        private string SifraGen()
        {
            string sifra;
            do
            {
                sifra = SifraGenerator(10);
            } while (ProvjeraSifre(sifra));
            return sifra;
        }

        //UKLANJANJE PROIZVODA
        private void ExecuteUkloni(object obj)
        {
            if (SelectedProduct != null)
            {
                var sifra = SelectedProduct["Šifra"];
                int kolicina = Convert.ToInt32(SelectedProduct["Količina"]);

                foreach (DataRowView row in ProizvodiView)
                {
                    if (row["Sifra"].Equals(sifra))
                    {
                        int trenutnoStanje = Convert.ToInt32(row["Stanje"]);
                        row["Stanje"] = trenutnoStanje + kolicina;
                        break;
                    }
                }

                StavkeRacuna.Table.Rows.Remove(SelectedProduct.Row);
                IzracunajUkupnuCijenu();
                SelectedProduct = null;
            }
            else
                return;
        }

        private void ExecuteKreirajRacun(object obj)
        {
            if(StavkeRacuna != null && StavkeRacuna.Count == 0)
            { 
                return;
            }

            string sifraRacuna = SifraGen();
            DateTime datumKreiranja = DateTime.Now;

            MessageBoxResult result = MessageBox.Show("Da li želite da kreirate račun?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(result == MessageBoxResult.Yes) { 
                using (MySqlConnection conn = DBConnector.getConnection())
                {
                    if(conn != null)
                    {
                        try
                        {
                            conn.Open();
                            MySqlCommand cmd = new MySqlCommand("DodajRacun", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("pSifra", sifraRacuna);
                            cmd.Parameters.AddWithValue("pDatumKreiranja", datumKreiranja);
                            cmd.Parameters.AddWithValue("pUkupnaCijena", UkupnaCijena);
                            cmd.Parameters.AddWithValue("pZaposleniJMB", Sesija.JMB);
                            int uspjesnoKreiranRacun = cmd.ExecuteNonQuery();

                            if(uspjesnoKreiranRacun > 0)
                            {
                                MessageBox.Show("Račun uspješno kreiran.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                                foreach (DataRowView stavka in StavkeRacuna)
                                {
                                    int sifraStavka = Convert.ToInt32(stavka["Šifra"]);
                                    string upit = "INSERT INTO stavka (PROIZVOD_Sifra, RACUN_Sifra, NazivProizvoda, KolicinaProizvoda, ProdajnaCijena) VALUES (@pSifra, @rSifra, @naziv, @kolicina, @cijena)";
                                    MySqlCommand cmd2 = new MySqlCommand(upit, conn);
                                    cmd2.Parameters.AddWithValue("@pSifra", sifraStavka);
                                    cmd2.Parameters.AddWithValue("@rSifra", sifraRacuna);
                                    cmd2.Parameters.AddWithValue("@naziv", stavka["Naziv"].ToString());
                                    cmd2.Parameters.AddWithValue("@kolicina", Convert.ToInt32(stavka["Količina"]));
                                    cmd2.Parameters.AddWithValue("@cijena", Convert.ToDecimal(stavka["Cijena"]));
                                    cmd2.ExecuteNonQuery();

                                    foreach(DataRowView p in ProizvodiView)
                                    {
                                        int proizvodSifra = Convert.ToInt32(p["Sifra"]);
                                        
                                        if (proizvodSifra == sifraStavka)
                                        {
                                            int updateStanje = Convert.ToInt32(p["Stanje"]);
                                            
                                            string query = "UPDATE proizvod SET Stanje = @updateStanje  WHERE Sifra = @proizvodSifra";
                                            MySqlCommand cmd1 = new MySqlCommand(query, conn);
                                            cmd1.Parameters.AddWithValue("@updateStanje", updateStanje);
                                            cmd1.Parameters.AddWithValue("@proizvodSifra", proizvodSifra);
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }
                                }

                                ExecuteSearch(null);
                                StavkeRacuna = tabela();
                                UkupnaCijena = null;
                                Kolicina = null;
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
        }
    }
}
