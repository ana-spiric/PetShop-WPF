using MySql.Data.MySqlClient;
using Pet_Shop_HCI.views;
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
    public class PregledRacuna : ViewModelBase
    {
        public ICommand Search { get; set; }
        public ICommand Pregledaj { get; set; }

        public PregledRacuna()
        {
            Search = new RelayCommand(ExecuteSearch);
            Pregledaj = new RelayCommand(ExecutePregledaj);
            ExecuteSearch(null);
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; onPropertyChanged(nameof(SearchText)); ExecuteSearch(null); }
        }
        private DataView racuni;
        public DataView racuniView
        {
            get { return racuni; }
            set
            {
                racuni = value;
                onPropertyChanged(nameof(racuniView));
            }
        }

        private void ExecutePregledaj(object obj)
        {
            var narudzba = obj as DataRowView;
            if (narudzba != null)
            {
                var sifra = narudzba["Šifra računa"].ToString();
                var datum = narudzba["Datum kreiranja"].ToString();
                var ime = narudzba["Ime"].ToString();
                var prezime = narudzba["Prezime"].ToString();
                var cijena = narudzba["Ukupna cijena"].ToString();
                var dialog = new StavkeWindow(sifra, datum, ime, prezime, cijena);
                dialog.Show();
            }
            else
            {
                Console.WriteLine("Greska.");
            }
        }

        private void ExecuteSearch(object obj)
        {
            DataTable racuni = new DataTable();
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("PretraziRacun", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@searchTerm", SearchText ?? string.Empty);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(racuni);
                        racuni.Columns["Sifra"].ColumnName = "Šifra računa";
                        racuni.Columns["DatumKreiranja"].ColumnName = "Datum kreiranja";
                        racuni.Columns["ZAPOSLENI_JMB"].ColumnName = "Matični broj";
                        racuni.Columns["UkupnaCijena"].ColumnName = "Ukupna cijena";
                        racuniView = new DataView(racuni);
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
