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
    public partial class MyClubsForm : Form
    {

        private int selectedAvailableClubID = 0;
        private int selectedMembershipID = 0;
        public MyClubsForm()
        {
            InitializeComponent();
            LoadAvailableClubs();
            LoadMyClubs();
        }

        // --- READ: Show clubs the student has NOT joined yet ---
        private void LoadAvailableClubs()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    // Select clubs where the ClubID is NOT in the student's membership list
                    string query = @"SELECT ClubID, ClubName, Description 
                                     FROM Clubs 
                                     WHERE ClubID NOT IN (SELECT ClubID FROM Memberships WHERE StudentID = @studentId)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@studentId", UserSession.UserID);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvAvailableClubs.DataSource = dt;
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        // --- READ: Show clubs the student has already joined (or requested to join) ---
        private void LoadMyClubs()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = @"SELECT m.MembershipID, c.ClubName, m.JoinDate, m.Status 
                                     FROM Memberships m
                                     JOIN Clubs c ON m.ClubID = c.ClubID
                                     WHERE m.StudentID = @studentId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@studentId", UserSession.UserID);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvMyClubs.DataSource = dt;

                    // Hide the MembershipID column so the UI stays clean
                    if (dgvMyClubs.Columns["MembershipID"] != null)
                        dgvMyClubs.Columns["MembershipID"].Visible = false;
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        private void MyClubsForm_Load(object sender, EventArgs e)
        {

        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            if (selectedAvailableClubID == 0)
            {
                MessageBox.Show("Please select a club from the 'Available Clubs' list to join.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    // Add the student to the Memberships table with a 'Pending' status
                    string query = "INSERT INTO Memberships (StudentID, ClubID, Status) VALUES (@studentId, @clubId, 'Pending')";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@studentId", UserSession.UserID);
                    cmd.Parameters.AddWithValue("@clubId", selectedAvailableClubID);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Successfully requested to join the club! Status is pending.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    selectedAvailableClubID = 0; // Reset selection

                    // Refresh both tables! The club should move from the top table to the bottom table.
                    LoadAvailableClubs();
                    LoadMyClubs();
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            if (selectedMembershipID == 0)
            {
                MessageBox.Show("Please select a club from 'My Current Memberships' to leave.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to leave this club or cancel your request?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                var conn = dbHelper.GetConnection();
                if (conn != null)
                {
                    try
                    {
                        string query = "DELETE FROM Memberships WHERE MembershipID = @membershipId";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@membershipId", selectedMembershipID);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("You have left the club.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        selectedMembershipID = 0; // Reset selection
                        LoadAvailableClubs();
                        LoadMyClubs();
                    }
                    finally { dbHelper.CloseConnection(); }
                }
            }
        }

        private void dgvAvailableClubs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !dgvAvailableClubs.Rows[e.RowIndex].IsNewRow)
            {
                var val = dgvAvailableClubs.Rows[e.RowIndex].Cells["ClubID"].Value;
                if (val != DBNull.Value && val != null)
                {
                    selectedAvailableClubID = Convert.ToInt32(val);
                }
            }
        }

        private void dgvMyClubs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !dgvMyClubs.Rows[e.RowIndex].IsNewRow)
            {
                var val = dgvMyClubs.Rows[e.RowIndex].Cells["MembershipID"].Value;
                if (val != DBNull.Value && val != null)
                {
                    selectedMembershipID = Convert.ToInt32(val);
                }
            }
        }
    }
}
