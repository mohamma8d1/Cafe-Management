using System.Windows.Forms;

namespace CafeManagemnt
{
    partial class AdminForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.CafeTitle = new System.Windows.Forms.Label();
            this.Header = new System.Windows.Forms.Panel();
            this.username = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Reportbtn = new CafeManagemnt.Controls.MainButton();
            this.Restorebtn = new CafeManagemnt.Controls.MainButton();
            this.Backupbtn = new CafeManagemnt.Controls.MainButton();
            this.UserShowbtn = new CafeManagemnt.Controls.MainButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Adduserbtn = new CafeManagemnt.Controls.MainButton();
            this.AddRolebtn = new CafeManagemnt.Controls.MainButton();
            this.Header.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // CafeTitle
            // 
            this.CafeTitle.AutoSize = true;
            this.CafeTitle.Font = new System.Drawing.Font("Microsoft Tai Le", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CafeTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(137)))), ((int)(((byte)(104)))));
            this.CafeTitle.Location = new System.Drawing.Point(12, 22);
            this.CafeTitle.Name = "CafeTitle";
            this.CafeTitle.Size = new System.Drawing.Size(250, 34);
            this.CafeTitle.TabIndex = 0;
            this.CafeTitle.Text = "Cafe Managemnet";
            // 
            // Header
            // 
            this.Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Header.Controls.Add(this.username);
            this.Header.Controls.Add(this.CafeTitle);
            this.Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.Header.Location = new System.Drawing.Point(0, 0);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(1085, 80);
            this.Header.TabIndex = 1;
            // 
            // username
            // 
            this.username.AutoSize = true;
            this.username.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(137)))), ((int)(((byte)(104)))));
            this.username.Location = new System.Drawing.Point(897, 31);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(91, 20);
            this.username.TabIndex = 1;
            this.username.Text = "Username";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.panel1.Controls.Add(this.Reportbtn);
            this.panel1.Controls.Add(this.Restorebtn);
            this.panel1.Controls.Add(this.Backupbtn);
            this.panel1.Controls.Add(this.UserShowbtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 596);
            this.panel1.TabIndex = 2;
            // 
            // Reportbtn
            // 
            this.Reportbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.Reportbtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.Reportbtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.Reportbtn.BorderRadius = 0;
            this.Reportbtn.BorderSize = 0;
            this.Reportbtn.FlatAppearance.BorderSize = 0;
            this.Reportbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Reportbtn.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Reportbtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Reportbtn.Location = new System.Drawing.Point(0, 234);
            this.Reportbtn.Name = "Reportbtn";
            this.Reportbtn.Size = new System.Drawing.Size(262, 72);
            this.Reportbtn.TabIndex = 4;
            this.Reportbtn.Text = "Report";
            this.Reportbtn.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Reportbtn.UseVisualStyleBackColor = false;
            this.Reportbtn.Click += new System.EventHandler(this.Reportbtn_Click);
            // 
            // Restorebtn
            // 
            this.Restorebtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.Restorebtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.Restorebtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.Restorebtn.BorderRadius = 0;
            this.Restorebtn.BorderSize = 0;
            this.Restorebtn.FlatAppearance.BorderSize = 0;
            this.Restorebtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Restorebtn.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Restorebtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Restorebtn.Location = new System.Drawing.Point(0, 156);
            this.Restorebtn.Name = "Restorebtn";
            this.Restorebtn.Size = new System.Drawing.Size(262, 72);
            this.Restorebtn.TabIndex = 5;
            this.Restorebtn.Text = "Restore";
            this.Restorebtn.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Restorebtn.UseVisualStyleBackColor = false;
            this.Restorebtn.Click += new System.EventHandler(this.Restorebtn_Click);
            // 
            // Backupbtn
            // 
            this.Backupbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.Backupbtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.Backupbtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.Backupbtn.BorderRadius = 0;
            this.Backupbtn.BorderSize = 0;
            this.Backupbtn.FlatAppearance.BorderSize = 0;
            this.Backupbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Backupbtn.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Backupbtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Backupbtn.Location = new System.Drawing.Point(0, 78);
            this.Backupbtn.Name = "Backupbtn";
            this.Backupbtn.Size = new System.Drawing.Size(262, 72);
            this.Backupbtn.TabIndex = 4;
            this.Backupbtn.Text = "Backup";
            this.Backupbtn.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Backupbtn.UseVisualStyleBackColor = false;
            this.Backupbtn.Click += new System.EventHandler(this.Backupbtn_Click);
            // 
            // UserShowbtn
            // 
            this.UserShowbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.UserShowbtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.UserShowbtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.UserShowbtn.BorderRadius = 0;
            this.UserShowbtn.BorderSize = 0;
            this.UserShowbtn.FlatAppearance.BorderSize = 0;
            this.UserShowbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UserShowbtn.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserShowbtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.UserShowbtn.Location = new System.Drawing.Point(0, 0);
            this.UserShowbtn.Name = "UserShowbtn";
            this.UserShowbtn.Size = new System.Drawing.Size(262, 72);
            this.UserShowbtn.TabIndex = 3;
            this.UserShowbtn.Text = "Users";
            this.UserShowbtn.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.UserShowbtn.UseVisualStyleBackColor = false;
            this.UserShowbtn.Click += new System.EventHandler(this.UserShowbtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(333, 108);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(676, 470);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.Visible = false;
            // 
            // Adduserbtn
            // 
            this.Adduserbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Adduserbtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Adduserbtn.BorderColor = System.Drawing.Color.White;
            this.Adduserbtn.BorderRadius = 20;
            this.Adduserbtn.BorderSize = 0;
            this.Adduserbtn.FlatAppearance.BorderSize = 0;
            this.Adduserbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Adduserbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Adduserbtn.ForeColor = System.Drawing.Color.White;
            this.Adduserbtn.Location = new System.Drawing.Point(369, 594);
            this.Adduserbtn.Name = "Adduserbtn";
            this.Adduserbtn.Size = new System.Drawing.Size(168, 60);
            this.Adduserbtn.TabIndex = 4;
            this.Adduserbtn.Text = "Add User";
            this.Adduserbtn.TextColor = System.Drawing.Color.White;
            this.Adduserbtn.UseVisualStyleBackColor = false;
            this.Adduserbtn.Visible = false;
            this.Adduserbtn.Click += new System.EventHandler(this.Adduserbtn_Click);
            // 
            // AddRolebtn
            // 
            this.AddRolebtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.AddRolebtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.AddRolebtn.BorderColor = System.Drawing.Color.White;
            this.AddRolebtn.BorderRadius = 20;
            this.AddRolebtn.BorderSize = 0;
            this.AddRolebtn.FlatAppearance.BorderSize = 0;
            this.AddRolebtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddRolebtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddRolebtn.ForeColor = System.Drawing.Color.White;
            this.AddRolebtn.Location = new System.Drawing.Point(808, 594);
            this.AddRolebtn.Name = "AddRolebtn";
            this.AddRolebtn.Size = new System.Drawing.Size(168, 60);
            this.AddRolebtn.TabIndex = 5;
            this.AddRolebtn.Text = "Add Role";
            this.AddRolebtn.TextColor = System.Drawing.Color.White;
            this.AddRolebtn.UseVisualStyleBackColor = false;
            this.AddRolebtn.Visible = false;
            this.AddRolebtn.Click += new System.EventHandler(this.AddRolebtn_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(137)))), ((int)(((byte)(104)))));
            this.ClientSize = new System.Drawing.Size(1085, 676);
            this.Controls.Add(this.AddRolebtn);
            this.Controls.Add(this.Adduserbtn);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Header);
            this.Name = "AdminForm";
            this.Text = "AdminForm";
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.Header.ResumeLayout(false);
            this.Header.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel reportPanel;
        private ComboBox reportTypeComboBox;
        private DateTimePicker fromDatePicker;
        private DateTimePicker toDatePicker;
        private Controls.MainButton generateReportButton;
        private DataGridView reportDataGridView;
        private Controls.MainButton exportReportButton;
        private System.Windows.Forms.Label CafeTitle;
        private System.Windows.Forms.Panel Header;
        private System.Windows.Forms.Label username;
        private System.Windows.Forms.Panel panel1;
        private Controls.MainButton UserShowbtn;
        private Controls.MainButton Reportbtn;
        private Controls.MainButton Restorebtn;
        private Controls.MainButton Backupbtn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private Controls.MainButton Adduserbtn;
        private Controls.MainButton AddRolebtn;
    }
}