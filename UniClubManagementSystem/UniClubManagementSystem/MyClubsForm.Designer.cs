namespace UniClubManagementSystem
{
    partial class MyClubsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvAvailableClubs = new System.Windows.Forms.DataGridView();
            this.dgvMyClubs = new System.Windows.Forms.DataGridView();
            this.btnJoin = new System.Windows.Forms.Button();
            this.btnLeave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableClubs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyClubs)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(765, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "My Current Memberships";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(251, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Available Clubs to Join";
            // 
            // dgvAvailableClubs
            // 
            this.dgvAvailableClubs.AllowUserToAddRows = false;
            this.dgvAvailableClubs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAvailableClubs.Location = new System.Drawing.Point(126, 125);
            this.dgvAvailableClubs.Name = "dgvAvailableClubs";
            this.dgvAvailableClubs.RowHeadersWidth = 51;
            this.dgvAvailableClubs.RowTemplate.Height = 24;
            this.dgvAvailableClubs.Size = new System.Drawing.Size(397, 303);
            this.dgvAvailableClubs.TabIndex = 2;
            this.dgvAvailableClubs.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAvailableClubs_CellClick);
            // 
            // dgvMyClubs
            // 
            this.dgvMyClubs.AllowUserToAddRows = false;
            this.dgvMyClubs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMyClubs.Location = new System.Drawing.Point(641, 125);
            this.dgvMyClubs.Name = "dgvMyClubs";
            this.dgvMyClubs.RowHeadersWidth = 51;
            this.dgvMyClubs.RowTemplate.Height = 24;
            this.dgvMyClubs.Size = new System.Drawing.Size(397, 303);
            this.dgvMyClubs.TabIndex = 3;
            this.dgvMyClubs.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMyClubs_CellClick);
            // 
            // btnJoin
            // 
            this.btnJoin.Location = new System.Drawing.Point(254, 434);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(140, 40);
            this.btnJoin.TabIndex = 4;
            this.btnJoin.Text = "Join Selected Club";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // btnLeave
            // 
            this.btnLeave.Location = new System.Drawing.Point(768, 434);
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Size = new System.Drawing.Size(140, 40);
            this.btnLeave.TabIndex = 5;
            this.btnLeave.Text = "Leave Selected Club";
            this.btnLeave.UseVisualStyleBackColor = true;
            this.btnLeave.Click += new System.EventHandler(this.btnLeave_Click);
            // 
            // MyClubsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 572);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.dgvMyClubs);
            this.Controls.Add(this.dgvAvailableClubs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MyClubsForm";
            this.Text = "MyClubsForm";
            this.Load += new System.EventHandler(this.MyClubsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableClubs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMyClubs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvAvailableClubs;
        private System.Windows.Forms.DataGridView dgvMyClubs;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnLeave;
    }
}