namespace CafeManagemnt
{
    partial class StaffForm
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

        private void InitializeComponent()
        {
            this.Header = new System.Windows.Forms.Panel();
            this.username = new System.Windows.Forms.Label();
            this.CafeTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.OrderProssbtn = new CafeManagemnt.Controls.MainButton();
            this.CreateOrderbtn = new CafeManagemnt.Controls.MainButton();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.Header.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
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
            this.Header.TabIndex = 2;
            // 
            // username
            // 
            this.username.AutoSize = true;
            this.username.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(137)))), ((int)(((byte)(104)))));
            this.username.Location = new System.Drawing.Point(950, 31);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(49, 20);
            this.username.TabIndex = 1;
            this.username.Text = "Employee";
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
            this.CafeTitle.Text = "Cafe Management";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.panel1.Controls.Add(this.OrderProssbtn);
            this.panel1.Controls.Add(this.CreateOrderbtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 596);
            this.panel1.TabIndex = 3;
            // 
            // OrderProssbtn
            // 
            this.OrderProssbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.OrderProssbtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.OrderProssbtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.OrderProssbtn.BorderRadius = 0;
            this.OrderProssbtn.BorderSize = 0;
            this.OrderProssbtn.FlatAppearance.BorderSize = 0;
            this.OrderProssbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OrderProssbtn.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrderProssbtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.OrderProssbtn.Location = new System.Drawing.Point(0, 78);
            this.OrderProssbtn.Name = "OrderProssbtn";
            this.OrderProssbtn.Size = new System.Drawing.Size(262, 72);
            this.OrderProssbtn.TabIndex = 4;
            this.OrderProssbtn.Text = "Order Processing";
            this.OrderProssbtn.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.OrderProssbtn.UseVisualStyleBackColor = false;
            this.OrderProssbtn.Click += new System.EventHandler(this.OrderProssbtn_Click);
            // 
            // CreateOrderbtn
            // 
            this.CreateOrderbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.CreateOrderbtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(102)))), ((int)(((byte)(68)))));
            this.CreateOrderbtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(224)))), ((int)(((byte)(212)))));
            this.CreateOrderbtn.BorderRadius = 0;
            this.CreateOrderbtn.BorderSize = 0;
            this.CreateOrderbtn.FlatAppearance.BorderSize = 0;
            this.CreateOrderbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CreateOrderbtn.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateOrderbtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.CreateOrderbtn.Location = new System.Drawing.Point(0, 0);
            this.CreateOrderbtn.Name = "CreateOrderbtn";
            this.CreateOrderbtn.Size = new System.Drawing.Size(262, 72);
            this.CreateOrderbtn.TabIndex = 3;
            this.CreateOrderbtn.Text = "Create Order";
            this.CreateOrderbtn.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(40)))), ((int)(((byte)(24)))));
            this.CreateOrderbtn.UseVisualStyleBackColor = false;
            this.CreateOrderbtn.Click += new System.EventHandler(this.CreateOrderbtn_Click);
            // 
            // contentPanel
            // 
            this.contentPanel.Location = new System.Drawing.Point(268, 86);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(810, 584);
            this.contentPanel.TabIndex = 5;
            // 
            // StaffForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(137)))), ((int)(((byte)(104)))));
            this.ClientSize = new System.Drawing.Size(1085, 676);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Header);
            this.Name = "StaffForm";
            this.Text = "StaffForm";
            this.Load += new System.EventHandler(this.StaffForm_Load);
            this.Header.ResumeLayout(false);
            this.Header.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel Header;
        private System.Windows.Forms.Label username;
        private System.Windows.Forms.Label CafeTitle;
        private System.Windows.Forms.Panel panel1;
        private Controls.MainButton CreateOrderbtn;
        private Controls.MainButton OrderProssbtn;
        private System.Windows.Forms.Panel contentPanel;
    }
}
