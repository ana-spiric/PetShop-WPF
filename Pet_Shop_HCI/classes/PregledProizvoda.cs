using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using MySql.Data.MySqlClient;
using PetShopHCI.classes;
using System.Windows.Input;
using Pet_Shop_HCI.views;
using System.Windows.Controls;

namespace Pet_Shop_HCI.classes
{
    public class PregledProizvoda : ViewModelBase
    {
        private DataView proizvodi;
        public ICommand Izmijeni { get; set; }
        public ICommand Ukloni { get; set; }
        public ICommand Search { get; set; }
        public DataView proizvodiView
        {
            get { return proizvodi; }
            set
            {
                proizvodi = value;
                onPropertyChanged(nameof(proizvodiView));
                onPropertyChanged(nameof(IsEmpty));
            }
        }

        private bool _isEmpty;
        public bool IsEmpty
        {
            get { return _isEmpty; }
            set
            {
                _isEmpty = value;
                onPropertyChanged(nameof(IsEmpty));
            }
        }

        private string _tipProizvoda;
        public string TipProizvoda
        {
            get { return _tipProizvoda; }
            set
            {
                _tipProizvoda = value;
                onPropertyChanged(nameof(TipProizvoda));
            }

        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; onPropertyChanged(nameof(SearchText)); ExecuteSearch(null); }
        }

        public PregledProizvoda()
        {
            Izmijeni = new RelayCommand(ExecuteIzmijeni);
            Ukloni = new RelayCommand(ExecuteUkloni);
            Search = new RelayCommand(ExecuteSearch);
            loadSviProizvodi();
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
                        proizvodiView = new DataView(proizvodi);
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

        private void ExecuteIzmijeni(object obj)
        {
            var proizvod = obj as DataRowView;
            if (proizvod != null)
            {
                var sifra = Convert.ToInt32(proizvod["Šifra"]);
                var dialog = new IzmijeniProizvodView(sifra);
                bool? result = dialog.ShowDialog();
                if (result == true)
                {
                    if (TipProizvoda == "Asesoar")
                    {
                        loadAsesoar();
                    }
                    else if (TipProizvoda == "Svi")
                    {
                        loadSviProizvodi();
                    }
                    else if (TipProizvoda == "Ljubimci")
                    {
                        loadLjubimci();
                    }
                }
            }
            else
            {
                Console.WriteLine("Proizvod nije validan");
            }
        }

        private void ExecuteUkloni(object obj)
        {
            var proizvod = obj as DataRowView;
            if (proizvod != null)
            {
                var sifra = Convert.ToInt32(proizvod["Šifra"]);
                var naziv = proizvod["Naziv"].ToString();
                UkloniProizvod(sifra, naziv);
                if (TipProizvoda == "Asesoar")
                {
                    loadAsesoar();
                }
                else if (TipProizvoda == "Svi")
                {
                    loadSviProizvodi();
                }
                else if (TipProizvoda == "Ljubimci")
                {
                    loadLjubimci();
                }
                else
                {
                    loadSviProizvodi();
                }
            }
            else
            {
                Console.WriteLine("Proizvod nije validan");
            }
        }

        private void UkloniProizvod(int sifra, string naziv)
        {
            MessageBoxResult result = MessageBox.Show($"Da li želite da uklonite proizvod ({sifra}, {naziv})?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {

                using (MySqlConnection conn = DBConnector.getConnection())
                {
                    if (conn != null)
                    {
                        try
                        {
                            conn.Open();
                            string query = "UPDATE proizvod SET Postoji = 0 WHERE Sifra = @sifra";
                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@sifra", sifra);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Proizvod je uspješno uklonjen.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show($"Greška prilikom uklanjanja proizvoda: {e.Message}");
                        }
                        finally
                        {
                            DBConnector.closeConnection(conn);
                        }
                    }
                }
            }
        }

        public void loadSviProizvodi()
        {
            DataTable proizvodi = new DataTable();
            using(MySqlConnection conn = DBConnector.getConnection())
            {
                if(conn != null)
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT Sifra, Naziv, Opis, Stanje, Cijena FROM proizvod WHERE Postoji=1";
                        MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                        adapter.Fill(proizvodi);

                        proizvodi.Columns["Sifra"].ColumnName = "Šifra";
                        proizvodi.Columns["Cijena"].ColumnName = "Cijena (KM)";

                        proizvodiView = proizvodi.DefaultView;
                        IsEmpty = proizvodi.Rows.Count == 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error fetching products: {e.Message}");
                    }
                    finally
                    {
                        DBConnector.closeConnection(conn);
                    }
                }
            }
        }

        public void loadAsesoar()
        {
            DataTable proizvodi = new DataTable();
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        string query = @"SELECT p.Sifra, p.Naziv, o.Vrsta, p.Opis, o.DodatniOpis, p.Stanje, p.Cijena 
                                        FROM proizvod AS p
                                        JOIN ostalo AS o ON p.Sifra = o.PROIZVOD_Sifra AND p.Postoji=1";
                        MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                        adapter.Fill(proizvodi);

                        ///Nacin da se preimenuju kolone
                        /*proizvodi.Columns["Sifra"].ColumnName = "Product Code";
                        proizvodi.Columns["Naziv"].ColumnName = "Product Name";
                        proizvodi.Columns["Vrsta"].ColumnName = "Category";
                        proizvodi.Columns["Opis"].ColumnName = "Description";
                        proizvodi.Columns["DodatniOpis"].ColumnName = "Additional Description";
                        proizvodi.Columns["Stanje"].ColumnName = "Stock";
                        proizvodi.Columns["Cijena"].ColumnName = "Price";*/

                        proizvodi.Columns["Sifra"].ColumnName = "Šifra";
                        proizvodi.Columns["DodatniOpis"].ColumnName = "Dodatni opis";
                        proizvodi.Columns["Cijena"].ColumnName = "Cijena (KM)";

                        proizvodiView = proizvodi.DefaultView;
                        IsEmpty = proizvodi.Rows.Count == 0;
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error fetching products: {e.Message}");
                    }
                    finally
                    {
                        DBConnector.closeConnection(conn);
                    }
                }
            }
        }

        public void loadLjubimci()
        {
            DataTable proizvodi = new DataTable();
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        string query = @"SELECT p.Sifra, p.Naziv, lj.VrstaZivotinje, lj.Rasa, lj.Tezina, lj.Uzrast, p.Opis, p.Stanje, p.Cijena 
                                       FROM proizvod AS p
                                       JOIN ljubimac AS lj ON p.Sifra = lj.PROIZVOD_Sifra AND p.Postoji=1";
                        MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                        adapter.Fill(proizvodi);

                        proizvodi.Columns["Sifra"].ColumnName = "Šifra";
                        proizvodi.Columns["VrstaZivotinje"].ColumnName = "Vrsta životinje";
                        proizvodi.Columns["Tezina"].ColumnName = "Težina (kg)";
                        proizvodi.Columns["Uzrast"].ColumnName = "Uzrast (m)";
                        proizvodi.Columns["Cijena"].ColumnName = "Cijena (KM)";

                        proizvodiView = proizvodi.DefaultView;
                        IsEmpty = proizvodi.Rows.Count == 0;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error fetching products: {e.Message}");
                    }
                    finally
                    {
                        DBConnector.closeConnection(conn);
                    }
                }
            }
        }

        public void SacuvajPromjeneOstalo(int sifra, string naziv, int stanje, decimal cijena, string opis, string vrsta, string dodatniOpis)
        {
            using(MySqlConnection conn = DBConnector.getConnection())
            {
                if(conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("IzmijeniOstalo", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("p_sifra", sifra);
                        cmd.Parameters.AddWithValue("p_naziv", naziv);
                        cmd.Parameters.AddWithValue("p_stanje", stanje);
                        cmd.Parameters.AddWithValue("p_cijena", cijena);
                        cmd.Parameters.AddWithValue("p_opis", opis);
                        cmd.Parameters.AddWithValue("p_vrsta", vrsta);
                        cmd.Parameters.AddWithValue("p_dodatni_opis", dodatniOpis);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Podaci su uspješno izmjenjeni.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                    } 
                    catch(Exception e)
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

        public void SacuvajPromjeneLjubimac(int sifra, string naziv, int stanje, decimal cijena, string opis, string vrstaZivotinje, string rasa, decimal uzrast, decimal tezina)
        {
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("IzmijeniLjubimac", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("p_sifra", sifra);
                        cmd.Parameters.AddWithValue("p_naziv", naziv);
                        cmd.Parameters.AddWithValue("p_stanje", stanje);
                        cmd.Parameters.AddWithValue("p_cijena", cijena);
                        cmd.Parameters.AddWithValue("p_opis", opis);
                        cmd.Parameters.AddWithValue("p_vrsta_zivotinje", vrstaZivotinje);
                        cmd.Parameters.AddWithValue("p_uzrast", uzrast);
                        cmd.Parameters.AddWithValue("p_tezina", tezina);
                        cmd.Parameters.AddWithValue("p_rasa", rasa);

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

        public void DodajProizvodOstalo(int sifra, string naziv, decimal cijena, int stanje, string opis, string vrsta, string dodatniOpis)
        {
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("DodajProizvodOstalo", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("pSifra", sifra);
                        cmd.Parameters.AddWithValue("pNaziv", naziv);
                        cmd.Parameters.AddWithValue("pStanje", stanje);
                        cmd.Parameters.AddWithValue("pCijena", cijena);
                        cmd.Parameters.AddWithValue("pOpis", opis);
                        cmd.Parameters.AddWithValue("pVrsta", vrsta);
                        cmd.Parameters.AddWithValue("pDodatniOpis", dodatniOpis);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Novi proizvod je uspješno sačuvan.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Greška prilikom dodavanja proizvoda: {e.Message}");
                    }
                    finally
                    {
                        DBConnector.closeConnection(conn);
                    }
                }
            }
        }

        public void DodajProizvodLjubimac(int sifra, string naziv, decimal cijena, int stanje, string opis, string vrstaZivotinje, string rasa, decimal tezina, decimal uzrast)
        {
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("DodajProizvodLjubimac", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("pSifra", sifra);
                        cmd.Parameters.AddWithValue("pNaziv", naziv);
                        cmd.Parameters.AddWithValue("pStanje", stanje);
                        cmd.Parameters.AddWithValue("pCijena", cijena);
                        cmd.Parameters.AddWithValue("pOpis", opis);
                        cmd.Parameters.AddWithValue("pVrstaZivotinje", vrstaZivotinje);
                        cmd.Parameters.AddWithValue("pRasa", rasa);
                        cmd.Parameters.AddWithValue("pUzrast", uzrast);
                        cmd.Parameters.AddWithValue("pTezina", tezina);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Novi proizvod je uspješno sačuvan.", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Greška prilikom dodavanja proizvoda: {e.Message}");
                    }
                    finally
                    {
                        DBConnector.closeConnection(conn);
                    }
                }
            }
        }

        public bool ProvjeraSifra(string sifra)
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
                        cmd.CommandText = "SELECT COUNT(*) FROM proizvod WHERE Sifra = @sifra";
                        cmd.Parameters.AddWithValue("@sifra", sifra);
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
    }
}
