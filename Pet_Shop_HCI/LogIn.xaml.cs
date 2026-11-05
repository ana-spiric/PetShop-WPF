using System.Windows;
using System.Windows.Input;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using PetShopHCI.classes;
using System.Text;
using System;
using Pet_Shop_HCI.classes;

namespace Pet_Shop_HCI
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPassword.Password;

            if(!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                Employee employee = validationLogIn(username, password);
                if(employee != null)
                {
                    if (employee.active == 0)
                    {
                        MessageBox.Show("Prijavu pokušava korisnik koji više nema aktivan status.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Sesija.JMB = employee.jmb;
                    Sesija.KorisnickoIme = employee.userName;
                    Sesija.Admin = employee.admin == 1;
                    Sesija.Tema = employee.theme;

                    Window nextWindow;
                    if (employee.admin == 1)
                    {
                        nextWindow = new Admin();
                    }
                    else
                    {
                        nextWindow = new User();
                    }
                    nextWindow.Show();
                    this.Close();
                }
                else 
                {
                    MessageBox.Show("Pogrešno korisničko ime ili lozinka.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Polja za korisničko ime i lozinku ne mogu biti prazna.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private Employee validationLogIn(string username, string password)
        {
            MySqlConnection conn = DBConnector.getConnection();
            try
            {
                conn.Open();
                MySqlCommand command = conn.CreateCommand();
                //command.CommandText = "SELECT COUNT(*) FROM zaposleni WHERE KorisnickoIme = @username AND Lozinka = @password;";
                command.CommandText = "SELECT JMB, Ime, Prezime, Admin, Status, KorisnickoIme, Lozinka, Tema FROM zaposleni WHERE KorisnickoIme = @username AND Lozinka = @password;";
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", sha256hash(password));
                //int rowCount = Convert.ToInt32(command.ExecuteScalar());
                //return rowCount == 1;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                       return new Employee(
                            reader.GetString("JMB"),
                            reader.GetString("Ime"),
                            reader.GetString("Prezime"),
                            reader.GetInt32("Admin"),
                            reader.GetInt32("Status"),
                            reader.GetString("KorisnickoIme"),
                            reader.GetString("Lozinka"),
                            reader.GetString("Tema")
                        );
                        /*if(e.active == 1)
                        {
                            return e;
                        }
                        else
                        {
                            return null;
                        }*/
                    }
                    return null;
                }

            } catch(MySqlException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
                //return false;
            }
            finally
            {
                conn.Close();
            }
        }

        private string sha256hash(string password)
        {
            using (SHA256 hash = SHA256.Create())
            {
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder stringBuilder = new StringBuilder();
                for(int i = 0; i < bytes.Length; i++)
                {
                    stringBuilder.Append(bytes[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogIn_Click(sender, e);
            }
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogIn_Click(sender, e);
            }
        }
    }
}
