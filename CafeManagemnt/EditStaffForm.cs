using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CafeManagemnt
{
    public partial class EditStaffForm : Form
    {
        private readonly int _userId;
        private const string ConnectionString = @"Data Source=.;Initial Catalog=CafeManagementDB;Integrated Security=True;Encrypt=False";
        private ToolTip toolTip; // Tooltip instance for displaying input hints

        public EditStaffForm(int userId)
        {
            try
            {
                InitializeComponent();
                _userId = userId;

                // Initialize and configure tooltips
                toolTip = new ToolTip();
                toolTip.SetToolTip(txtFirstname, "Enter only English letters and spaces.");
                toolTip.SetToolTip(txtLastname, "Enter only English letters and spaces.");
                toolTip.SetToolTip(txtUsername, "Enter only English letters and digits.");
                toolTip.SetToolTip(txtPassword, "Enter English letters, digits, and special characters (@, ., _, -). Minimum 8 characters.");
                toolTip.SetToolTip(txtPhone, "Enter only digits.");
                toolTip.SetToolTip(txtEmail, "Enter a valid email (e.g., example@domain.com).");
                toolTip.SetToolTip(txtSalary, "Enter a non-negative number (e.g., 1000.50).");
                toolTip.SetToolTip(txtAddress, "Enter English letters, digits, spaces, commas, dots, or hyphens.");

                // Attach validation handlers for input fields
                txtFirstname.KeyPress += TextBoxLettersOnly_KeyPress;
                txtLastname.KeyPress += TextBoxLettersOnly_KeyPress;
                txtUsername.KeyPress += TextBoxEnglishOnly_KeyPress;
                txtPassword.KeyPress += TextBoxEnglishOnly_KeyPress;
                txtPhone.KeyPress += TextBoxNumbersOnly_KeyPress;
                txtEmail.KeyPress += TextBoxEnglishOnly_KeyPress;
                txtSalary.KeyPress += TextBoxDecimalOnly_KeyPress;
                txtAddress.KeyPress += TextBoxEnglishOnly_KeyPress;

                LoadStaffData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStaffData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = @"SELECT e.first_name, e.last_name, e.username, e.password_hash, 
                                   e.phone_number, e.email, e.address, e.salary, u.is_active
                                   FROM employees e
                                   INNER JOIN users u ON e.user_id = u.user_id
                                   WHERE e.user_id = @user_id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", _userId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtFirstname.Text = reader["first_name"].ToString();
                                txtLastname.Text = reader["last_name"].ToString();
                                txtUsername.Text = reader["username"].ToString();
                                txtPassword.Text = ""; // Do not display hashed password
                                txtPhone.Text = reader["phone_number"].ToString();
                                txtEmail.Text = reader["email"].ToString();
                                txtAddress.Text = reader["address"].ToString();
                                txtSalary.Text = reader["salary"] != DBNull.Value ? Convert.ToDecimal(reader["salary"]).ToString("0.00") : "";
                                chkActive.Checked = Convert.ToBoolean(reader["is_active"]);
                            }
                            else
                            {
                                MessageBox.Show("No staff data found for the specified user ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading staff data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Allow only English letters and spaces
        private void TextBoxLettersOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"^[a-zA-Z\s]*$") && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                toolTip.Show("Use only English letters and spaces.", sender as TextBox, 0, -20, 2000);
            }
        }

        // Allow only English letters, digits, and selected special characters
        private void TextBoxEnglishOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"^[a-zA-Z0-9@._\-]*$") && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                toolTip.Show("Use only English letters, digits, and special characters (@, ., _, -).", sender as TextBox, 0, -20, 2000);
            }
        }

        // Allow only digits
        private void TextBoxNumbersOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                toolTip.Show("Use only digits.", sender as TextBox, 0, -20, 2000);
            }
        }

        // Allow decimal numbers (only one dot)
        private void TextBoxDecimalOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                toolTip.Show("Use only digits or one decimal point.", textBox, 0, -20, 2000);
            }
            else if (e.KeyChar == '.' && textBox.Text.Contains("."))
            {
                e.Handled = true;
                toolTip.Show("Only one decimal point is allowed.", textBox, 0, -20, 2000);
            }
        }

        private void Apply_btn_Click(object sender, EventArgs e)
        {
            // Read inputs
            string firstname = txtFirstname.Text.Trim();
            string lastname = txtLastname.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string salaryText = txtSalary.Text.Trim();
            string address = txtAddress.Text.Trim();
            bool isActive = chkActive.Checked;

            // Collect input errors
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(firstname)) errors.Add("First name is required.");
            if (string.IsNullOrWhiteSpace(lastname)) errors.Add("Last name is required.");
            if (string.IsNullOrWhiteSpace(username)) errors.Add("Username is required.");
            if (string.IsNullOrWhiteSpace(password)) errors.Add("Password is required.");
            if (password.Length < 8) errors.Add("Password must be at least 8 characters.");
            if (!string.IsNullOrEmpty(phone) && !Regex.IsMatch(phone, @"^\d+$")) errors.Add("Phone number must contain only digits.");
            if (!string.IsNullOrEmpty(email) && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) errors.Add("Invalid email format.");
            if (!string.IsNullOrWhiteSpace(salaryText) && (!decimal.TryParse(salaryText, out decimal salary) || salary < 0)) errors.Add("Salary must be a valid non-negative number.");

            // English-only checks
            if (!Regex.IsMatch(firstname, @"^[a-zA-Z\s]*$")) errors.Add("First name must contain only English letters and spaces.");
            if (!Regex.IsMatch(lastname, @"^[a-zA-Z\s]*$")) errors.Add("Last name must contain only English letters and spaces.");
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9]*$")) errors.Add("Username must contain only English letters and digits.");
            if (!Regex.IsMatch(password, @"^[a-zA-Z0-9@._\-]*$")) errors.Add("Password must contain only English letters, digits, and special characters (@, ., _, -).");
            if (!string.IsNullOrEmpty(address) && !Regex.IsMatch(address, @"^[a-zA-Z0-9\s,.-]*$")) errors.Add("Address must contain only English letters, digits, spaces, commas, dots, or hyphens.");

            // Check for unique username and email (excluding current user)
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string checkQuery = @"
                        SELECT COUNT(*) FROM users WHERE username = @Username AND user_id != @UserId
                        UNION ALL
                        SELECT COUNT(*) FROM employees WHERE (username = @Username OR email = @Email) AND user_id != @UserId";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", username);
                        checkCmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                        checkCmd.Parameters.AddWithValue("@UserId", _userId);
                        using (SqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            int userCount = 0, employeeCount = 0;
                            if (reader.Read()) userCount = reader.GetInt32(0);
                            if (reader.NextResult() && reader.Read()) employeeCount = reader.GetInt32(0);
                            if (userCount > 0 || employeeCount > 0)
                            {
                                errors.Add("Username or email already exists.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add($"Error checking username/email uniqueness: {ex.Message}");
            }

            // Show errors if any
            if (errors.Any())
            {
                MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hash password
            string hashedPassword = PasswordHelper.HashPassword(password);

            // Update data in database with transaction
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        // Update employees table
                        string employeeQuery = @"UPDATE employees 
                                                SET first_name = @first_name, last_name = @last_name, 
                                                    username = @username, password_hash = @password_hash,
                                                    phone_number = @phone_number, email = @email, 
                                                    address = @address, salary = @salary
                                                WHERE user_id = @user_id";
                        using (SqlCommand command = new SqlCommand(employeeQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@user_id", _userId);
                            command.Parameters.AddWithValue("@first_name", firstname);
                            command.Parameters.AddWithValue("@last_name", lastname);
                            command.Parameters.AddWithValue("@username", username);
                            command.Parameters.AddWithValue("@password_hash", hashedPassword);
                            command.Parameters.AddWithValue("@phone_number", (object)phone ?? DBNull.Value);
                            command.Parameters.AddWithValue("@email", (object)email ?? DBNull.Value);
                            command.Parameters.AddWithValue("@address", (object)address ?? DBNull.Value);
                            command.Parameters.AddWithValue("@salary", string.IsNullOrWhiteSpace(salaryText) ? (object)DBNull.Value : decimal.Parse(salaryText));
                            int employeeRowsAffected = command.ExecuteNonQuery();
                            if (employeeRowsAffected == 0)
                            {
                                throw new Exception("No employee found with the specified user ID.");
                            }
                        }

                        // Update users table for username, password_hash, and is_active
                        string userQuery = @"UPDATE users 
                                            SET username = @username, password_hash = @password_hash, is_active = @is_active 
                                            WHERE user_id = @user_id";
                        using (SqlCommand command = new SqlCommand(userQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@user_id", _userId);
                            command.Parameters.AddWithValue("@username", username);
                            command.Parameters.AddWithValue("@password_hash", hashedPassword);
                            command.Parameters.AddWithValue("@is_active", isActive);
                            int userRowsAffected = command.ExecuteNonQuery();
                            if (userRowsAffected == 0)
                            {
                                throw new Exception("No user found with the specified user ID.");
                            }
                        }

                        transaction.Commit();
                        MessageBox.Show("Staff updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating staff: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Back_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditStaffForm_Load(object sender, EventArgs e)
        {

        }
    }
}