using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; // za MySQL
using System.Data.SqlClient; // za SQL Server
using System.Data.OleDb; // za access
using System.Configuration;

namespace PetShopHCI.classes
{
    class DBConnector
    {
        public static MySqlConnection getConnection()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;  
                MySqlConnection conn = new MySqlConnection(connectionString);
                // MySqlConnection conn = new MySqlConnection("Server=localhost;Database=hciprojekat;UserId=root;Password=;");
                return conn;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error creating MySQL connection: {e.Message}");
                return null;
            }        
        }

        public static void closeConnection(MySqlConnection conn)
        {
            if (conn != null)
            {
                conn.Close();
            }    
        }
    }
}
