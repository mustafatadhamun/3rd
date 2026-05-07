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
    public partial class ManageMembersForm : Form
    {
        private int selectedMembershipID = 0;
        private DataTable membersTable; // NEW
        public ManageMembersForm()
        {
            InitializeComponent();
            dgvMembers.AllowUserToAddRows = false; // Code method to hide blank row
            LoadMembers();
        }

        // --- READ: Load memberships based on Role ---
        private void LoadMembers()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    // Base query to get student name, club name, and status
                    string query = @"SELECT m.MembershipID, u.FullName AS StudentName, 
                        c.ClubName AS Club, m.Status, m.JoinDate 
                 FROM Memberships m
                 JOIN Users u ON m.StudentID = u.UserID
                 JOIN Clubs c ON m.ClubID = c.ClubID ";

                    // If it's a Coordinator, only show their clubs!
                    if (UserSession.Role == "Coordinator")
                    {
                        query += "WHERE c.CoordinatorID = @userId";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (UserSession.Role == "Coordinator")
                    {
                        cmd.Parameters.AddWithValue("@userId", UserSession.UserID);
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    membersTable = new DataTable(); // Use the global table
                    adapter.Fill(membersTable);
                    dgvMembers.DataSource = membersTable;

                    // Hide the ID column for a cleaner look
                    if (dgvMembers.Columns["MembershipID"] != null)
                        dgvMembers.Columns["MembershipID"].Visible = false;

                    // Add this line so it looks pretty!
                    if (dgvMembers.Columns["StudentName"] != null)
                        dgvMembers.Columns["StudentName"].HeaderText = "Student Name";
                }
                finally { dbHelper.CloseConnection(); }
            }
        }
        private void ManageMembersForm_Load(object sender, EventArgs e)
        {

        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            UpdateStatus("Approved");
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            UpdateStatus("Rejected");
        }
        private void UpdateStatus(string newStatus)
        {
            if (selectedMembershipID == 0)
            {
                MessageBox.Show("Please select a student's request from the list.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = "UPDATE Memberships SET Status = @status WHERE MembershipID = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@id", selectedMembershipID);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Membership marked as {newStatus}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    selectedMembershipID = 0;
                    LoadMembers();
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (selectedMembershipID == 0) return;

            if (MessageBox.Show("Are you sure you want to completely remove this student from the club?", "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                var conn = dbHelper.GetConnection();
                if (conn != null)
                {
                    try
                    {
                        string query = "DELETE FROM Memberships WHERE MembershipID = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", selectedMembershipID);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Student removed from the club.", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        selectedMembershipID = 0;
                        LoadMembers();
                    }
                    finally { dbHelper.CloseConnection(); }
                }
            }
        }

        private void dgvMembers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !dgvMembers.Rows[e.RowIndex].IsNewRow)
            {
                var val = dgvMembers.Rows[e.RowIndex].Cells["MembershipID"].Value;
                if (val != DBNull.Value && val != null)
                {
                    selectedMembershipID = Convert.ToInt32(val);
                }
            }
        }

        private void dgvMembers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (membersTable != null)
            {
                if(membersTable != null)
    {
                    string searchValue = txtSearch.Text.Trim().Replace("'", "''");

                    // No more brackets! Just clean column names.
                    membersTable.DefaultView.RowFilter = string.Format("StudentName LIKE '%{0}%' OR Club LIKE '%{0}%' OR Status LIKE '%{0}%'", searchValue);
                }
            }
        }
    }
}
