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
    /// Interaction logic for DodajProizvodWindow.xaml
    /// </summary>
    public partial class DodajProizvodWindow : Window
    {
        public DodajProizvodWindow()
        {
            InitializeComponent();
        }

        private void VrstaProizvodaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DodatnaPoljaPanel.Children.Clear();
            ComboBoxItem selectedItem = VrstaProizvodaComboBox.SelectedItem as ComboBoxItem;
            string selectedVrsta = selectedItem?.Content.ToString();

            if (selectedVrsta == "Asesoar")
            {
                DodatnaPoljaPanel.Children.Add(new TextBlock { Text = "Vrsta:", Style = (Style)FindResource("TextBlockStyle"), Margin = new Thickness(0, 2, 0, 5) });
                DodatnaPoljaPanel.Children.Add(new TextBox { Width = 200, Padding = new Thickness(5), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0,0,0,10), 
                                                             BorderBrush = (SolidColorBrush)FindResource("textBox")});
                DodatnaPoljaPanel.Children.Add(new TextBlock { Text = "Dodatni opis:", Style = (Style)FindResource("TextBlockStyle"), Margin = new Thickness(0, 10, 0, 5) });
                DodatnaPoljaPanel.Children.Add(new TextBox { Width = 200, Height = 50,  Padding = new Thickness(5), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 0, 10),
                                                             BorderBrush = (SolidColorBrush)FindResource("textBox"), TextWrapping = TextWrapping.Wrap, AcceptsReturn = true });

            }
            else if (selectedVrsta == "Ljubimac")
            {
                DodatnaPoljaPanel.Children.Add(new TextBlock { Text = "Vrsta životinje:", Style = (Style)FindResource("TextBlockStyle"), Margin = new Thickness(0, 2, 0, 5) });
                DodatnaPoljaPanel.Children.Add(new TextBox { Width = 200, Padding = new Thickness(5), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 0, 10),
                                                             BorderBrush = (SolidColorBrush)FindResource("textBox")});
                DodatnaPoljaPanel.Children.Add(new TextBlock { Text = "Uzrast (mjesec):", Style = (Style)FindResource("TextBlockStyle"), Margin = new Thickness(0, 10, 0, 5) });
                DodatnaPoljaPanel.Children.Add(new TextBox { Width = 200, Padding = new Thickness(5), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 0, 10),
                                                             BorderBrush = (SolidColorBrush)FindResource("textBox")});
                DodatnaPoljaPanel.Children.Add(new TextBlock { Text = "Težina (kg):", Style = (Style)FindResource("TextBlockStyle"), Margin = new Thickness(0, 10, 0, 5) });
                DodatnaPoljaPanel.Children.Add(new TextBox { Width = 200, Padding = new Thickness(5), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 0, 10),
                                                             BorderBrush = (SolidColorBrush)FindResource("textBox")});
                DodatnaPoljaPanel.Children.Add(new TextBlock { Text = "Rasa:", Style = (Style)FindResource("TextBlockStyle"), Margin = new Thickness(0, 10, 0, 5) });
                DodatnaPoljaPanel.Children.Add(new TextBox { Width = 200, Padding = new Thickness(5), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 0, 10),
                                                             BorderBrush = (SolidColorBrush)FindResource("textBox")});

                DodatnaPoljaPanel.Children[3].PreviewTextInput += TextBox_PreviewTextInput;
                DodatnaPoljaPanel.Children[5].PreviewTextInput += TextBox_PreviewTextDecimal;
            }
        }

        private void SacuvajProizvodButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li želite sačuvati promjene?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result  == MessageBoxResult.Yes)
            {
                PregledProizvoda pregledProizvoda = new PregledProizvoda();

                string sifra = SifraTxt.Text;
                string naziv = NazivTxt.Text;
                string cijena = CijenaTxt.Text;
                string stanje = StanjeTxt.Text;
                string opis = OpisTxt.Text;
                ComboBoxItem selectedItem = VrstaProizvodaComboBox.SelectedItem as ComboBoxItem;
                string selectedTip = selectedItem?.Content.ToString();

                if (selectedTip == "Asesoar")
                {
                    string vrsta = ((TextBox)DodatnaPoljaPanel.Children[1]).Text;
                    string dodatniOpis = ((TextBox)DodatnaPoljaPanel.Children[3]).Text;
                    if (string.IsNullOrEmpty(sifra) || string.IsNullOrEmpty(naziv) || string.IsNullOrEmpty(cijena) || string.IsNullOrEmpty(stanje)
                        || string.IsNullOrEmpty(vrsta))
                    {
                        MessageBox.Show("Niste popunili obavezna polja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    int stanjeInt = Convert.ToInt32(stanje);
                    decimal cijenaDecimal = Convert.ToDecimal(cijena);
                    int sifraInt = Convert.ToInt32(sifra);

                    if (pregledProizvoda.ProvjeraSifra(sifra))
                    {
                        MessageBox.Show("Proizvod sa unijetom šifrom već postoji.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        pregledProizvoda.DodajProizvodOstalo(sifraInt, naziv, cijenaDecimal, stanjeInt, opis, vrsta, dodatniOpis);
                        this.DialogResult = true;
                        this.Close();
                    }


                }
                else if (selectedTip == "Ljubimac")
                {
                    string vrstaZivotinje = ((TextBox)DodatnaPoljaPanel.Children[1]).Text;
                    string uzrast = ((TextBox)DodatnaPoljaPanel.Children[3]).Text;
                    string tezina = ((TextBox)DodatnaPoljaPanel.Children[5]).Text;
                    string rasa = ((TextBox)DodatnaPoljaPanel.Children[7]).Text;
                    if (string.IsNullOrEmpty(sifra) || string.IsNullOrEmpty(naziv) || string.IsNullOrEmpty(cijena) || string.IsNullOrEmpty(stanje)
                       || string.IsNullOrEmpty(uzrast) || string.IsNullOrEmpty(vrstaZivotinje))
                    {
                        MessageBox.Show("Niste popunili obavezna polja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    decimal uzrastDecimal = Convert.ToDecimal(uzrast);
                    decimal tezinaDecimal;
                    if(string.IsNullOrEmpty(tezina))
                    {
                        tezinaDecimal = 0;
                    }
                    else
                    {
                        tezinaDecimal = Convert.ToDecimal(tezina);
                    }
                    //decimal tezinaDecimal = Convert.ToDecimal(tezina);
                    int stanjeInt = Convert.ToInt32(stanje);
                    decimal cijenaDecimal = Convert.ToDecimal(cijena);
                    int sifraInt = Convert.ToInt32(sifra);

                    if (pregledProizvoda.ProvjeraSifra(sifra))
                    {
                        MessageBox.Show("Proizvod sa unijetom šifrom već postoji.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        pregledProizvoda.DodajProizvodLjubimac(sifraInt, naziv, cijenaDecimal, stanjeInt, opis, vrstaZivotinje, rasa, tezinaDecimal, uzrastDecimal);
                        this.DialogResult = true;
                        this.Close();
                    }
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

        private static bool IsDecimal(string currentText, string newText)
        {
            string decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string fullText = currentText.Insert(currentText.Length, newText);
            Regex regex = new Regex(@"^\d*\" + decimalSeparator + @"?\d*$");
            return !regex.IsMatch(fullText);
        }

        private void TextBox_PreviewTextDecimal(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsDecimal((sender as TextBox).Text, e.Text);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
