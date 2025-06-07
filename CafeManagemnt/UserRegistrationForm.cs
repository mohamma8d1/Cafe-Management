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
using System.Security.Cryptography;
using CafeManagemnt;

namespace UserManagementSystem
{
    public partial class UserRegistrationForm : Form
    {
        private const string ConnectionString = @"Data Source=.;Initial Catalog=CafemanagementDB;Integrated Security=True;Encrypt=False";
        private List<Role> availableRoles = new List<Role>();

        public UserRegistrationForm()
        {
            InitializeComponent();
        }

        private void UserRegistrationForm_Load(object sender, EventArgs e)
        {
            // Set default state for UI elements
            isActiveCheckBox.Checked = true;

            // Load roles from database
            LoadRoles();
        }

        private void LoadRoles()
        {
            try
            {
                // Clear existing items
                roleComboBox.Items.Clear();
                availableRoles.Clear();

                // Get roles from database
                string query = "SELECT role_id, role_name FROM roles";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int roleId = Convert.ToInt32(reader["role_id"]);
                            string roleName = reader["role_name"].ToString();

                            Role role = new Role { RoleId = roleId, RoleName = roleName };
                            availableRoles.Add(role);
                            roleComboBox.Items.Add(roleName);
                        }
                    }
                }

                // Select first role by default if any exist
                if (roleComboBox.Items.Count > 0)
                {
                    roleComboBox.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("No roles found in the database. Please create at least one role first.",
                        "No Roles Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnRegister.Enabled = false; // Disable registration without roles
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading roles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Please enter a username", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please enter a password", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (roleComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a role", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if username already exists
                if (UsernameExists(txtUsername.Text))
                {
                    MessageBox.Show("Username already exists. Please choose a different username.",
                        "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Create user
                if (CreateUser())
                {
                    MessageBox.Show("User registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Failed to register user. Please try again.",
                        "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool UsernameExists(string username)
        {
            try
            {
                string query = "SELECT COUNT(1) FROM users WHERE username = @username";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking username: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Assume username doesn't exist if there's an error
            }
        }

        private bool CreateUser()
        {
            try
            {
                string roleName = roleComboBox.SelectedItem.ToString();
                int roleId = availableRoles.First(r => r.RoleName == roleName).RoleId;
                bool isActive = isActiveCheckBox.Checked;
                string hashedPassword = PasswordHelper.HashPassword(txtPassword.Text);

                string query = @"INSERT INTO users (username, password_hash, role_id, is_active) 
                             VALUES (@username, @passwordHash, @roleId, @isActive)";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    command.Parameters.AddWithValue("@passwordHash", hashedPassword);
                    command.Parameters.AddWithValue("@roleId", roleId);
                    command.Parameters.AddWithValue("@isActive", isActive);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private string HashPassword(string password)
        {
            // Using SHA256 for password hashing
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the password string to a byte array
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Compute the hash
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Convert the hash bytes to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private void ClearForm()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            isActiveCheckBox.Checked = true;

            // Select first role by default if any exist
            if (roleComboBox.Items.Count > 0)
            {
                roleComboBox.SelectedIndex = 0;
            }

            txtUsername.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    // Helper class for roles
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}