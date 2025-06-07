using System.Data;
using System.Windows.Forms;

namespace CafeManagemnt
{
    partial class OwnerForm
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
            this.Header = new System.Windows.Forms.Panel();
            this.username = new System.Windows.Forms.Label();
            this.CafeTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Backtostaff = new CafeManagemnt.Controls.MainButton();
            this.EFstaff_btn = new CafeManagemnt.Controls.MainButton();
            this.Stafflist_btn = new CafeManagemnt.Controls.MainButton();
            this.Newstaff_btn = new CafeManagemnt.Controls.MainButton();
            this.UserShowbtn = new CafeManagemnt.Controls.MainButton();
            this.Report_btn = new CafeManagemnt.Controls.MainButton();
            this.InventoryManagement_btn = new CafeManagemnt.Controls.MainButton();
            this.StaffManagement_btn = new CafeManagemnt.Controls.MainButton();
            this.MenuManagement_btn = new CafeManagemnt.Controls.MainButton();
            this.Header.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Header
            // 
            this.Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Header.Controls.Add(this.username);
            this.Header.Controls.Add(this.CafeTitle);
            this.Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.Header.Location = new System.Drawing.Point(0, 0);
            this.Header.Margin = new System.Windows.Forms.Padding(4);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(1447, 89);
            this.Header.TabIndex = 6;
            // 
            // username
            // 
            this.username.AutoSize = true;
            this.username.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(137)))), ((int)(((byte)(104)))));
            this.username.Location = new System.Drawing.Point(1196, 38);
            this.username.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(75, 25);
            this.username.TabIndex = 1;
            this.username.Text = "Owner";
            // 
            // CafeTitle
            // 
            this.CafeTitle.AutoSize = true;
            this.CafeTitle.Font = new System.Drawing.Font("Microsoft Tai Le", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CafeTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(137)))), ((int)(((byte)(104)))));
            this.CafeTitle.Location = new System.Drawing.Point(16, 27);
            this.CafeTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CafeTitle.Name = "CafeTitle";
            this.CafeTitle.Size = new System.Drawing.Size(310, 44);
            this.CafeTitle.TabIndex = 0;
            this.CafeTitle.Text = "Cafe Managemnet";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.panel1.Controls.Add(this.UserShowbtn);
            this.panel1.Controls.Add(this.Report_btn);
            this.panel1.Controls.Add(this.InventoryManagement_btn);
            this.panel1.Controls.Add(this.StaffManagement_btn);
            this.panel1.Controls.Add(this.MenuManagement_btn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 89);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(349, 743);
            this.panel1.TabIndex = 9;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(137)))), ((int)(((byte)(104)))));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.Color.Black;
            this.dataGridView1.Location = new System.Drawing.Point(348, 89);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(1099, 622);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.Visible = false;
            // 
            // Backtostaff
            // 
            this.Backtostaff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Backtostaff.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Backtostaff.BorderColor = System.Drawing.Color.White;
            this.Backtostaff.BorderRadius = 20;
            this.Backtostaff.BorderSize = 0;
            this.Backtostaff.FlatAppearance.BorderSize = 0;
            this.Backtostaff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Backtostaff.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Backtostaff.ForeColor = System.Drawing.Color.White;
            this.Backtostaff.Location = new System.Drawing.Point(793, 737);
            this.Backtostaff.Margin = new System.Windows.Forms.Padding(4);
            this.Backtostaff.Name = "Backtostaff";
            this.Backtostaff.Size = new System.Drawing.Size(208, 83);
            this.Backtostaff.TabIndex = 17;
            this.Backtostaff.Text = "Back";
            this.Backtostaff.TextColor = System.Drawing.Color.White;
            this.Backtostaff.UseVisualStyleBackColor = false;
            this.Backtostaff.Visible = false;
            this.Backtostaff.Click += new System.EventHandler(this.Backtostaff_Click);
            // 
            // EFstaff_btn
            // 
            this.EFstaff_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.EFstaff_btn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.EFstaff_btn.BorderColor = System.Drawing.Color.White;
            this.EFstaff_btn.BorderRadius = 20;
            this.EFstaff_btn.BorderSize = 0;
            this.EFstaff_btn.FlatAppearance.BorderSize = 0;
            this.EFstaff_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EFstaff_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EFstaff_btn.ForeColor = System.Drawing.Color.White;
            this.EFstaff_btn.Location = new System.Drawing.Point(727, 540);
            this.EFstaff_btn.Margin = new System.Windows.Forms.Padding(4);
            this.EFstaff_btn.Name = "EFstaff_btn";
            this.EFstaff_btn.Size = new System.Drawing.Size(339, 100);
            this.EFstaff_btn.TabIndex = 15;
            this.EFstaff_btn.Text = "Edit / Fair Staff";
            this.EFstaff_btn.TextColor = System.Drawing.Color.White;
            this.EFstaff_btn.UseVisualStyleBackColor = false;
            this.EFstaff_btn.Visible = false;
            this.EFstaff_btn.Click += new System.EventHandler(this.Editstaff_btn_Click);
            // 
            // Stafflist_btn
            // 
            this.Stafflist_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Stafflist_btn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Stafflist_btn.BorderColor = System.Drawing.Color.White;
            this.Stafflist_btn.BorderRadius = 20;
            this.Stafflist_btn.BorderSize = 0;
            this.Stafflist_btn.FlatAppearance.BorderSize = 0;
            this.Stafflist_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Stafflist_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Stafflist_btn.ForeColor = System.Drawing.Color.White;
            this.Stafflist_btn.Location = new System.Drawing.Point(727, 380);
            this.Stafflist_btn.Margin = new System.Windows.Forms.Padding(4);
            this.Stafflist_btn.Name = "Stafflist_btn";
            this.Stafflist_btn.Size = new System.Drawing.Size(339, 100);
            this.Stafflist_btn.TabIndex = 14;
            this.Stafflist_btn.Text = "Staff List";
            this.Stafflist_btn.TextColor = System.Drawing.Color.White;
            this.Stafflist_btn.UseVisualStyleBackColor = false;
            this.Stafflist_btn.Visible = false;
            this.Stafflist_btn.Click += new System.EventHandler(this.Stafflist_btn_Click);
            // 
            // Newstaff_btn
            // 
            this.Newstaff_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Newstaff_btn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Newstaff_btn.BorderColor = System.Drawing.Color.White;
            this.Newstaff_btn.BorderRadius = 20;
            this.Newstaff_btn.BorderSize = 0;
            this.Newstaff_btn.FlatAppearance.BorderSize = 0;
            this.Newstaff_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Newstaff_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Newstaff_btn.ForeColor = System.Drawing.Color.White;
            this.Newstaff_btn.Location = new System.Drawing.Point(727, 220);
            this.Newstaff_btn.Margin = new System.Windows.Forms.Padding(4);
            this.Newstaff_btn.Name = "Newstaff_btn";
            this.Newstaff_btn.Size = new System.Drawing.Size(339, 100);
            this.Newstaff_btn.TabIndex = 13;
            this.Newstaff_btn.Text = "New Staff";
            this.Newstaff_btn.TextColor = System.Drawing.Color.White;
            this.Newstaff_btn.UseVisualStyleBackColor = false;
            this.Newstaff_btn.Visible = false;
            this.Newstaff_btn.Click += new System.EventHandler(this.Newstaff_btn_Click);
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
            this.UserShowbtn.Margin = new System.Windows.Forms.Padding(4);
            this.UserShowbtn.Name = "UserShowbtn";
            this.UserShowbtn.Size = new System.Drawing.Size(349, 89);
            this.UserShowbtn.TabIndex = 3;
            this.UserShowbtn.Text = "Users";
            this.UserShowbtn.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.UserShowbtn.UseVisualStyleBackColor = false;
            this.UserShowbtn.Click += new System.EventHandler(this.UserShowbtn_Click);
            // 
            // Report_btn
            // 
            this.Report_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.Report_btn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.Report_btn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.Report_btn.BorderRadius = 0;
            this.Report_btn.BorderSize = 0;
            this.Report_btn.FlatAppearance.BorderSize = 0;
            this.Report_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Report_btn.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Report_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Report_btn.Location = new System.Drawing.Point(0, 385);
            this.Report_btn.Margin = new System.Windows.Forms.Padding(4);
            this.Report_btn.Name = "Report_btn";
            this.Report_btn.Size = new System.Drawing.Size(349, 89);
            this.Report_btn.TabIndex = 6;
            this.Report_btn.Text = "Report";
            this.Report_btn.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.Report_btn.UseVisualStyleBackColor = false;
            this.Report_btn.Click += new System.EventHandler(this.Report_btn_Click);
            // 
            // InventoryManagement_btn
            // 
            this.InventoryManagement_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.InventoryManagement_btn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.InventoryManagement_btn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.InventoryManagement_btn.BorderRadius = 0;
            this.InventoryManagement_btn.BorderSize = 0;
            this.InventoryManagement_btn.FlatAppearance.BorderSize = 0;
            this.InventoryManagement_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InventoryManagement_btn.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InventoryManagement_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.InventoryManagement_btn.Location = new System.Drawing.Point(0, 96);
            this.InventoryManagement_btn.Margin = new System.Windows.Forms.Padding(4);
            this.InventoryManagement_btn.Name = "InventoryManagement_btn";
            this.InventoryManagement_btn.Size = new System.Drawing.Size(349, 89);
            this.InventoryManagement_btn.TabIndex = 4;
            this.InventoryManagement_btn.Text = "Inventory Management";
            this.InventoryManagement_btn.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.InventoryManagement_btn.UseVisualStyleBackColor = false;
            this.InventoryManagement_btn.Click += new System.EventHandler(this.InventoryManagement_btn_Click);
            // 
            // StaffManagement_btn
            // 
            this.StaffManagement_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.StaffManagement_btn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.StaffManagement_btn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.StaffManagement_btn.BorderRadius = 0;
            this.StaffManagement_btn.BorderSize = 0;
            this.StaffManagement_btn.FlatAppearance.BorderSize = 0;
            this.StaffManagement_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StaffManagement_btn.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StaffManagement_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.StaffManagement_btn.Location = new System.Drawing.Point(0, 192);
            this.StaffManagement_btn.Margin = new System.Windows.Forms.Padding(4);
            this.StaffManagement_btn.Name = "StaffManagement_btn";
            this.StaffManagement_btn.Size = new System.Drawing.Size(349, 89);
            this.StaffManagement_btn.TabIndex = 5;
            this.StaffManagement_btn.Text = "Staff Management";
            this.StaffManagement_btn.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.StaffManagement_btn.UseVisualStyleBackColor = false;
            this.StaffManagement_btn.Click += new System.EventHandler(this.StaffManagement_btn_Click);
            // 
            // MenuManagement_btn
            // 
            this.MenuManagement_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.MenuManagement_btn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.MenuManagement_btn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.MenuManagement_btn.BorderRadius = 0;
            this.MenuManagement_btn.BorderSize = 0;
            this.MenuManagement_btn.FlatAppearance.BorderSize = 0;
            this.MenuManagement_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MenuManagement_btn.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuManagement_btn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.MenuManagement_btn.Location = new System.Drawing.Point(0, 288);
            this.MenuManagement_btn.Margin = new System.Windows.Forms.Padding(4);
            this.MenuManagement_btn.Name = "MenuManagement_btn";
            this.MenuManagement_btn.Size = new System.Drawing.Size(349, 89);
            this.MenuManagement_btn.TabIndex = 4;
            this.MenuManagement_btn.Text = "Menu Management";
            this.MenuManagement_btn.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.MenuManagement_btn.UseVisualStyleBackColor = false;
            this.MenuManagement_btn.Click += new System.EventHandler(this.MenuManagement_btn_Click);
            // 
            // OwnerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(137)))), ((int)(((byte)(104)))));
            this.ClientSize = new System.Drawing.Size(1447, 832);
            this.Controls.Add(this.Backtostaff);
            this.Controls.Add(this.EFstaff_btn);
            this.Controls.Add(this.Stafflist_btn);
            this.Controls.Add(this.Newstaff_btn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Header);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OwnerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cofe Management";
            this.Load += new System.EventHandler(this.OwnerForm_Load);
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
        private Controls.MainButton exportReportButton;
        private DataGridView reportDataGridView;
        private DataTable _reportDataTable; // To store the report data
        private System.Windows.Forms.Panel Header;
        private System.Windows.Forms.Label username;
        private System.Windows.Forms.Label CafeTitle;
        private Controls.MainButton MenuManagement_btn;
        private Controls.MainButton StaffManagement_btn;
        private Controls.MainButton InventoryManagement_btn;
        private Controls.MainButton UserShowbtn;
        private Controls.MainButton Report_btn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private Controls.MainButton Newstaff_btn;
        private Controls.MainButton Stafflist_btn;
        private Controls.MainButton EFstaff_btn;
        private Controls.MainButton Backtostaff;
    }
}