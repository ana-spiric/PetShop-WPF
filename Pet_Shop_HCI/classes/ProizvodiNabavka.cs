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
    public class ProizvodiNabavka : ViewModelBase
    {
        private DataView _proizvodiNarudzbaView;

        ICommand SearchNabavka { get; set; }

        private string _podaci;
        public string Podaci
        {
            get { return _podaci; }
            set { _podaci = value; onPropertyChanged(nameof(Podaci)); }
        }

        public ProizvodiNabavka(string sifra, string cijena)
        {
            Podaci = string.Empty;
            SifraNabavkaProizvod = sifra;
            Cijena = cijena;
            SearchNabavka = new RelayCommand(ExecuteSearch);
            ExecuteSearch(null);
        }

        public DataView ProizvodiNarudzbaView
        {
            get { return _proizvodiNarudzbaView; }
            set
            {
                _proizvodiNarudzbaView = value;
                onPropertyChanged(nameof(ProizvodiNarudzbaView));
            }
        }

        private string _cijena;
        public string Cijena
        {
            get => _cijena;
            set
            {
                _cijena = value; onPropertyChanged(nameof(Cijena));
            }
        }

        private string _sifraNabavkaProizvod;
        public string SifraNabavkaProizvod
        {
            get => _sifraNabavkaProizvod;
            set
            {
                _sifraNabavkaProizvod = value; onPropertyChanged(nameof(SifraNabavkaProizvod));
            }
        }

        private string _searchTextNabavka;
        public string SearchTextNabavka
        {
            get { return _searchTextNabavka; }
            set { _searchTextNabavka = value; onPropertyChanged(nameof(SearchTextNabavka)); ExecuteSearch(null); }
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
                        MySqlCommand cmd = new MySqlCommand("PretraziProizvodNabavka", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@searchTerm", SearchTextNabavka ?? string.Empty);
                        cmd.Parameters.AddWithValue("@sifra", SifraNabavkaProizvod);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(proizvodi);
                        proizvodi.Columns["PROIZVOD_Sifra"].ColumnName = "Šifra";
                        proizvodi.Columns["Kolicina"].ColumnName = "Količina";
                        proizvodi.Columns["NabavnaCijena"].ColumnName = "Nabavna cijena (KM)";
                        ProizvodiNarudzbaView = new DataView(proizvodi);
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
