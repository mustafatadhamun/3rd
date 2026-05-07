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
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            dgvReport.AllowUserToAddRows = false;
            LoadReport();
        }
        private void LoadReport()
        {
            DataTable dt = new DataTable();
            DatabaseHelper dbHelper = new DatabaseHelper();
            var conn = dbHelper.GetConnection();

            if (conn != null)
            {
                try
                {
                    // This query proves you know how to use JOIN, COUNT, and GROUP BY for reporting!
                    string sql = @"SELECT c.ClubName AS 'Club Name', 
                                          IFNULL(u.FullName, 'No Coordinator') AS 'Coordinator', 
                                          COUNT(m.MembershipID) AS 'Total Approved Members',
                                          c.CreationDate AS 'Date Created'
                                   FROM Clubs c
                                   LEFT JOIN Users u ON c.CoordinatorID = u.UserID
                                   LEFT JOIN Memberships m ON c.ClubID = m.ClubID AND m.Status = 'Approved'
                                   GROUP BY c.ClubID, c.ClubName, u.FullName, c.CreationDate
                                   ORDER BY 'Total Approved Members' DESC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        var adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }

                    dgvReport.DataSource = dt;
                    dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating report: " + ex.Message);
                }
                finally
                {
                    dbHelper.CloseConnection();
                }
            }
        }
        private void ReportsForm_Load(object sender, EventArgs e)
        {
            
        }

        private void dgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
