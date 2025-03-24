using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManagemnt
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void mainTextBox1__TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = usernameTxtBox.Texts;
            string password = passwordTxtBox.Texts;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void usernameTxtBox_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void btnLogin_MouseHover(object sender, EventArgs e)
        {
           
        }
    }
}
