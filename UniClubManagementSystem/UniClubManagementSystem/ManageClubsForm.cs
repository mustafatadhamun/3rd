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
    public partial class ManageClubsForm : Form
    {
        private int selectedClubID = 0; // Tracks which club is selected in the grid
        private DataTable clubsTable; // NEW: Holds our data globally so we can search it
        public ManageClubsForm()
        {
            InitializeComponent();
            dgvClubs.AllowUserToAddRows = false; // (If you added this earlier)

            LoadClubs(); // <-- CRITICAL: This must be here!
        }

        // --- READ: Load data into DataGridView ---

        // --- VALIDATION HELPER ---
        private bool IsInputValid()
        {
            // 1. Check for empty fields
            if (string.IsNullOrWhiteSpace(txtClubName.Text))
            {
                MessageBox.Show("Club Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClubName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(rtbDescription.Text))
            {
                MessageBox.Show("Club Description cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbDescription.Focus();
                return false;
            }

            // 2. Check length requirements
            if (txtClubName.Text.Trim().Length < 3)
            {
                MessageBox.Show("Club Name must be at least 3 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClubName.Focus();
                return false;
            }

            return true; // Everything is valid!
        }

        // --- PREVENT DUPLICATES ---
        private bool IsClubNameDuplicate(string clubName, int currentClubId = 0)
        {
            bool isDuplicate = false;
            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    // Check if the name exists, ignoring the current club if we are updating it
                    string query = "SELECT COUNT(*) FROM Clubs WHERE ClubName = @name AND ClubID != @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", clubName);
                    cmd.Parameters.AddWithValue("@id", currentClubId);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0) isDuplicate = true;
                }
                finally { dbHelper.CloseConnection(); }
            }
            return isDuplicate;
        }
        private void LoadClubs()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = "SELECT ClubID, ClubName, Description, CreationDate FROM Clubs";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);

                    clubsTable = new DataTable(); // <-- Must use the global variable here
                    adapter.Fill(clubsTable);
                    dgvClubs.DataSource = clubsTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading clubs: " + ex.Message);
                }
                finally
                {
                    dbHelper.CloseConnection();
                }
            }
        }



        private void ManageClubsForm_Load(object sender, EventArgs e)
        {

        }

        private void dgvClubs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvClubs.Rows[e.RowIndex];
                selectedClubID = Convert.ToInt32(row.Cells["ClubID"].Value);
                txtClubName.Text = row.Cells["ClubName"].Value.ToString();
                rtbDescription.Text = row.Cells["Description"].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // 1. Run basic validation
            if (!IsInputValid()) return;

            // 2. Check for duplicates
            if (IsClubNameDuplicate(txtClubName.Text.Trim()))
            {
                MessageBox.Show("A club with this name already exists! Please choose a different name.", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtClubName.Text))
            {
                MessageBox.Show("Club Name is required!");
                return;
            }

            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = "INSERT INTO Clubs (ClubName, Description) VALUES (@name, @desc)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", txtClubName.Text.Trim());
                    cmd.Parameters.AddWithValue("@desc", rtbDescription.Text.Trim());
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Club added successfully!");
                    ClearFields();
                    LoadClubs(); // Refresh the grid
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedClubID == 0)
            {
                MessageBox.Show("Please select a club from the list first.");
                return;
            }

            // 1. Run basic validation
            if (!IsInputValid()) return;

            // 2. Check for duplicates (pass the selected ID so it doesn't flag itself as a duplicate!)
            if (IsClubNameDuplicate(txtClubName.Text.Trim(), selectedClubID))
            {
                MessageBox.Show("Another club is already using this name!", "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedClubID == 0)
            {
                MessageBox.Show("Please select a club from the list first.");
                return;
            }

            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = "UPDATE Clubs SET ClubName=@name, Description=@desc WHERE ClubID=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", txtClubName.Text.Trim());
                    cmd.Parameters.AddWithValue("@desc", rtbDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", selectedClubID);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Club updated successfully!");
                    ClearFields();
                    LoadClubs();
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedClubID == 0) return;

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this club?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                var conn = dbHelper.GetConnection();
                if (conn != null)
                {
                    try
                    {
                        string query = "DELETE FROM Clubs WHERE ClubID=@id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", selectedClubID);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Club deleted.");
                        ClearFields();
                        LoadClubs();
                    }
                    finally { dbHelper.CloseConnection(); }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        private void ClearFields()
        {
            txtClubName.Clear();
            rtbDescription.Clear();
            selectedClubID = 0;
        }

        private void rtbDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Make sure the table actually has data before we try to search it
            if (clubsTable != null)
            {
                // Get the search text and escape single quotes to prevent crashes (e.g., searching for "Bob's Club")
                string searchValue = txtSearch.Text.Trim().Replace("'", "''");

                // Filter the rows! We use LIKE to find partial matches in either the Name or the Description.
                clubsTable.DefaultView.RowFilter = string.Format("ClubName LIKE '%{0}%' OR Description LIKE '%{0}%'", searchValue);
            }
        }
    }
}
