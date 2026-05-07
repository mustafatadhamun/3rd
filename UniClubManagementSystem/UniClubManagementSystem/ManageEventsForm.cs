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
    public partial class ManageEventsForm : Form
    {
        private int selectedEventID = 0;
        private DataTable eventsTable; // NEW
        public ManageEventsForm()
        {
            InitializeComponent();
            // Add this line right here:
            dgvEvents.AllowUserToAddRows = false;
            LoadClubs();
            LoadEvents();
            ApplyRolePermissions();
        }

        // --- Restrict actions based on Role ---
        private void ApplyRolePermissions()
        {
            // If the user is just an Advisor, maybe they shouldn't Add/Delete events, only Update the status
            if (UserSession.Role == "Advisor")
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                cmbStatus.Enabled = true; // They can change status
            }
            // If Student, they can only view
            else if (UserSession.Role == "Student")
            {
                btnAdd.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                btnClear.Visible = false;
            }
        }

        // --- READ: Load clubs into dropdown ---
        private void LoadClubs()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = "SELECT ClubID, ClubName FROM Clubs";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cmbClub.DataSource = dt;
                    cmbClub.DisplayMember = "ClubName";
                    cmbClub.ValueMember = "ClubID";
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        // --- READ: Load events into DataGridView ---
        private void LoadEvents()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = @"SELECT e.EventID, c.ClubName, e.EventName, e.Description, 
                                    e.EventDate, e.Location, e.ApprovalStatus, e.ClubID 
                             FROM Events e
                             JOIN Clubs c ON e.ClubID = c.ClubID";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);

                    eventsTable = new DataTable(); // Use the global table
                    adapter.Fill(eventsTable);
                    dgvEvents.DataSource = eventsTable;

                    if (dgvEvents.Columns["ClubID"] != null)
                        dgvEvents.Columns["ClubID"].Visible = false;
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void ManageEventsForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEventName.Text))
            {
                MessageBox.Show("Event Name is required!");
                return;
            }

            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = @"INSERT INTO Events (ClubID, EventName, Description, EventDate, Location, ApprovalStatus) 
                                     VALUES (@club, @name, @desc, @date, @loc, @status)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@club", cmbClub.SelectedValue);
                    cmd.Parameters.AddWithValue("@name", txtEventName.Text.Trim());
                    cmd.Parameters.AddWithValue("@desc", rtbDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@date", dtpEventDate.Value);
                    cmd.Parameters.AddWithValue("@loc", txtLocation.Text.Trim());
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text); // Defaults to whatever is selected (e.g., Pending)

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event scheduled successfully!");
                    ClearFields();
                    LoadEvents();
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedEventID == 0) return;

            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();
            if (conn != null)
            {
                try
                {
                    string query = @"UPDATE Events SET ClubID=@club, EventName=@name, Description=@desc, 
                                                       EventDate=@date, Location=@loc, ApprovalStatus=@status 
                                     WHERE EventID=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@club", cmbClub.SelectedValue);
                    cmd.Parameters.AddWithValue("@name", txtEventName.Text.Trim());
                    cmd.Parameters.AddWithValue("@desc", rtbDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@date", dtpEventDate.Value);
                    cmd.Parameters.AddWithValue("@loc", txtLocation.Text.Trim());
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@id", selectedEventID);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event updated successfully!");
                    ClearFields();
                    LoadEvents();
                }
                finally { dbHelper.CloseConnection(); }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedEventID == 0) return;

            if (MessageBox.Show("Cancel this event?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                var conn = dbHelper.GetConnection();
                if (conn != null)
                {
                    try
                    {
                        string query = "DELETE FROM Events WHERE EventID=@id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", selectedEventID);
                        cmd.ExecuteNonQuery();

                        ClearFields();
                        LoadEvents();
                    }
                    finally { dbHelper.CloseConnection(); }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearFields();

        private void ClearFields()
        {
            txtEventName.Clear();
            rtbDescription.Clear();
            txtLocation.Clear();
            cmbStatus.SelectedIndex = -1; // Deselects status
            dtpEventDate.Value = DateTime.Now;
            selectedEventID = 0;
        }

        private void dgvEvents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvEvents.Rows[e.RowIndex];
                if (row.IsNewRow) return;

                if (row.Cells["EventID"].Value != DBNull.Value && row.Cells["EventID"].Value != null)
                {
                    selectedEventID = Convert.ToInt32(row.Cells["EventID"].Value);

                    cmbClub.SelectedValue = row.Cells["ClubID"].Value;
                    txtEventName.Text = row.Cells["EventName"].Value?.ToString();
                    rtbDescription.Text = row.Cells["Description"].Value?.ToString();

                    if (DateTime.TryParse(row.Cells["EventDate"].Value?.ToString(), out DateTime eventDate))
                        dtpEventDate.Value = eventDate;

                    txtLocation.Text = row.Cells["Location"].Value?.ToString();
                    cmbStatus.Text = row.Cells["ApprovalStatus"].Value?.ToString();
                }
            }
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (eventsTable != null)
            {
                string searchValue = txtSearch.Text.Trim().Replace("'", "''");
                // Search by Event Name, Location, or Club Name!
                eventsTable.DefaultView.RowFilter = string.Format("EventName LIKE '%{0}%' OR Location LIKE '%{0}%' OR ClubName LIKE '%{0}%'", searchValue);
            }
        }
    }
}
