using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;


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
        }

        private void OwnerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Show confirmation dialog for closing the form
            DialogResult result = MessageBox.Show(
                "Are you sure you want to close and log out?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
            {
                // Cancel form closing if user selects No
                e.Cancel = true;
                return;
            }

            // Log logout time in the database
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
                // Show error message if logging out fails
                MessageBox.Show($"Error logging out: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            // Load user data from the database
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
                    dataGridView1.DataSource = _dataTable;
                    SetupDataGridView();

                    // Format the Last Login column
                    if (dataGridView1.Columns["Last Login"] != null)
                    {
                        dataGridView1.Columns["Last Login"].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss";
                        dataGridView1.Columns["Last Login"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }

                    // Set column widths
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
                // Show error message if data loading fails
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadInventoryData()
        {
            // Load inventory data from the database
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
                    dataGridView1.DataSource = _inventoryDataTable;
                    SetupDataGridView();

                    // Set column widths for inventory
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

                    // display with $
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
                // Show error message if inventory data loading fails
                MessageBox.Show("Failed to load inventory data. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridView()
        {
            // Set row height
            dataGridView1.RowTemplate.Height = 35;

            // Set default cell styles
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Set alternating row styles
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(176, 137, 104);
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(176, 137, 104);
            dataGridView1.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.Black;

            // Set header styles
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(176, 137, 104);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(176, 137, 104);
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            // Set other DataGridView properties
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
            // Load and display user data
            LoadData();
            dataGridView1.Visible = true;
        }

        private void InventoryManagement_btn_Click(object sender, EventArgs e)
        {
            // Load and display inventory data
            LoadInventoryData();
            dataGridView1.Visible = true;
        }

        private void StaffManagement_btn_Click(object sender, EventArgs e)
        {

        }

        private void MenuManagement_btn_Click(object sender, EventArgs e)
        {

        }

        private void Report_btn_Click(object sender, EventArgs e)
        {

        }

    }
}