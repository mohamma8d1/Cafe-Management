using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CafeManagemnt
{
    public partial class Form1 : Form
    {
        private const string ConnectionString = @"Data Source=.;Initial Catalog=CafemanagementDB;Integrated Security=True;Encrypt=False";

        public Form1()
        {
            InitializeComponent();
        }
        public static bool CreateUser(string username, string password, string role)
        {
            try
            {
                string hashedPassword = PasswordHelper.HashPassword(password);

                string query = @"INSERT INTO users 
                               (username, password_hash, role_id, is_active) 
                               VALUES (@username, @password_hash, 
                                      (SELECT role_id FROM roles WHERE role_name = @role), 1)";

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

                string query = @"UPDATE users 
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
            // Empty method, can be removed if not needed
        }

        private void ShowLoginError(string message)
        {
            MessageBox.Show(message, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    string query = @"SELECT u.user_id, u.username, u.password_hash, r.role_name, u.is_active 
                                   FROM users u
                                   JOIN roles r ON u.role_id = r.role_id
                                   WHERE u.username = @username AND u.is_active = 1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHash = reader["password_hash"].ToString();
                                string role = reader["role_name"].ToString();
                                int userId = Convert.ToInt32(reader["user_id"]);

                                if (PasswordHelper.VerifyPassword(password, storedHash))
                                {
                                    reader.Close();
                                    string logQuery = @"INSERT INTO user_logs (user_id, login_time) 
                                                      VALUES (@user_id, GETDATE())";
                                    using (SqlCommand logCommand = new SqlCommand(logQuery, connection))
                                    {
                                        logCommand.Parameters.AddWithValue("@user_id", userId);
                                        logCommand.ExecuteNonQuery();
                                    }

                                    OpenRoleSpecificForm(role, userId);
                                    this.Hide();
                                }
                                else
                                {
                                    ShowLoginError("Invalid username or password");
                                }
                            }
                            else
                            {
                                ShowLoginError("Invalid username or password or inactive account");
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

        private void OpenRoleSpecificForm(string role, int userId)
        {
            this.Hide();
            try
            {
                Form roleForm = null;

                switch (role.ToLower())
                {
                    case "admin":
                        roleForm = new AdminForm(userId);
                        break;

                    case "owner":
                        roleForm = new OwnerForm(userId);
                        break;

                    case "employee":
                        roleForm = new StaffForm(userId);
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

