using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniClubManagementSystem
{
    public partial class MainDashboard : Form
    {
        public MainDashboard()
        {
            InitializeComponent();
        }

        private void MainDashboard_Load(object sender, EventArgs e)
        {
            // Personalize the welcome message
            lblWelcome.Text = $"Welcome, {UserSession.FullName}! (Role: {UserSession.Role})";

            // Hide all restricted buttons by default
            btnManageUsers.Visible = false;
            btnManageClubs.Visible = false;
            btnManageEvents.Visible = false;
            btnMyClubs.Visible = false;
            btnManageMembers.Visible = false;
            btnReports.Visible = false;

            // Show buttons based on the user's role
            switch (UserSession.Role)
            {
                case "Admin":
                    btnManageMembers.Visible = true;
                    btnManageUsers.Visible = true;
                    btnManageClubs.Visible = true;
                    btnManageEvents.Visible = true;
                    btnReports.Visible=true;
                    break;
                case "Coordinator":
                    btnManageMembers.Visible = true;
                    btnManageClubs.Visible = true;
                    btnManageEvents.Visible = true;
                    break;
                case "Advisor":
                    btnManageEvents.Visible = true; // To approve events
                    break;
                case "Student":
                    btnMyClubs.Visible = true;
                    btnManageEvents.Visible = true; // Just to view events
                    break;
            }
        }

        private void btnManageClubs_Click(object sender, EventArgs e)
        {
            ManageClubsForm clubsForm = new ManageClubsForm();
            clubsForm.ShowDialog(); // ShowDialog locks the dashboard until this form is closed
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            ManageUsersForm usersForm = new ManageUsersForm();
            usersForm.ShowDialog();
        }

        private void btnManageEvents_Click(object sender, EventArgs e)
        {
            ManageEventsForm eventsForm = new ManageEventsForm();
            eventsForm.ShowDialog();
        }

        private void btnMyClubs_Click(object sender, EventArgs e)
        {
            MyClubsForm myClubsForm = new MyClubsForm();
            myClubsForm.ShowDialog();
        }

        private void btnManageMembers_Click(object sender, EventArgs e)
        {
            ManageMembersForm membersForm = new ManageMembersForm();
            membersForm.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // 1. Ask for confirmation
            DialogResult confirm = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                // 2. Clear the global user session we created in Step 4
                UserSession.ClearSession();

                // 3. Restart the application! 
                // This is the safest way in Windows Forms to guarantee all memory is cleared 
                // and the user is securely returned to a fresh Login screen without leaving hidden forms running.
                Application.Restart();
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.ShowDialog();
        }
    }
}
