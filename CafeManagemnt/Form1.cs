using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace CafeManagemnt
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = @"Data Source=MOHAMMAD-LOQ;Initial Catalog=CafeManagementDB;Integrated Security=True;Encrypt=False";

        public Form1()
        {
            InitializeComponent();
        }
        public static bool CreateUser(string username, string password, string role)
        {
            try
            {
                string hashedPassword = PasswordHelper.HashPassword(password);

                string query = @"INSERT INTO Login_users 
                               (username, password_hash, role, status) 
                               VALUES (@username, @password_hash, @role, '1')";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password_hash", hashedPassword);
                    command.Parameters.AddWithValue("@role", role);

                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool UpdatePassword(string username, string newPassword)
        {
            try
            {
                string hashedPassword = PasswordHelper.HashPassword(newPassword);

                string query = @"UPDATE Login_users 
                                SET password_hash = @password_hash 
                                WHERE username = @username";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password_hash", hashedPassword);

                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch
            {
                return false;
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            
        }

        private void ShowLoginError(string message)
        {
            MessageBox.Show(message, "Login Failed",MessageBoxButtons.OK, MessageBoxIcon.Error);
            passwordTxtBox.Texts = "";
            usernameTxtBox.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string username = usernameTxtBox.Texts.Trim();
            string password = passwordTxtBox.Texts;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = @"SELECT username, password_hash, role, status 
                                   FROM Login_users 
                                   WHERE username = @username AND status = '1'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                string storedHash = reader["password_hash"].ToString();
                                string role = reader["role"].ToString();

                                if (PasswordHelper.VerifyPassword(password, storedHash))
                                {
                                    OpenRoleSpecificForm(role);
                                    this.Hide();
                                }
                                else
                                {
                                    ShowLoginError("Invalid username or password");
                                }
                            }
                            else
                            {
                                ShowLoginError("Invalid username or password or Not Active account");
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenRoleSpecificForm(string role)
        {
            // First hide the login form
            this.Hide();
            try
            {
                Form roleForm = null;

                switch (role.ToLower())
                {
                    case "admin":
                        roleForm = new AdminForm();
                        break;

                    case "owner":
                        roleForm = new OwnerForm();
                        break;

                    case "staff":
                        roleForm = new StaffForm();
                        break;

                    default:
                        MessageBox.Show("Unknown user role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Show();
                        return;
                }

                roleForm.FormClosed += (sender, e) =>
                {
                    this.Close();
                };
                roleForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
            }
        }
    }
}