using Pet_Shop_HCI.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for DodajDobavljacaWindow.xaml
    /// </summary>
    public partial class DodajDobavljacaWindow : Window
    {
        Narudzba narudzba = new Narudzba();
        public DodajDobavljacaWindow()
        {
            InitializeComponent();
            DataContext = narudzba;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private static bool IsInteger(string currentText, string newText)
        {
            string fullText = currentText.Insert(currentText.Length, newText);
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(fullText);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInteger((sender as TextBox).Text, e.Text);
        }

        private void ButtonSacuvajPromjene_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li želite sačuvati promjene?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                string jib = JIBtxt.Text;
                string naziv = NazivTxt.Text;
                string adresa = AdresaTxt.Text;

                if (string.IsNullOrEmpty(jib) || string.IsNullOrEmpty(naziv))
                {
                    MessageBox.Show("Niste popunili obavezna polja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //Narudzba narudzba = new Narudzba();

                if (narudzba.ProvjeraJIB(jib))
                {
                    MessageBox.Show("JIB već postoji u bazi.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    narudzba.DodajDobavljaca(jib, naziv, adresa);
                    narudzba.loadDobavljaci();
                    this.Close();
                }
            }     
        }
    }
}
