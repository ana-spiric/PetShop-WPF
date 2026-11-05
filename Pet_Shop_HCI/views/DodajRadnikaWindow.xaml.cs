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
using Pet_Shop_HCI.classes;

namespace Pet_Shop_HCI.views
{
    /// <summary>
    /// Interaction logic for DodajRadnikaWindow.xaml
    /// </summary>
    public partial class DodajRadnikaWindow : Window
    {
        public DodajRadnikaWindow()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        

        private void SacuvajRadnikaButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li želite sačuvati promjene?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                string jmb = JMBTextBox.Text;
                string ime = ImeTextBox.Text;
                string prezime = PrezimeTextBox.Text;
                bool admin = AdminCheckBox.IsChecked == true;
                bool status = AktivanCheckBox.IsChecked == true;
                string korisnickoIme = UsernameTextBox.Text;
                string lozinka = PasswordTextBox.Text;

                if (string.IsNullOrEmpty(jmb) || string.IsNullOrEmpty(ime) || string.IsNullOrEmpty(prezime) ||
                    string.IsNullOrEmpty(korisnickoIme) || string.IsNullOrEmpty(lozinka))
                {
                    MessageBox.Show("Sva polja moraju biti popunjena.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int adminInt = admin ? 1 : 0;
                int statusInt = status ? 1 : 0;
                PregledRadnika pregledRadnika = new PregledRadnika();
                if (pregledRadnika.ProvjeraJMB(jmb))
                {
                    MessageBox.Show("Matični broj već postoji u bazi.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (pregledRadnika.ProvjeraKorisnickogImena(korisnickoIme))
                {
                    MessageBox.Show("Korisničko ime već postoji u bazi.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    pregledRadnika.DodajRadnikaUBazu(jmb, ime, prezime, adminInt, statusInt, korisnickoIme, lozinka);
                    this.Close();
                }
            }
            else
            {
                return;
            }
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
    }
}
