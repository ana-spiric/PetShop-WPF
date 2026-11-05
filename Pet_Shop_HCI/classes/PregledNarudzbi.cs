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
    public class PregledNarudzbi : ViewModelBase
    {
        public ICommand Search { get; set; }
        public ICommand Pregledaj { get; set; }

        public PregledNarudzbi()
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

        private DataView narudzbe;
        public DataView narudzbeView
        {
            get { return narudzbe; }
            set
            {
                narudzbe = value;
                onPropertyChanged(nameof(narudzbeView));
            }
        }

        private void ExecutePregledaj(object obj)
        {
            var narudzba = obj as DataRowView;
            if(narudzba != null)
            {
                var sifra = narudzba["Šifra narudžbe"].ToString();
                var datum = narudzba["Datum kreiranja"].ToString();
                var ime = narudzba["Ime"].ToString();
                var prezime = narudzba["Prezime"].ToString();
                var naziv = narudzba["Naziv dobavljača"].ToString();
                var cijena = narudzba["Ukupna cijena"].ToString();
                var dialog = new ProizvodiNarudzbeWindow(sifra, datum, ime, prezime, naziv, cijena);
                dialog.Show();
            }
            else
            {
                Console.WriteLine("Greska.");
            }
        }

        private void ExecuteSearch(object obj)
        {
            DataTable narudzbe = new DataTable();
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if (conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("PretraziNabavku", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@searchTerm", SearchText ?? string.Empty);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(narudzbe);
                        narudzbe.Columns["SifraNabavke"].ColumnName = "Šifra narudžbe";
                        narudzbe.Columns["DatumKreiranja"].ColumnName = "Datum kreiranja";
                        narudzbe.Columns["ZAPOSLENI_JMB"].ColumnName = "Matični broj";
                        narudzbe.Columns["DOBAVLJAC_JIB"].ColumnName = "JIB dobavljača";
                        narudzbe.Columns["Naziv"].ColumnName = "Naziv dobavljača";
                        narudzbe.Columns["UkupnaCijena"].ColumnName = "Ukupna cijena";
                        narudzbeView = new DataView(narudzbe);
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
