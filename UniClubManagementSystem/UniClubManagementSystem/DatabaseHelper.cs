using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace UniClubManagementSystem
{
    public class DatabaseHelper
    {
        // Update "your_password" with your actual MySQL root password
        private string connectionString = "Server=localhost;Database=UniClubDB;Uid=root;Pwd=12345;port=3306";
        private MySqlConnection connection;

        public DatabaseHelper()
        {
            connection = new MySqlConnection(connectionString);
        }

        // Method to open the connection
        public MySqlConnection GetConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Connection Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Method to close the connection
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}