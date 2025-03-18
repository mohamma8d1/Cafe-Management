namespace CafeManagemnt
{
    partial class Form1
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblusername = new System.Windows.Forms.Label();
            this.lblpassword = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.mainTextBox2 = new CafeManagemnt.Controls.MainTextBox();
            this.mainTextBox1 = new CafeManagemnt.Controls.MainTextBox();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(212)))), ((int)(((byte)(224)))));
            this.lblTitle.Location = new System.Drawing.Point(346, 51);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 55);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Cafe Management";
            this.lblTitle.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblusername
            // 
            this.lblusername.AutoSize = true;
            this.lblusername.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblusername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(212)))), ((int)(((byte)(224)))));
            this.lblusername.Location = new System.Drawing.Point(411, 273);
            this.lblusername.Name = "lblusername";
            this.lblusername.Size = new System.Drawing.Size(108, 27);
            this.lblusername.TabIndex = 3;
            this.lblusername.Text = "Username";
            // 
            // lblpassword
            // 
            this.lblpassword.AutoSize = true;
            this.lblpassword.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblpassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(212)))), ((int)(((byte)(224)))));
            this.lblpassword.Location = new System.Drawing.Point(415, 386);
            this.lblpassword.Name = "lblpassword";
            this.lblpassword.Size = new System.Drawing.Size(104, 27);
            this.lblpassword.TabIndex = 4;
            this.lblpassword.Text = "Password";
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(212)))), ((int)(((byte)(224)))));
            this.lblLogin.Location = new System.Drawing.Point(493, 181);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(104, 40);
            this.lblLogin.TabIndex = 5;
            this.lblLogin.Text = "Login";
            // 
            // mainTextBox2
            // 
            this.mainTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(93)))), ((int)(((byte)(160)))));
            this.mainTextBox2.Bordercolor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(212)))), ((int)(((byte)(224)))));
            this.mainTextBox2.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(45)))), ((int)(((byte)(72)))));
            this.mainTextBox2.Bordersize = 2;
            this.mainTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainTextBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(212)))), ((int)(((byte)(224)))));
            this.mainTextBox2.Location = new System.Drawing.Point(420, 417);
            this.mainTextBox2.Margin = new System.Windows.Forms.Padding(4);
            this.mainTextBox2.Multiline = false;
            this.mainTextBox2.Name = "mainTextBox2";
            this.mainTextBox2.Padding = new System.Windows.Forms.Padding(7);
            this.mainTextBox2.PasswordChar = true;
            this.mainTextBox2.Size = new System.Drawing.Size(250, 39);
            this.mainTextBox2.TabIndex = 1;
            this.mainTextBox2.Texts = "";
            this.mainTextBox2.Underlinedstyle = true;
            // 
            // mainTextBox1
            // 
            this.mainTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(93)))), ((int)(((byte)(160)))));
            this.mainTextBox1.Bordercolor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(212)))), ((int)(((byte)(224)))));
            this.mainTextBox1.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(45)))), ((int)(((byte)(72)))));
            this.mainTextBox1.Bordersize = 2;
            this.mainTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(212)))), ((int)(((byte)(224)))));
            this.mainTextBox1.Location = new System.Drawing.Point(420, 304);
            this.mainTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.mainTextBox1.Multiline = false;
            this.mainTextBox1.Name = "mainTextBox1";
            this.mainTextBox1.Padding = new System.Windows.Forms.Padding(7);
            this.mainTextBox1.PasswordChar = false;
            this.mainTextBox1.Size = new System.Drawing.Size(250, 39);
            this.mainTextBox1.TabIndex = 0;
            this.mainTextBox1.Texts = "";
            this.mainTextBox1.Underlinedstyle = true;
            this.mainTextBox1._TextChanged += new System.EventHandler(this.mainTextBox1__TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(93)))), ((int)(((byte)(160)))));
            this.ClientSize = new System.Drawing.Size(1113, 622);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.lblpassword);
            this.Controls.Add(this.lblusername);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.mainTextBox2);
            this.Controls.Add(this.mainTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.MainTextBox mainTextBox1;
        private Controls.MainTextBox mainTextBox2;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblusername;
        private System.Windows.Forms.Label lblpassword;
        private System.Windows.Forms.Label lblLogin;
    }
}

