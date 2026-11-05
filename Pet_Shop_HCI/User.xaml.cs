using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Pet_Shop_HCI.views;
using FontAwesome.Sharp;
using MySql.Data.MySqlClient;
using PetShopHCI.classes;
using Pet_Shop_HCI.classes;

namespace Pet_Shop_HCI
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Window
    {
        public User()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            PodesiTemu(Sesija.Tema);

            foreach (ComboBoxItem item in TemaSelektor.Items)
            {
                if(item.Tag != null && item.Tag.ToString() == Sesija.Tema)
                {
                    TemaSelektor.SelectedItem = item;
                    break;
                }
            }

            Loaded += (s, e) =>
            {
               PregledProizvodaButton.IsChecked = true;
            };
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da se želite odjaviti?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Window nextWindow = new LogIn();
                nextWindow.Show();
                this.Close();
            }
            else
            {
                return;
            }
        }

        private void PregledProizvodaButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentControlMain.Content = new PregledProizvodaView();
            IconContent.Icon = IconChar.ListOl;
            CaptionContent.Text = ((TextBlock)((RadioButton)sender).FindName("ListProizvodi")).Text;
        }

        private void ProdajaButton_Checked(object sender, RoutedEventArgs e)
        {
            ContentControlMain.Content = new ProdajaView();
            IconContent.Icon = IconChar.Receipt;
            CaptionContent.Text = ((TextBlock)((RadioButton)sender).FindName("Prodaja")).Text;
        }


        //tema funkcije

        private void TemaSelektor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem comboBoxItem = comboBox.SelectedItem as ComboBoxItem;
            PodesiTemu(comboBoxItem.Tag.ToString());
            PoveziSaKorisnikom(comboBoxItem.Tag.ToString());
        }

        private void PodesiTemu(string tema)
        {
            Application.Current.Resources.Clear();
            ResourceDictionary dictionary = new ResourceDictionary()
            {
                Source = new Uri($"/styles/{tema}.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Add(dictionary);

            var boolToVisibilityConverter = new Pet_Shop_HCI.styles.BoolToVisibilityConverter();
            Application.Current.Resources.Add("BoolToVisibilityConverter", boolToVisibilityConverter);

        }

        private void PoveziSaKorisnikom(string tema)
        {
            using (MySqlConnection conn = DBConnector.getConnection())
            {
                if(conn != null)
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "UPDATE zaposleni SET Tema = @Tema WHERE JMB = @Jmb";
                        cmd.Parameters.AddWithValue("@Tema", tema);
                        cmd.Parameters.AddWithValue("@Jmb", Sesija.JMB);
                        cmd.ExecuteNonQuery();
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
