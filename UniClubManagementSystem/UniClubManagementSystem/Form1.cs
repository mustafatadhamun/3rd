using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniClubManagementSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            // 1. Create an instance of our DatabaseHelper class
            DatabaseHelper dbHelper = new DatabaseHelper();

            // 2. Attempt to open the connection
            var connection = dbHelper.GetConnection();

            // 3. Check if the connection was successful
            if (connection != null)
            {
                // Show a success message
                MessageBox.Show("Success! Your project is connected to UniClubDB.",
                                "Connection Test",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                // 4. Always close the connection when you are done!
                dbHelper.CloseConnection();
            }
            // Note: We don't need an 'else' block because our DatabaseHelper class 
            // already shows an error MessageBox if the connection fails.
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hash the entered password to compare with the database
            string hashedPassword = SecurityHelper.HashPassword(password);

            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();

            if (conn != null)
            {
                try
                {
                    // Query to check credentials and get the user's role
                    string query = @"SELECT u.UserID, u.FullName, r.RoleName 
                             FROM Users u 
                             JOIN Roles r ON u.RoleID = r.RoleID 
                             WHERE u.Username = @username AND u.PasswordHash = @password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parameters prevent SQL injection
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 1. Set the global session variables
                                UserSession.UserID = Convert.ToInt32(reader["UserID"]);
                                UserSession.FullName = reader["FullName"].ToString();
                                UserSession.Role = reader["RoleName"].ToString();

                                // 2. Hide the login form
                                this.Hide();

                                // 3. Open the Dashboard
                                MainDashboard dashboard = new MainDashboard();
                                dashboard.FormClosed += (s, args) => this.Close(); // Close app entirely if dashboard is closed
                                dashboard.Show();
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    dbHelper.CloseConnection();
                }
            }
        }
    }
}
