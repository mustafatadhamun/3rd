namespace UniClubManagementSystem
{
    partial class MainDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.btnManageClubs = new System.Windows.Forms.Button();
            this.btnManageEvents = new System.Windows.Forms.Button();
            this.btnMyClubs = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnManageMembers = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Location = new System.Drawing.Point(12, 9);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(103, 16);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome, User!";
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.Location = new System.Drawing.Point(136, 196);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(131, 48);
            this.btnManageUsers.TabIndex = 1;
            this.btnManageUsers.Text = "Manage Users";
            this.btnManageUsers.UseVisualStyleBackColor = true;
            this.btnManageUsers.Click += new System.EventHandler(this.btnManageUsers_Click);
            // 
            // btnManageClubs
            // 
            this.btnManageClubs.Location = new System.Drawing.Point(287, 196);
            this.btnManageClubs.Name = "btnManageClubs";
            this.btnManageClubs.Size = new System.Drawing.Size(131, 48);
            this.btnManageClubs.TabIndex = 2;
            this.btnManageClubs.Text = "Manage Clubs";
            this.btnManageClubs.UseVisualStyleBackColor = true;
            this.btnManageClubs.Click += new System.EventHandler(this.btnManageClubs_Click);
            // 
            // btnManageEvents
            // 
            this.btnManageEvents.Location = new System.Drawing.Point(441, 196);
            this.btnManageEvents.Name = "btnManageEvents";
            this.btnManageEvents.Size = new System.Drawing.Size(131, 48);
            this.btnManageEvents.TabIndex = 3;
            this.btnManageEvents.Text = "Manage Events";
            this.btnManageEvents.UseVisualStyleBackColor = true;
            this.btnManageEvents.Click += new System.EventHandler(this.btnManageEvents_Click);
            // 
            // btnMyClubs
            // 
            this.btnMyClubs.Location = new System.Drawing.Point(749, 196);
            this.btnMyClubs.Name = "btnMyClubs";
            this.btnMyClubs.Size = new System.Drawing.Size(131, 48);
            this.btnMyClubs.TabIndex = 4;
            this.btnMyClubs.Text = "My Clubs / Join Clubs";
            this.btnMyClubs.UseVisualStyleBackColor = true;
            this.btnMyClubs.Click += new System.EventHandler(this.btnMyClubs_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(903, 196);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(131, 48);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnManageMembers
            // 
            this.btnManageMembers.Location = new System.Drawing.Point(596, 196);
            this.btnManageMembers.Name = "btnManageMembers";
            this.btnManageMembers.Size = new System.Drawing.Size(131, 48);
            this.btnManageMembers.TabIndex = 6;
            this.btnManageMembers.Text = "Manage Members";
            this.btnManageMembers.UseVisualStyleBackColor = true;
            this.btnManageMembers.Click += new System.EventHandler(this.btnManageMembers_Click);
            // 
            // btnReports
            // 
            this.btnReports.Location = new System.Drawing.Point(512, 250);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(131, 48);
            this.btnReports.TabIndex = 7;
            this.btnReports.Text = "Generate Report";
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // MainDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1286, 540);
            this.Controls.Add(this.btnReports);
            this.Controls.Add(this.btnManageMembers);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnMyClubs);
            this.Controls.Add(this.btnManageEvents);
            this.Controls.Add(this.btnManageClubs);
            this.Controls.Add(this.btnManageUsers);
            this.Controls.Add(this.lblWelcome);
            this.Name = "MainDashboard";
            this.Text = "MainDashboard";
            this.Load += new System.EventHandler(this.MainDashboard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnManageUsers;
        private System.Windows.Forms.Button btnManageClubs;
        private System.Windows.Forms.Button btnManageEvents;
        private System.Windows.Forms.Button btnMyClubs;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnManageMembers;
        private System.Windows.Forms.Button btnReports;
    }
}