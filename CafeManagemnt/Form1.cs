using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace CafeManagemnt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection connect = new SqlConnection(@"Data Source=MOHAMMAD-LOQ;Initial Catalog=CafeManagementDB;Integrated Security=True;Encrypt=False");




        private void mainTextBox1__TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

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

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string username, user_password;
            username = usernameTxtBox.Texts;
            user_password = passwordTxtBox.Texts;

            try
            {
                connect.Open();

                // Use parameterized query to prevent SQL injection
                string query = "SELECT username, role, status FROM Login_users WHERE username = @username AND password = @password AND status = '1'";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", user_password); // Note: You should hash passwords

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Get the role from database
                            string userRole = reader["role"].ToString();
                            string userstatus = reader["status"].ToString();

                            switch (userRole)
                            {
                                case "admin":
                                    AdminForm admin_Form = new AdminForm();
                                    admin_Form.Show();
                                    this.Hide();
                                    break;

                                case "owner":
                                    OwnerForm owner_Form = new OwnerForm();
                                    owner_Form.Show();
                                    this.Hide();
                                    break;

                                case "staff":
                                    StaffForm staff_Form = new StaffForm();
                                    staff_Form.Show();
                                    this.Hide();
                                    break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid login details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            usernameTxtBox.Texts = "";
                            passwordTxtBox.Texts = "";
                            usernameTxtBox.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connect.Close();
            }
        }
    }
}
