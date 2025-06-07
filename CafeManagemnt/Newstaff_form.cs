using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CafeManagemnt
{
    public partial class Newstaff_form : Form
    {
        private ToolTip toolTip; // Tooltip instance for displaying input hints

        public Newstaff_form()
        {
            try
            {
                InitializeComponent();

                // Attach validation handlers for input fields
                txtFirstname.KeyPress += TextBoxLettersOnly_KeyPress;
                txtLastname.KeyPress += TextBoxLettersOnly_KeyPress;
                txtUsername.KeyPress += TextBoxEnglishOnly_KeyPress;
                txtPassword.KeyPress += TextBoxEnglishOnly_KeyPress;
                txtPhone.KeyPress += TextBoxNumbersOnly_KeyPress;
                txtEmail.KeyPress += TextBoxEnglishOnly_KeyPress;
                txtSalary.KeyPress += TextBoxDecimalOnly_KeyPress;
                txtAddress.KeyPress += TextBoxEnglishOnly_KeyPress;

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Main logic for validating inputs and inserting data into the database
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

            // Collect input errors
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(firstname)) errors.Add("First name is required.");
            if (string.IsNullOrWhiteSpace(lastname)) errors.Add("Last name is required.");
            if (string.IsNullOrWhiteSpace(username)) errors.Add("Username is required.");
            if (string.IsNullOrWhiteSpace(password)) errors.Add("Password is required.");
            if (password.Length < 8) errors.Add("Password must be at least 8 characters.");
            if (!string.IsNullOrEmpty(phone) && !Regex.IsMatch(phone, @"^\d+$")) errors.Add("Phone number must contain only digits.");
            if (!string.IsNullOrEmpty(email) && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) errors.Add("Invalid email format.");
            if (!decimal.TryParse(salaryText, out decimal salary) || salary < 0) errors.Add("Salary must be a valid non-negative number.");

            // English-only checks
            if (!Regex.IsMatch(firstname, @"^[a-zA-Z\s]*$")) errors.Add("First name must contain only English letters and spaces.");
            if (!Regex.IsMatch(lastname, @"^[a-zA-Z\s]*$")) errors.Add("Last name must contain only English letters and spaces.");
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9]*$")) errors.Add("Username must contain only English letters and digits.");
            if (!Regex.IsMatch(password, @"^[a-zA-Z0-9@._\-]*$")) errors.Add("Password must contain only English letters, digits, and special characters (@, ., _, -).");
            if (!string.IsNullOrEmpty(address) && !Regex.IsMatch(address, @"^[a-zA-Z0-9\s,.-]*$")) errors.Add("Address must contain only English letters, digits, spaces, commas, dots, or hyphens.");

            // Show errors if any
            if (errors.Any())
            {
                MessageBox.Show(string.Join("\n", errors), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hash password
            string hashedPassword = PasswordHelper.HashPassword(password);
            int roleId = 3; // Employee role

            string connectionString = @"Data Source=.;Initial Catalog=CafeManagementDB;Integrated Security=True;Encrypt=False";

            // Insert data into database with transaction
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Check for unique username and email
                            string checkQuery = @"
                                SELECT COUNT(*) FROM users WHERE username = @Username
                                UNION ALL
                                SELECT COUNT(*) FROM employees WHERE username = @Username OR email = @Email";
                            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@Username", username);
                                checkCmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                                using (SqlDataReader reader = checkCmd.ExecuteReader())
                                {
                                    int userCount = 0, employeeCount = 0;
                                    if (reader.Read()) userCount = reader.GetInt32(0);
                                    if (reader.NextResult() && reader.Read()) employeeCount = reader.GetInt32(0);
                                    if (userCount > 0 || employeeCount > 0)
                                    {
                                        MessageBox.Show("Username or email already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }

                            // Insert into users table
                            string insertUserQuery = @"
                                INSERT INTO users (username, password_hash, role_id, is_active)
                                OUTPUT INSERTED.user_id
                                VALUES (@Username, @PasswordHash, @RoleId, 1);";

                            using (SqlCommand userCmd = new SqlCommand(insertUserQuery, conn, transaction))
                            {
                                userCmd.Parameters.AddWithValue("@Username", username);
                                userCmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                                userCmd.Parameters.AddWithValue("@RoleId", roleId);

                                int userId = Convert.ToInt32(userCmd.ExecuteScalar());

                                // Insert into employees table
                                string insertEmployeeQuery = @"
                                    INSERT INTO employees (user_id, first_name, last_name, username, password_hash, phone_number, email, salary, address, role_id)
                                    VALUES (@UserId, @FirstName, @LastName, @Username, @Password, @Phone, @Email, @Salary, @Address, @RoleId);";

                                using (SqlCommand cmd = new SqlCommand(insertEmployeeQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@UserId", userId);
                                    cmd.Parameters.AddWithValue("@FirstName", firstname);
                                    cmd.Parameters.AddWithValue("@LastName", lastname);
                                    cmd.Parameters.AddWithValue("@Username", username);
                                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                                    cmd.Parameters.AddWithValue("@Phone", (object)phone ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Salary", salary);
                                    cmd.Parameters.AddWithValue("@Address", (object)address ?? DBNull.Value);
                                    cmd.Parameters.AddWithValue("@RoleId", roleId);

                                    int employeeResult = cmd.ExecuteNonQuery();

                                    if (employeeResult > 0)
                                    {
                                        transaction.Commit();
                                        MessageBox.Show("Employee and user account added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        ClearFields();
                                        this.Close();
                                    }
                                    else
                                    {
                                        throw new Exception("Failed to insert into employees table.");
                                    }
                                }
                            }
                        }
                        catch
                        {
                            transaction.Rollback(); // Roll back changes on failure
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Closes the form
        private void Back_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //// Clears all input fields
        private void ClearFields()
        {
            txtFirstname.Text = "";
            txtLastname.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtSalary.Text = "";
            txtAddress.Text = "";
        }
    }
}
