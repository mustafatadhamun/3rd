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
    public partial class ManageUsersForm : Form
    {
        private int selectedUserID = 0;
        private DataTable usersTable; // NEW
        public ManageUsersForm()
        {
            InitializeComponent();
            LoadRoles(); // Load dropdown first
            LoadUsers();
        }

        // --- READ: Load users into the grid ---
        private void LoadUsers()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = @"SELECT u.UserID, u.Username, u.FullName, u.Email, r.RoleName 
                             FROM Users u 
                             JOIN Roles r ON u.RoleID = r.RoleID";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);

                    usersTable = new DataTable(); // Use the global table
                    adapter.Fill(usersTable);
                    dgvUsers.DataSource = usersTable;
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        // --- READ: Populate the Role ComboBox ---
        private void LoadRoles()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = "SELECT RoleID, RoleName FROM Roles";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbRole.DataSource = dt;
                    cmbRole.DisplayMember = "RoleName"; // What the user sees
                    cmbRole.ValueMember = "RoleID";     // The actual ID saved to the database
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedUserID == 0) return;

            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = "UPDATE Users SET Username=@user, FullName=@name, Email=@email, RoleID=@role WHERE UserID=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@name", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@role", cmbRole.SelectedValue);
                    cmd.Parameters.AddWithValue("@id", selectedUserID);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User updated successfully!");
                    ClearFields();
                    LoadUsers();
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUserID == 0) return;

            if (MessageBox.Show("Delete this user?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                var conn = dbHelper.GetConnection();
                if (conn != null)
                {
                    try
                    {
                        string query = "DELETE FROM Users WHERE UserID=@id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", selectedUserID);
                        cmd.ExecuteNonQuery();

                        ClearFields();
                        LoadUsers();
                    }
                    catch (Exception ex) { MessageBox.Show("Cannot delete user. They might be tied to a club or event."); }
                    finally { dbHelper.CloseConnection(); }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearFields();

        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            txtEmail.Clear();
            selectedUserID = 0;
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Make sure the user clicked a valid row (not the column headers)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];

                // SAFETY CHECK: Ignore the click if it's the empty "new" row at the bottom
                if (row.IsNewRow) return;

                // SAFETY CHECK: Ensure the UserID cell isn't empty/null before converting
                if (row.Cells["UserID"].Value != DBNull.Value && row.Cells["UserID"].Value != null)
                {
                    selectedUserID = Convert.ToInt32(row.Cells["UserID"].Value);

                    // Fill the textboxes. We use .ToString() safely here.
                    txtUsername.Text = row.Cells["Username"].Value?.ToString();
                    txtFullName.Text = row.Cells["FullName"].Value?.ToString();
                    txtEmail.Text = row.Cells["Email"].Value?.ToString();
                    cmbRole.Text = row.Cells["RoleName"].Value?.ToString();

                    txtPassword.Text = ""; // Keep password hidden
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Username and Password are required!");
                return;
            }

            // Encrypt the password before saving!
            string hashedPwd = SecurityHelper.HashPassword(txtPassword.Text.Trim());

            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = "INSERT INTO Users (Username, PasswordHash, FullName, Email, RoleID) VALUES (@user, @pass, @name, @email, @role)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", hashedPwd);
                    cmd.Parameters.AddWithValue("@name", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@role", cmbRole.SelectedValue);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User added successfully!");
                    ClearFields();
                    LoadUsers();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
                finally { dbHelper.CloseConnection(); }
            }
        }

        private void ManageUsersForm_Load(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (usersTable != null)
            {
                string searchValue = txtSearch.Text.Trim().Replace("'", "''");
                // Search across multiple columns!
                usersTable.DefaultView.RowFilter = string.Format("Username LIKE '%{0}%' OR FullName LIKE '%{0}%' OR Email LIKE '%{0}%'", searchValue);
            }
        }
    }
}
