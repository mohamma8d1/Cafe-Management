using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using UserManagementSystem;

namespace CafeManagemnt
{
    public partial class OwnerForm : Form
    {
        private DataTable _dataTable; // Stores user data for DataGridView
        private DataTable _inventoryDataTable; // Stores inventory data for DataGridView
        private readonly int _userId; // Stores the current user's ID
        private const string ConnectionString = @"Data Source=.;Initial Catalog=CafeManagementDB;Integrated Security=True;Encrypt=False"; // Database connection string

        public OwnerForm(int userId)
        {
            InitializeComponent(); // Initialize form components
            _userId = userId; // Set user ID
            this.FormClosing += OwnerForm_FormClosing; // Attach form closing event handler
            dataGridView1.Visible = false; // Initially hide DataGridView
            Backtostaff.Visible = false;
        }

        private void OwnerForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void LoadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = @"SELECT u.user_id AS 'User ID', u.username AS 'Username', 
                           r.role_name AS 'Role', 
                           CASE WHEN u.is_active = 1 THEN 'Active' ELSE 'Inactive' END AS 'Status',
                           ul.login_time AS 'Last Login'
                           FROM users u
                           INNER JOIN roles r ON u.role_id = r.role_id
                           LEFT JOIN (
                               SELECT user_id, MAX(login_time) AS login_time
                               FROM user_logs
                               GROUP BY user_id
                           ) ul ON u.user_id = ul.user_id
                           WHERE u.role_id != 2";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    _dataTable = new DataTable();
                    adapter.Fill(_dataTable);
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = _dataTable;
                    SetupDataGridView();

                    if (dataGridView1.Columns["Last Login"] != null)
                    {
                        dataGridView1.Columns["Last Login"].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss";
                        dataGridView1.Columns["Last Login"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }

                    if (dataGridView1.Columns["User ID"] != null)
                        dataGridView1.Columns["User ID"].Width = 80;
                    if (dataGridView1.Columns["Username"] != null)
                        dataGridView1.Columns["Username"].Width = 120;
                    if (dataGridView1.Columns["Role"] != null)
                        dataGridView1.Columns["Role"].Width = 100;
                    if (dataGridView1.Columns["Status"] != null)
                        dataGridView1.Columns["Status"].Width = 80;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadInventoryData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = @"SELECT 
                                    p.product_id AS 'Product ID',
                                    p.product_name AS 'Product Name',
                                    p.unit_price AS 'Unit Price',
                                    i.quantity_in_stock AS 'Stock',
                                    ISNULL(SUM(oi.subtotal), 0) AS 'Total Sales'
                                FROM 
                                    products p
                                INNER JOIN 
                                    inventory i ON p.product_id = i.product_id
                                LEFT JOIN 
                                    order_items oi ON p.product_id = oi.product_id
                                GROUP BY 
                                    p.product_id, p.product_name, p.unit_price, i.quantity_in_stock";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    _inventoryDataTable = new DataTable();
                    adapter.Fill(_inventoryDataTable);
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = _inventoryDataTable;
                    SetupDataGridView();

                    if (dataGridView1.Columns["Product ID"] != null)
                        dataGridView1.Columns["Product ID"].Width = 80;
                    if (dataGridView1.Columns["Product Name"] != null)
                        dataGridView1.Columns["Product Name"].Width = 150;
                    if (dataGridView1.Columns["Unit Price"] != null)
                        dataGridView1.Columns["Unit Price"].Width = 100;
                    if (dataGridView1.Columns["Stock"] != null)
                        dataGridView1.Columns["Stock"].Width = 80;
                    if (dataGridView1.Columns["Total Sales"] != null)
                        dataGridView1.Columns["Total Sales"].Width = 100;

                    if (dataGridView1.Columns["Unit Price"] != null)
                    {
                        dataGridView1.Columns["Unit Price"].DefaultCellStyle.Format = "0.00 $";
                        dataGridView1.Columns["Unit Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if (dataGridView1.Columns["Total Sales"] != null)
                    {
                        dataGridView1.Columns["Total Sales"].DefaultCellStyle.Format = "0.00 $";
                        dataGridView1.Columns["Total Sales"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading inventory data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStaffData(bool includeEditDisableButtons = false)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = @"SELECT u.user_id AS 'User ID', u.username AS 'Username', 
                                   e.salary AS 'Salary', 
                                   ul.login_time AS 'Last Login', 
                                   ul.logout_time AS 'Last Logout'
                                   FROM users u
                                   INNER JOIN employees e ON u.user_id = e.user_id
                                   LEFT JOIN (
                                       SELECT user_id, 
                                              MAX(login_time) AS login_time,
                                              MAX(logout_time) AS logout_time
                                       FROM user_logs
                                       GROUP BY user_id
                                   ) ul ON u.user_id = ul.user_id
                                   WHERE u.is_active = 1";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    _dataTable = new DataTable();
                    adapter.Fill(_dataTable);
                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = _dataTable;

                    if (includeEditDisableButtons)
                    {
                        DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn
                        {
                            Name = "Edit",
                            HeaderText = "Edit",
                            Text = "Edit",
                            UseColumnTextForButtonValue = true,
                            Width = 80
                        };
                        dataGridView1.Columns.Add(editButtonColumn);

                        DataGridViewButtonColumn disableButtonColumn = new DataGridViewButtonColumn
                        {
                            Name = "Fire",
                            HeaderText = "Fire",
                            Text = "Fire",
                            UseColumnTextForButtonValue = true,
                            Width = 80
                        };
                        dataGridView1.Columns.Add(disableButtonColumn);
                    }

                    SetupDataGridView();

                    if (dataGridView1.Columns["Last Login"] != null)
                    {
                        dataGridView1.Columns["Last Login"].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss";
                        dataGridView1.Columns["Last Login"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dataGridView1.Columns["Last Login"].Width = 150;
                    }

                    if (dataGridView1.Columns["Last Logout"] != null)
                    {
                        dataGridView1.Columns["Last Logout"].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss";
                        dataGridView1.Columns["Last Logout"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dataGridView1.Columns["Last Logout"].Width = 150;
                    }

                    if (dataGridView1.Columns["Salary"] != null)
                    {
                        dataGridView1.Columns["Salary"].DefaultCellStyle.Format = "0.00 $";
                        dataGridView1.Columns["Salary"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView1.Columns["Salary"].Width = 100;
                    }

                    if (dataGridView1.Columns["User ID"] != null)
                        dataGridView1.Columns["User ID"].Width = 80;
                    if (dataGridView1.Columns["Username"] != null)
                        dataGridView1.Columns["Username"].Width = 120;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading staff data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridView()
        {
            dataGridView1.RowTemplate.Height = 35;
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(176, 137, 104);
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(176, 137, 104);
            dataGridView1.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(176, 137, 104);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(176, 137, 104);
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 12F);
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Black;
        }

        private void UserShowbtn_Click(object sender, EventArgs e)
        {
            LoadData();
            Newstaff_btn.Visible = false;
            Stafflist_btn.Visible = false;
            EFstaff_btn.Visible = false;
            dataGridView1.Visible = true;
            Backtostaff.Visible = false;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private void InventoryManagement_btn_Click(object sender, EventArgs e)
        {
            LoadInventoryData();
            Newstaff_btn.Visible = false;
            Stafflist_btn.Visible = false;
            EFstaff_btn.Visible = false;
            dataGridView1.Visible = true;
            Backtostaff.Visible = false;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private void StaffManagement_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            Newstaff_btn.Visible = true;
            Stafflist_btn.Visible = true;
            EFstaff_btn.Visible = true;
            Backtostaff.Visible = false;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private void MenuManagement_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private void Report_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private void Newstaff_btn_Click(object sender, EventArgs e)
        {
            Newstaff_form newStaffForm = new Newstaff_form();
            newStaffForm.ShowDialog();
            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private void Stafflist_btn_Click(object sender, EventArgs e)
        {
            LoadStaffData(); // Load without Edit/Fire buttons
            Newstaff_btn.Visible = false;
            Stafflist_btn.Visible = false;
            EFstaff_btn.Visible = false;
            dataGridView1.Visible = true;
            Backtostaff.Visible = true;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private void Editstaff_btn_Click(object sender, EventArgs e)
        {
            LoadStaffData(includeEditDisableButtons: true); // Load with Edit/Fire buttons
            Newstaff_btn.Visible = false;
            Stafflist_btn.Visible = false;
            EFstaff_btn.Visible = false;
            dataGridView1.Visible = true;
            Backtostaff.Visible = true;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
            dataGridView1.CellContentClick += DataGridView1_CellContentClick;
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Ignore header clicks

            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            // Check if User ID column exists and has a valid value
            if (dataGridView1.Rows[e.RowIndex].Cells["User ID"].Value == null)
            {
                MessageBox.Show("Invalid user ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int userId;
            try
            {
                userId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["User ID"].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving user ID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string username = dataGridView1.Rows[e.RowIndex].Cells["Username"].Value?.ToString() ?? "Unknown";

            if (columnName == "Edit")
            {
                try
                {
                    EditStaffForm editForm = new EditStaffForm(userId);
                    editForm.ShowDialog();
                    LoadStaffData(includeEditDisableButtons: true); // Refresh with buttons
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening edit form for user '{username}': {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (columnName == "Fire")
            {
                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to Fire the user '{username}'?",
                    "Confirm Fire",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2
                );

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {
                            string query = @"UPDATE users SET is_active = 0 WHERE user_id = @user_id";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@user_id", userId);
                                connection.Open();
                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show($"User '{username}' Fire successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadStaffData(includeEditDisableButtons: true); // Refresh with buttons
                                }
                                else
                                {
                                    MessageBox.Show($"No user found with ID '{userId}'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error disabling user '{username}': {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Backtostaff_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            Newstaff_btn.Visible = true;
            Stafflist_btn.Visible = true;
            EFstaff_btn.Visible = true;
            Backtostaff.Visible = false;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private void OwnerForm_Load(object sender, EventArgs e)
        {
        }
    }
}