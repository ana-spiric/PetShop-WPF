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
    public class StavkeRacuna : ViewModelBase
    {
        ICommand SearchRacun { get; set; }

        private string _podaci;
        public string Podaci
        {
            get { return _podaci; }
            set { _podaci = value; onPropertyChanged(nameof(Podaci)); }
        }
        public StavkeRacuna(string sifra, string cijena)
        {
            Podaci = string.Empty;
            Sifra = sifra;
            Cijena = cijena;
            SearchRacun = new RelayCommand(ExecuteSearch);
            ExecuteSearch(null);
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

        private string _sifra;
        public string Sifra
        {
            get => _sifra;
            set
            {
                _sifra = value; onPropertyChanged(nameof(Sifra));
            }
        }

        private DataView _stavkeRacuna;
        public DataView StavkeView
        {
            get { return _stavkeRacuna; }
            set
            {
                _stavkeRacuna = value;
                onPropertyChanged(nameof(StavkeView));
            }
        }

        private string _searchTextRacun;
        public string SearchTextRacun
        {
            get { return _searchTextRacun; }
            set { _searchTextRacun = value; onPropertyChanged(nameof(SearchTextRacun)); ExecuteSearch(null); }
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
                        MySqlCommand cmd = new MySqlCommand("PretraziStavka", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@searchTerm", SearchTextRacun ?? string.Empty);
                        cmd.Parameters.AddWithValue("@sifra", Sifra);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(proizvodi);
                        proizvodi.Columns["PROIZVOD_Sifra"].ColumnName = "Šifra";
                        proizvodi.Columns["KolicinaProizvoda"].ColumnName = "Količina";
                        proizvodi.Columns["ProdajnaCijena"].ColumnName = "Prodajna cijena (KM)";
                        StavkeView = new DataView(proizvodi);
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
