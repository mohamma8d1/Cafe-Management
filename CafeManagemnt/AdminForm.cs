using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UserManagementSystem;

namespace CafeManagemnt
{
    public partial class AdminForm : Form
    {
        private readonly int _userId;
        private const string ConnectionString = @"Data Source=.;Initial Catalog=CafeManagementDB;Integrated Security=True;Encrypt=False";
        private DataTable _dataTable;

        public AdminForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
            this.FormClosing += AdminForm_FormClosing;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadData();
        }

        private void SetupDataGridView()
        {
            // Configure DataGridView for full width and larger appearance
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = false;

            // Make DataGridView full width and height (Dock or Anchor)
            dataGridView1.Dock = DockStyle.Fill; // Fills the entire form
            // Alternatively, use Anchor for more control:
            // dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            // dataGridView1.Width = this.ClientSize.Width - 20; // Leave some margin
            // dataGridView1.Height = this.ClientSize.Height - 50; // Leave space for buttons

            // Increase row height and font size
            dataGridView1.RowTemplate.Height = 40; // Larger row height
            dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 12F); // Larger font
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);

            // Clear existing columns
            dataGridView1.Columns.Clear();

            // Add Username column
            DataGridViewTextBoxColumn usernameColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "username",
                HeaderText = "Username",
                Name = "username",
                Width = 200 // Set a wider column
            };
            dataGridView1.Columns.Add(usernameColumn);

            // Add Role combo box column
            DataGridViewComboBoxColumn roleColumn = new DataGridViewComboBoxColumn
            {
                DataPropertyName = "role_id",
                HeaderText = "Role",
                Name = "role_id",
                DisplayMember = "role_name",
                ValueMember = "role_id",
                Width = 150
            };
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT role_id, role_name FROM roles";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable rolesTable = new DataTable();
                adapter.Fill(rolesTable);
                roleColumn.DataSource = rolesTable;
            }
            dataGridView1.Columns.Add(roleColumn);

            // Add Status combo box column
            DataGridViewComboBoxColumn statusColumn = new DataGridViewComboBoxColumn
            {
                DataPropertyName = "is_active",
                HeaderText = "Status",
                Name = "is_active",
                Width = 100
            };
            statusColumn.Items.AddRange("Active", "Inactive");
            dataGridView1.Columns.Add(statusColumn);

            // Add Save button column
            DataGridViewButtonColumn saveButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "Save",
                UseColumnTextForButtonValue = true,
                Name = "save_button",
                Width = 80
            };
            dataGridView1.Columns.Add(saveButtonColumn);

            // Auto-size columns to fill remaining space
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Handle cell click for the Save button
            dataGridView1.CellClick += DataGridView1_CellClick;
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = @"SELECT u.user_id, u.username, u.role_id, r.role_name, 
                                    CASE WHEN u.is_active = 1 THEN 'Active' ELSE 'Inactive' END AS is_active
                                    FROM users u
                                    INNER JOIN roles r ON u.role_id = r.role_id";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    _dataTable = new DataTable();
                    adapter.Fill(_dataTable);
                    dataGridView1.DataSource = _dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["save_button"].Index)
            {
                SaveRowChanges(e.RowIndex);
            }
        }

        private void SaveRowChanges(int rowIndex)
        {
            try
            {
                DataRow row = _dataTable.Rows[rowIndex];
                int userId = Convert.ToInt32(row["user_id"]);
                string username = row["username"].ToString();
                int roleId = Convert.ToInt32(row["role_id"]);
                bool isActive = row["is_active"].ToString() == "Active";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = @"UPDATE users 
                                    SET username = @username, role_id = @role_id, is_active = @is_active 
                                    WHERE user_id = @user_id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@role_id", roleId);
                        command.Parameters.AddWithValue("@is_active", isActive);
                        command.Parameters.AddWithValue("@user_id", userId);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("User updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserShowbtn_Click(object sender, EventArgs e)
        {
            LoadData();
            
            adduserbtn.Visible = true;
            AddRolebtn.Visible = true;
            dataGridView1.Visible = true;
        }

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to close and log out?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
            {

                e.Cancel = true;
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = @"UPDATE user_logs 
                                   SET logout_time = GETDATE() 
                                   WHERE user_id = @user_id AND logout_time IS NULL";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", _userId);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error logging out: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void adduserbtn_Click(object sender, EventArgs e)
        {
            Form AddUser = new UserRegistrationForm();
            AddUser.Show();
        }

        private void AddRolebtn_Click(object sender, EventArgs e)
        {
            Form AddRole = new RoleCreationForm();
            AddRole.Show();

        }
    }
}