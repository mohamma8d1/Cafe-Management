using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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
                                   WHERE u.is_active = 1 AND u.role_id != 2";

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

        private void ShowReportOptions()
        {
            // Hide other controls
            dataGridView1.Visible = false;
            Newstaff_btn.Visible = false;
            Stafflist_btn.Visible = false;
            EFstaff_btn.Visible = false;
            Backtostaff.Visible = false;
            Additem_btn.Visible = false;

            // Create or show report panel
            if (!Controls.Contains(reportPanel))
            {
                CreateReportPanel();
            }

            // Show report panel and reset selection
            reportPanel.Visible = true;
            reportTypeComboBox.SelectedIndex = 0;
        }

        private void CreateReportPanel()
        {
            // Create main report panel with same positioning as dataGridView1
            reportPanel = new Panel
            {
                BackColor = Color.FromArgb(176, 137, 104),
                Location = dataGridView1.Location,
                Name = "reportPanel",
                Size = dataGridView1.Size,
                Visible = false
            };

            // Create report type label
            Label reportTypeLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Calibri", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(67, 40, 24),
                Location = new Point(20, 20),
                Text = "Select Report Type:"
            };

            // Create report type combo box
            reportTypeComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 14F),
                Location = new Point(250, 20),
                Size = new Size(250, 30),
                Name = "reportTypeComboBox"
            };

            // Add report types
            reportTypeComboBox.Items.AddRange(new object[] {
                "Sales Report",
                "Inventory Report",
                "User Activity Report",
                "Popular Items Report"
            });

            // Create date range controls
            Label fromDateLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Calibri", 14F),
                ForeColor = Color.FromArgb(67, 40, 24),
                Location = new Point(20, 70),
                Text = "From Date:"
            };

            fromDatePicker = new DateTimePicker
            {
                Font = new Font("Arial", 12F),
                Format = DateTimePickerFormat.Short,
                Location = new Point(120, 70),
                Size = new Size(150, 30),
                Value = DateTime.Now.AddDays(-30)
            };

            Label toDateLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Calibri", 14F),
                ForeColor = Color.FromArgb(67, 40, 24),
                Location = new Point(300, 70),
                Text = "To Date:"
            };

            toDatePicker = new DateTimePicker
            {
                Font = new Font("Arial", 12F),
                Format = DateTimePickerFormat.Short,
                Location = new Point(380, 70),
                Size = new Size(150, 30),
                Value = DateTime.Now
            };

            // Create generate report button
            generateReportButton = new Controls.MainButton
            {
                BackColor = Color.FromArgb(67, 40, 24),
                BackgroundColor = Color.FromArgb(67, 40, 24),
                BorderColor = Color.White,
                BorderRadius = 20,
                BorderSize = 0,
                FlatAppearance = { BorderSize = 0 },
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(550, 65),
                Name = "generateReportButton",
                Size = new Size(120, 40),
                Text = "Generate",
                TextColor = Color.White,
                UseVisualStyleBackColor = false
            };
            generateReportButton.Click += GenerateReportButton_Click;

            // Create report data grid view
            reportDataGridView = new DataGridView
            {
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Location = new Point(20, 120),
                Name = "reportDataGridView",
                Size = new Size(1059, 450),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                RowTemplate = { Height = 30 },
                DefaultCellStyle = { Font = new Font("Arial", 10F) },
                ColumnHeadersDefaultCellStyle = { Font = new Font("Arial", 10F, FontStyle.Bold) },
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false
            };

            // Add controls to report panel
            reportPanel.Controls.Add(reportTypeLabel);
            reportPanel.Controls.Add(reportTypeComboBox);
            reportPanel.Controls.Add(fromDateLabel);
            reportPanel.Controls.Add(fromDatePicker);
            reportPanel.Controls.Add(toDateLabel);
            reportPanel.Controls.Add(toDatePicker);
            reportPanel.Controls.Add(generateReportButton);
            reportPanel.Controls.Add(reportDataGridView);

            // Add report panel to form
            Controls.Add(reportPanel);
        }

        private void GenerateReportButton_Click(object sender, EventArgs e)
        {
            if (reportTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a report type.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string reportType = reportTypeComboBox.SelectedItem.ToString();
            DateTime fromDate = fromDatePicker.Value.Date;
            DateTime toDate = toDatePicker.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            if (fromDate > toDate)
            {
                MessageBox.Show("From Date cannot be after To Date.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                switch (reportType)
                {
                    case "Sales Report":
                        GenerateSalesReport(fromDate, toDate);
                        break;
                    case "Inventory Report":
                        GenerateInventoryReport(fromDate, toDate);
                        break;
                    case "User Activity Report":
                        GenerateUserActivityReport(fromDate, toDate);
                        break;
                    case "Popular Items Report":
                        GeneratePopularItemsReport(fromDate, toDate);
                        break;
                    default:
                        MessageBox.Show("Invalid report type selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

                LogOperation("Report Generation", $"Generated {reportType} from {fromDate.ToShortDateString()} to {toDate.ToShortDateString()}");
            }
            catch (Exception ex)
            {
                LogError(ex);
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void GenerateSalesReport(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = @"
                    SELECT 
                        CONVERT(date, o.order_date) AS Date,
                        COUNT(DISTINCT o.order_id) AS TotalOrders,
                        SUM(oi.quantity * oi.unit_price) AS TotalSales,
                        AVG(oi.quantity * oi.unit_price) AS AverageOrderValue
                    FROM 
                        orders o
                        INNER JOIN order_items oi ON o.order_id = oi.order_id
                    WHERE 
                        o.order_date BETWEEN @fromDate AND @toDate
                    GROUP BY 
                        CONVERT(date, o.order_date)
                    ORDER BY 
                        Date";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@fromDate", fromDate);
                adapter.SelectCommand.Parameters.AddWithValue("@toDate", toDate);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    reportDataGridView.DataSource = dataTable;

                    if (reportDataGridView.Columns["TotalSales"] != null)
                        reportDataGridView.Columns["TotalSales"].DefaultCellStyle.Format = "C";
                    if (reportDataGridView.Columns["AverageOrderValue"] != null)
                        reportDataGridView.Columns["AverageOrderValue"].DefaultCellStyle.Format = "C";
                }
                else
                {
                    MessageBox.Show("No sales data found for the selected date range.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reportDataGridView.DataSource = null;
                }
            }
        }

        private void GenerateInventoryReport(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = @"
                    SELECT 
                        COALESCE(p.product_name, 'Product ID: ' + CAST(i.product_id AS VARCHAR(10))) AS ProductName,
                        i.quantity_in_stock AS CurrentStock,
                        i.minimum_stock_level AS ReorderLevel,
                        CASE 
                            WHEN i.quantity_in_stock <= i.minimum_stock_level THEN 'Reorder Required'
                            WHEN i.quantity_in_stock <= (i.minimum_stock_level * 1.5) THEN 'Low Stock'
                            ELSE 'In Stock'
                        END AS Status,
                        FORMAT(COALESCE(p.unit_price, 0), 'C') AS UnitPrice
                    FROM 
                        inventory i
                        LEFT JOIN products p ON i.product_id = p.product_id
                    WHERE i.is_active = 1
                    ORDER BY 
                        CASE 
                            WHEN i.quantity_in_stock <= i.minimum_stock_level THEN 1
                            WHEN i.quantity_in_stock <= (i.minimum_stock_level * 1.5) THEN 2
                            ELSE 3
                        END,
                        p.product_name";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    reportDataGridView.DataSource = dataTable;

                    reportDataGridView.CellFormatting -= ReportDataGridView_CellFormatting;
                    reportDataGridView.CellFormatting += ReportDataGridView_CellFormatting;

                    reportDataGridView.Columns["ProductName"].HeaderText = "Product Name";
                    reportDataGridView.Columns["CurrentStock"].HeaderText = "Current Stock";
                    reportDataGridView.Columns["ReorderLevel"].HeaderText = "Reorder Level";
                    reportDataGridView.Columns["Status"].HeaderText = "Status";
                    reportDataGridView.Columns["UnitPrice"].HeaderText = "Unit Price";
                }
                else
                {
                    MessageBox.Show("No inventory data found.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reportDataGridView.DataSource = null;
                }
            }
        }

        private void ReportDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (reportDataGridView.Columns[e.ColumnIndex].Name == "Status" && e.RowIndex >= 0)
            {
                string status = e.Value?.ToString();
                switch (status)
                {
                    case "Reorder Required":
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                        break;
                    case "Low Stock":
                        e.CellStyle.ForeColor = Color.Orange;
                        e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                        break;
                    case "In Stock":
                        e.CellStyle.ForeColor = Color.Green;
                        break;
                }
            }
        }

        private void GenerateUserActivityReport(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = @"
                    SELECT 
                        u.username AS Username,
                        r.role_name AS Role,
                        COUNT(ul.log_id) AS LoginCount,
                        MIN(ul.login_time) AS FirstLogin,
                        MAX(ul.login_time) AS LastLogin,
                        ISNULL(AVG(CAST(DATEDIFF(MINUTE, ul.login_time, 
                            CASE WHEN ul.logout_time IS NULL THEN GETDATE() ELSE ul.logout_time END) AS FLOAT)), 0) AS AvgSessionMinutes
                    FROM 
                        users u
                        INNER JOIN roles r ON u.role_id = r.role_id
                        LEFT JOIN user_logs ul ON u.user_id = ul.user_id AND ul.login_time BETWEEN @fromDate AND @toDate
                    WHERE 
                        u.role_id != 2
                    GROUP BY 
                        u.username, r.role_name
                    ORDER BY 
                        LoginCount DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@fromDate", fromDate);
                adapter.SelectCommand.Parameters.AddWithValue("@toDate", toDate);

                DataTable dataTable = new DataTable();

                try
                {
                    adapter.Fill(dataTable);
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("Invalid object name") || ex.Message.Contains("Invalid column name"))
                    {
                        query = @"
                            SELECT 
                                u.username AS Username,
                                r.role_name AS Role,
                                0 AS LoginCount,
                                NULL AS FirstLogin,
                                NULL AS LastLogin,
                                0 AS AvgSessionMinutes
                            FROM 
                                users u
                                INNER JOIN roles r ON u.role_id = r.role_id
                            WHERE 
                                u.role_id != 2
                            ORDER BY 
                                u.username";

                        adapter = new SqlDataAdapter(query, connection);
                        adapter.Fill(dataTable);
                    }
                    else
                    {
                        throw;
                    }
                }

                if (dataTable.Rows.Count > 0)
                {
                    DataTable formattedTable = new DataTable();
                    formattedTable.Columns.Add("Username", typeof(string));
                    formattedTable.Columns.Add("Role", typeof(string));
                    formattedTable.Columns.Add("LoginCount", typeof(int));
                    formattedTable.Columns.Add("FirstLogin", typeof(string));
                    formattedTable.Columns.Add("LastLogin", typeof(string));
                    formattedTable.Columns.Add("AvgSessionTime", typeof(string));

                    foreach (DataRow row in dataTable.Rows)
                    {
                        DataRow newRow = formattedTable.NewRow();
                        newRow["Username"] = row["Username"];
                        newRow["Role"] = row["Role"];
                        newRow["LoginCount"] = row["LoginCount"];

                        newRow["FirstLogin"] = row["FirstLogin"] == DBNull.Value ? "N/A" :
                            Convert.ToDateTime(row["FirstLogin"]).ToString("yyyy-MM-dd HH:mm");
                        newRow["LastLogin"] = row["LastLogin"] == DBNull.Value ? "N/A" :
                            Convert.ToDateTime(row["LastLogin"]).ToString("yyyy-MM-dd HH:mm");

                        string sessionTime = "N/A";
                        if (row["AvgSessionMinutes"] != DBNull.Value)
                        {
                            string avgSessionStr = row["AvgSessionMinutes"].ToString();
                            if (double.TryParse(avgSessionStr, out double minutes) && minutes > 0)
                            {
                                TimeSpan timeSpan = TimeSpan.FromMinutes(minutes);
                                sessionTime = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}";
                            }
                            else
                            {
                                sessionTime = "00:00";
                            }
                        }
                        newRow["AvgSessionTime"] = sessionTime;

                        formattedTable.Rows.Add(newRow);
                    }

                    reportDataGridView.DataSource = formattedTable;
                }
                else
                {
                    MessageBox.Show("No user activity data found for the selected date range.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reportDataGridView.DataSource = null;
                }
            }
        }

        private void GeneratePopularItemsReport(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = @"
                    SELECT 
                        p.product_name AS Item,
                        SUM(oi.quantity) AS TotalQuantitySold,
                        SUM(oi.quantity * oi.unit_price) AS TotalRevenue,
                        COUNT(DISTINCT o.order_id) AS OrderCount,
                        CAST(COUNT(DISTINCT o.order_id) * 100.0 / 
                            (SELECT COUNT(DISTINCT order_id) FROM orders WHERE order_date BETWEEN @fromDate AND @toDate) 
                            AS DECIMAL(5,2)) AS OrderPercentage
                    FROM 
                        order_items oi
                        INNER JOIN orders o ON oi.order_id = o.order_id
                        INNER JOIN products p ON oi.product_id = p.product_id
                    WHERE 
                        o.order_date BETWEEN @fromDate AND @toDate
                    GROUP BY 
                        p.product_name
                    ORDER BY 
                        TotalQuantitySold DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@fromDate", fromDate);
                adapter.SelectCommand.Parameters.AddWithValue("@toDate", toDate);

                DataTable dataTable = new DataTable();

                try
                {
                    adapter.Fill(dataTable);
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("Invalid object name") || ex.Message.Contains("Invalid column name"))
                    {
                        query = @"
                            SELECT 
                                'Product ' + CAST(oi.product_id AS VARCHAR(10)) AS Item,
                                SUM(oi.quantity) AS TotalQuantitySold,
                                SUM(oi.quantity * oi.unit_price) AS TotalRevenue,
                                COUNT(DISTINCT o.order_id) AS OrderCount
                            FROM 
                                order_items oi
                                INNER JOIN orders o ON oi.order_id = o.order_id
                            WHERE 
                                o.order_date BETWEEN @fromDate AND @toDate
                            GROUP BY 
                                oi.product_id
                            ORDER BY 
                                TotalQuantitySold DESC";

                        adapter = new SqlDataAdapter(query, connection);
                        adapter.SelectCommand.Parameters.AddWithValue("@fromDate", fromDate);
                        adapter.SelectCommand.Parameters.AddWithValue("@toDate", toDate);
                        adapter.Fill(dataTable);
                    }
                    else
                    {
                        throw;
                    }
                }

                if (dataTable.Rows.Count > 0)
                {
                    reportDataGridView.DataSource = dataTable;

                    if (reportDataGridView.Columns["OrderPercentage"] != null)
                        reportDataGridView.Columns["OrderPercentage"].DefaultCellStyle.Format = "0.00\\%";
                    if (reportDataGridView.Columns["TotalRevenue"] != null)
                        reportDataGridView.Columns["TotalRevenue"].DefaultCellStyle.Format = "C";
                }
                else
                {
                    MessageBox.Show("No sales data found for the selected date range.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reportDataGridView.DataSource = null;
                }
            }
        }

        private void LogError(Exception ex)
        {
            try
            {
                System.IO.File.AppendAllText("error.log", $"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}\n");
            }
            catch
            {
                // Suppress logging errors
            }
        }

        private void LogOperation(string operation, string details)
        {
            try
            {
                System.IO.File.AppendAllText("operation.log", $"{DateTime.Now}: {operation} - {details} (User: {_userId})\n");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void UserShowbtn_Click(object sender, EventArgs e)
        {
            LoadData();
            Newstaff_btn.Visible = false;
            Stafflist_btn.Visible = false;
            EFstaff_btn.Visible = false;
            dataGridView1.Visible = true;
            Backtostaff.Visible = false;
            Additem_btn.Visible = false;
            if (reportPanel != null) reportPanel.Visible = false;
            if (menuPanel != null) menuPanel.Visible = false;

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
            Additem_btn.Visible = false;
            if (reportPanel != null) reportPanel.Visible = false;
            if (menuPanel != null) menuPanel.Visible = false;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private void StaffManagement_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            Newstaff_btn.Visible = true;
            Stafflist_btn.Visible = true;
            EFstaff_btn.Visible = true;
            Backtostaff.Visible = false;
            Additem_btn.Visible = false;
            if (reportPanel != null) reportPanel.Visible = false;
            if (menuPanel != null) menuPanel.Visible = false;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private Panel menuPanel;

        private void MenuManagement_btn_Click(object sender, EventArgs e)
        {
            LoadMenuData();
            Newstaff_btn.Visible = false;
            Stafflist_btn.Visible = false;
            EFstaff_btn.Visible = false;
            dataGridView1.Visible = true;
            Backtostaff.Visible = false;
            Additem_btn.Visible = true;
            if (reportPanel != null) reportPanel.Visible = false;
            if (menuPanel != null) menuPanel.Visible = false;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
            dataGridView1.CellContentClick += MenuDataGridView_CellContentClick;
        }

        private void LoadMenuData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = @"SELECT 
                            p.product_id AS 'ID',
                            p.product_name AS 'Menu Item',
                            p.category AS 'Category',
                            p.unit_price AS 'Price',
                            i.quantity_in_stock AS 'Stock',
                            CASE WHEN p.is_active = 1 THEN 'Active' ELSE 'Inactive' END AS 'Status'
                        FROM 
                            products p
                        INNER JOIN 
                            inventory i ON p.product_id = i.product_id
                        ORDER BY 
                            p.category, p.product_name";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable menuDataTable = new DataTable();
                    adapter.Fill(menuDataTable);

                    dataGridView1.DataSource = null;
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = menuDataTable;

                    // Add edit and toggle status buttons
                    DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn
                    {
                        Name = "Edit",
                        HeaderText = "Edit",
                        Text = "Edit",
                        UseColumnTextForButtonValue = true,
                        Width = 80
                    };
                    dataGridView1.Columns.Add(editButtonColumn);

                    DataGridViewButtonColumn toggleButtonColumn = new DataGridViewButtonColumn
                    {
                        Name = "Toggle",
                        HeaderText = "Toggle",
                        Text = "Toggle",
                        UseColumnTextForButtonValue = true,
                        Width = 80
                    };
                    dataGridView1.Columns.Add(toggleButtonColumn);

                    // Format columns
                    if (dataGridView1.Columns["ID"] != null)
                        dataGridView1.Columns["ID"].Width = 60;
                    if (dataGridView1.Columns["Menu Item"] != null)
                        dataGridView1.Columns["Menu Item"].Width = 200;
                    if (dataGridView1.Columns["Category"] != null)
                        dataGridView1.Columns["Category"].Width = 120;
                    if (dataGridView1.Columns["Price"] != null)
                    {
                        dataGridView1.Columns["Price"].Width = 100;
                        dataGridView1.Columns["Price"].DefaultCellStyle.Format = "0.00 $";
                        dataGridView1.Columns["Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if (dataGridView1.Columns["Stock"] != null)
                        dataGridView1.Columns["Stock"].Width = 80;
                    if (dataGridView1.Columns["Status"] != null)
                        dataGridView1.Columns["Status"].Width = 80;

                    SetupDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading menu data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void MenuDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            int productId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            string productName = dataGridView1.Rows[e.RowIndex].Cells["Menu Item"].Value.ToString();

            if (columnName == "Edit")
            {
                try
                {
                    MenuItemForm editForm = new MenuItemForm(productId);
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadMenuData(); // Refresh data after editing
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error editing menu item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (columnName == "Toggle")
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        string query = @"UPDATE products 
                               SET is_active = CASE WHEN is_active = 1 THEN 0 ELSE 1 END 
                               WHERE product_id = @product_id";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@product_id", productId);
                            connection.Open();
                            command.ExecuteNonQuery();
                            LoadMenuData(); // Refresh data after toggling
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error toggling menu item status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowMenuManagementPanel()
        {
            dataGridView1.Visible = false;
            Newstaff_btn.Visible = false;
            Stafflist_btn.Visible = false;
            EFstaff_btn.Visible = false;
            Backtostaff.Visible = false;
            Additem_btn.Visible = true;
            if (reportPanel != null) reportPanel.Visible = false;

            if (!Controls.Contains(menuPanel))
            {
                CreateMenuManagementPanel();
            }

            menuPanel.Visible = true;
        }

        private void CreateMenuManagementPanel()
        {
            menuPanel = new Panel
            {
                BackColor = Color.FromArgb(176, 137, 104),
                Location = dataGridView1.Location,
                Name = "menuPanel",
                Size = dataGridView1.Size,
                Visible = false
            };

            // Use existing Additem_btn instead of creating a new button
            Controls.Add(menuPanel);
        }

        private void Report_btn_Click(object sender, EventArgs e)
        {
            ShowReportOptions();
            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
            if (menuPanel != null) menuPanel.Visible = false;
        }

        private void Newstaff_btn_Click(object sender, EventArgs e)
        {
            Newstaff_form newStaffForm = new Newstaff_form();
            newStaffForm.ShowDialog();
            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private void Stafflist_btn_Click(object sender, EventArgs e)
        {
            LoadStaffData();
            Newstaff_btn.Visible = true;
            Stafflist_btn.Visible = true;
            EFstaff_btn.Visible = true;
            dataGridView1.Visible = true;
            Backtostaff.Visible = true;
            Additem_btn.Visible = false;
            if (reportPanel != null) reportPanel.Visible = false;
            if (menuPanel != null) menuPanel.Visible = false;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private void Editstaff_btn_Click(object sender, EventArgs e)
        {
            LoadStaffData(includeEditDisableButtons: true);
            Newstaff_btn.Visible = false;
            Stafflist_btn.Visible = false;
            EFstaff_btn.Visible = false;
            dataGridView1.Visible = true;
            Backtostaff.Visible = true;
            Additem_btn.Visible = false;
            if (reportPanel != null) reportPanel.Visible = false;
            if (menuPanel != null) menuPanel.Visible = false;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
            dataGridView1.CellContentClick += DataGridView1_CellContentClick;
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

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
                    LoadStaffData(includeEditDisableButtons: true);
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
                                    LoadStaffData(includeEditDisableButtons: true);
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
            Additem_btn.Visible = false;
            if (reportPanel != null) reportPanel.Visible = false;
            if (menuPanel != null) menuPanel.Visible = false;

            dataGridView1.CellContentClick -= DataGridView1_CellContentClick;
        }

        private void OwnerForm_Load(object sender, EventArgs e)
        {
        }

        private void Additem_btn_Click(object sender, EventArgs e)
        {
            MenuItemForm newItemForm = new MenuItemForm(0); // 0 for new item
            if (newItemForm.ShowDialog() == DialogResult.OK)
            {
                LoadMenuData(); // Refresh data after adding new item
            }
        }
    }
}