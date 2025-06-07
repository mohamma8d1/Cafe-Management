using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UserManagementSystem
{
    public partial class RoleCreationForm : Form
    {
        private string connectionString = @"Data Source=.;Initial Catalog=CafemanagementDB;Integrated Security=True;Encrypt=False";

        public RoleCreationForm()
        {
            InitializeComponent();
        }

        private void RoleCreationForm_Load(object sender, EventArgs e)
        {
            LoadRoles();
        }

        private void LoadRoles()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT role_id, role_name FROM roles ORDER BY role_name";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable roleTable = new DataTable();
                    adapter.Fill(roleTable);

                    rolesDataGridView.DataSource = roleTable;

                    // Optionally rename the columns for better display
                    if (rolesDataGridView.Columns.Count > 0)
                    {
                        rolesDataGridView.Columns["role_id"].HeaderText = "ID";
                        rolesDataGridView.Columns["role_name"].HeaderText = "Role Name";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading roles: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoleName.Text))
            {
                MessageBox.Show("Role name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if role name already exists
                    string checkQuery = "SELECT COUNT(*) FROM roles WHERE role_name = @RoleName";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@RoleName", txtRoleName.Text);
                    int roleCount = (int)checkCommand.ExecuteScalar();

                    if (roleCount > 0)
                    {
                        MessageBox.Show("Role name already exists.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Insert new role
                    string insertQuery = "INSERT INTO roles (role_name) VALUES (@RoleName)";
                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@RoleName", txtRoleName.Text);
                    command.ExecuteNonQuery();

                    txtRoleName.Text = "";
                    LoadRoles();

                    MessageBox.Show("Role added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding role: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteRole_Click(object sender, EventArgs e)
        {
            if (rolesDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a role to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int roleId = Convert.ToInt32(rolesDataGridView.SelectedRows[0].Cells["role_id"].Value);
            string roleName = rolesDataGridView.SelectedRows[0].Cells["role_name"].Value.ToString();

            // Confirm deletion
            DialogResult result = MessageBox.Show($"Are you sure you want to delete the role '{roleName}'?",
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if role is in use by any users
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE role_id = @RoleId";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@RoleId", roleId);
                    int userCount = (int)checkCommand.ExecuteScalar();

                    if (userCount > 0)
                    {
                        MessageBox.Show($"Cannot delete role '{roleName}' because it is assigned to {userCount} user(s).",
                            "Delete Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Delete the role
                    string deleteQuery = "DELETE FROM roles WHERE role_id = @RoleId";
                    SqlCommand command = new SqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@RoleId", roleId);
                    command.ExecuteNonQuery();

                    LoadRoles();

                    MessageBox.Show("Role deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting role: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}