using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using UserManagementSystem;

namespace CafeManagemnt
{
    public partial class AdminForm : Form
    {
        private readonly int _userId;
        private const string _connectionString = @"Data Source=.;Initial Catalog=CafemanagementDB;Integrated Security=True;Encrypt=False";
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
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = false;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.RowTemplate.Height = 40;
            dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 12F);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);

            dataGridView1.Columns.Clear();

            DataGridViewTextBoxColumn usernameColumn = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "username",
                HeaderText = "Username",
                Name = "username",
                Width = 200
            };
            dataGridView1.Columns.Add(usernameColumn);

            DataGridViewComboBoxColumn roleColumn = new DataGridViewComboBoxColumn
            {
                DataPropertyName = "role_id",
                HeaderText = "Role",
                Name = "role_id",
                DisplayMember = "role_name",
                ValueMember = "role_id",
                Width = 150
            };
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT role_id, role_name FROM roles";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable rolesTable = new DataTable();
                adapter.Fill(rolesTable);
                roleColumn.DataSource = rolesTable;
            }
            dataGridView1.Columns.Add(roleColumn);

            DataGridViewComboBoxColumn statusColumn = new DataGridViewComboBoxColumn
            {
                DataPropertyName = "is_active",
                HeaderText = "Status",
                Name = "is_active",
                Width = 100
            };
            statusColumn.DataSource = new[]
            {
                new { Display = "Active", Value = "Active" },
                new { Display = "Inactive", Value = "Inactive" }
            };
            statusColumn.DisplayMember = "Display";
            statusColumn.ValueMember = "Value";
            dataGridView1.Columns.Add(statusColumn);

            DataGridViewButtonColumn saveButtonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "Save",
                UseColumnTextForButtonValue = true,
                Name = "save_button",
                Width = 80
            };
            dataGridView1.Columns.Add(saveButtonColumn);

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.CellClick += DataGridView1_CellClick;
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
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

        // Add this method to your AdminForm class
        private void ShowReportOptions()
        {
            // Hide user management controls
            dataGridView1.Visible = false;
            Adduserbtn.Visible = false;
            AddRolebtn.Visible = false;

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
                BackColor = System.Drawing.Color.FromArgb(176, 137, 104),
                Location = new System.Drawing.Point(333, 108), // Same as dataGridView1
                Name = "reportPanel",
                Size = new System.Drawing.Size(676, 470), // Same as dataGridView1
                Visible = false
            };

            // Create report type label
            Label reportTypeLabel = new Label
            {
                AutoSize = true,
                Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(67, 40, 24),
                Location = new System.Drawing.Point(20, 20),
                Text = "Select Report Type:"
            };

            // Create report type combo box
            reportTypeComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new System.Drawing.Font("Arial", 14F),
                FormattingEnabled = true,
                Location = new System.Drawing.Point(250, 20),
                Size = new System.Drawing.Size(250, 30),
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
                Font = new System.Drawing.Font("Calibri", 14F),
                ForeColor = System.Drawing.Color.FromArgb(67, 40, 24),
                Location = new System.Drawing.Point(20, 70),
                Text = "From Date:"
            };

            fromDatePicker = new DateTimePicker
            {
                Font = new System.Drawing.Font("Arial", 12F),
                Format = DateTimePickerFormat.Short,
                Location = new System.Drawing.Point(120, 70),
                Size = new System.Drawing.Size(150, 30),
                Value = DateTime.Now.AddDays(-30)
            };

            Label toDateLabel = new Label
            {
                AutoSize = true,
                Font = new System.Drawing.Font("Calibri", 14F),
                ForeColor = System.Drawing.Color.FromArgb(67, 40, 24),
                Location = new System.Drawing.Point(300, 70),
                Text = "To Date:"
            };

            toDatePicker = new DateTimePicker
            {
                Font = new System.Drawing.Font("Arial", 12F),
                Format = DateTimePickerFormat.Short,
                Location = new System.Drawing.Point(380, 70),
                Size = new System.Drawing.Size(150, 30),
                Value = DateTime.Now
            };

            // Create generate report button
            generateReportButton = new Controls.MainButton
            {
                BackColor = System.Drawing.Color.FromArgb(67, 40, 24),
                BackgroundColor = System.Drawing.Color.FromArgb(67, 40, 24),
                BorderColor = System.Drawing.Color.White,
                BorderRadius = 20,
                BorderSize = 0,
                FlatAppearance = { BorderSize = 0 },
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(550, 65),
                Name = "generateReportButton",
                Size = new System.Drawing.Size(120, 40),
                Text = "Generate",
                TextColor = System.Drawing.Color.White,
                UseVisualStyleBackColor = false
            };
            generateReportButton.Click += GenerateReportButton_Click;

            // Create report data grid view
            reportDataGridView = new DataGridView
            {
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Location = new System.Drawing.Point(20, 120),
                Name = "reportDataGridView",
                Size = new System.Drawing.Size(630, 320),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = System.Drawing.Color.White,
                RowTemplate = { Height = 30 },
                DefaultCellStyle = { Font = new System.Drawing.Font("Arial", 10F) },
                ColumnHeadersDefaultCellStyle = { Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold) },
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false
            };

            // Create export report button (positioned like AddRolebtn)
            exportReportButton = new Controls.MainButton
            {
                BackColor = System.Drawing.Color.FromArgb(67, 40, 24),
                BackgroundColor = System.Drawing.Color.FromArgb(67, 40, 24),
                BorderColor = System.Drawing.Color.White,
                BorderRadius = 20,
                BorderSize = 0,
                FlatAppearance = { BorderSize = 0 },
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(475, 486), // Positioned relative to form, not panel
                Name = "exportReportButton",
                Size = new System.Drawing.Size(168, 60),
                Text = "Export Excel",
                TextColor = System.Drawing.Color.White,
                UseVisualStyleBackColor = false,
                Visible = false
            };
            exportReportButton.Click += ExportReportButton_Click;

            // Add controls to report panel
            reportPanel.Controls.Add(reportTypeLabel);
            reportPanel.Controls.Add(reportTypeComboBox);
            reportPanel.Controls.Add(fromDateLabel);
            reportPanel.Controls.Add(fromDatePicker);
            reportPanel.Controls.Add(toDateLabel);
            reportPanel.Controls.Add(toDatePicker);
            reportPanel.Controls.Add(generateReportButton);
            reportPanel.Controls.Add(reportDataGridView);

            // Add report panel and export button to form
            Controls.Add(reportPanel);
            Controls.Add(exportReportButton);
        }

        // Add this method to handle report generation
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

                    exportReportButton.Visible = reportDataGridView.Rows.Count > 0;
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

        // Replace your existing report generation methods with these corrected versions

        private void GenerateSalesReport(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
                    // Format currency columns
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["TotalSales"] != DBNull.Value)
                            row["TotalSales"] = string.Format("{0:C}", row["TotalSales"]);
                        if (row["AverageOrderValue"] != DBNull.Value)
                            row["AverageOrderValue"] = string.Format("{0:C}", row["AverageOrderValue"]);
                    }

                    reportDataGridView.DataSource = dataTable;
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Query to get inventory with product names - showing only important details
                string query = @"
            SELECT 
                CASE
                    WHEN p.product_name IS NOT NULL THEN p.product_name
                    WHEN i.item_name IS NOT NULL THEN i.item_name
                    ELSE 'Product ID: ' + CAST(i.product_id AS VARCHAR(10))
                END AS ProductName,
                i.quantity_in_stock AS CurrentStock,
                i.reorder_level AS ReorderLevel,
                CASE 
                    WHEN i.quantity_in_stock <= i.reorder_level THEN 'Reorder Required'
                    WHEN i.quantity_in_stock <= (i.reorder_level * 1.5) THEN 'Low Stock'
                    ELSE 'In Stock'
                END AS Status,
                i.unit_price AS UnitPrice
            FROM 
                inventory i
                LEFT JOIN products p ON i.product_id = p.product_id
                /* If you need to filter by date, add a WHERE clause here */
                /* WHERE i.last_updated BETWEEN @fromDate AND @toDate */
            ORDER BY 
                CASE 
                    WHEN i.quantity_in_stock <= i.reorder_level THEN 1
                    WHEN i.quantity_in_stock <= (i.reorder_level * 1.5) THEN 2
                    ELSE 3
                END,
                CASE
                    WHEN p.product_name IS NOT NULL THEN p.product_name
                    WHEN i.item_name IS NOT NULL THEN i.item_name
                    ELSE CAST(i.product_id AS VARCHAR(10))
                END";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                try
                {
                    adapter.Fill(dataTable);
                }
                catch (SqlException ex)
                {
                    // If the above query fails, try alternative approaches
                    if (ex.Message.Contains("Invalid object name") || ex.Message.Contains("Invalid column name"))
                    {
                        // Try without products table join
                        try
                        {
                            query = @"
                        SELECT 
                            p.product_name AS ProductName,
                            i.quantity_in_stock AS CurrentStock,
                            i.reorder_level AS ReorderLevel,
                            CASE 
                                WHEN i.quantity_in_stock <= i.reorder_level THEN 'Reorder Required'
                                WHEN i.quantity_in_stock <= (i.reorder_level * 1.5) THEN 'Low Stock'
                                ELSE 'In Stock'
                            END AS Status,
                            i.unit_price AS UnitPrice
                        FROM 
                            inventory i
                            LEFT JOIN products p ON i.product_id = p.product_id
                        ORDER BY 
                            CASE 
                                WHEN i.quantity_in_stock <= i.reorder_level THEN 1
                                WHEN i.quantity_in_stock <= (i.reorder_level * 1.5) THEN 2
                                ELSE 3
                            END,
                            p.product_name";

                            adapter = new SqlDataAdapter(query, connection);
                            adapter.Fill(dataTable);
                        }
                        catch (SqlException ex2)
                        {
                            // Last resort - get all columns and format them
                            query = @"SELECT * FROM inventory";
                            adapter = new SqlDataAdapter(query, connection);
                            adapter.Fill(dataTable);

                            // Filter to show only important columns
                            DataTable filteredTable = new DataTable();
                            filteredTable.Columns.Add("ProductName", typeof(string));
                            filteredTable.Columns.Add("CurrentStock", typeof(int));
                            filteredTable.Columns.Add("Status", typeof(string));
                            filteredTable.Columns.Add("UnitPrice", typeof(string));

                            foreach (DataRow row in dataTable.Rows)
                            {
                                DataRow newRow = filteredTable.NewRow();

                                // Get product name from available columns
                                string productName = "Unknown Product";
                                if (dataTable.Columns.Contains("item_name") && row["item_name"] != DBNull.Value)
                                    productName = row["item_name"].ToString();
                                else if (dataTable.Columns.Contains("product_name") && row["product_name"] != DBNull.Value)
                                    productName = row["product_name"].ToString();
                                else if (dataTable.Columns.Contains("inventory_id"))
                                    productName = "Item ID: " + row["inventory_id"].ToString();

                                newRow["ProductName"] = productName;

                                // Get stock quantity
                                int currentStock = 0;
                                if (dataTable.Columns.Contains("quantity_in_stock") && row["quantity_in_stock"] != DBNull.Value)
                                    int.TryParse(row["quantity_in_stock"].ToString(), out currentStock);
                                else if (dataTable.Columns.Contains("current_stock") && row["current_stock"] != DBNull.Value)
                                    int.TryParse(row["current_stock"].ToString(), out currentStock);

                                newRow["CurrentStock"] = currentStock;

                                // Determine status
                                int reorderLevel = 0;
                                if (dataTable.Columns.Contains("reorder_level") && row["reorder_level"] != DBNull.Value)
                                    int.TryParse(row["reorder_level"].ToString(), out reorderLevel);

                                string status = "In Stock";
                                if (reorderLevel > 0)
                                {
                                    if (currentStock <= reorderLevel)
                                        status = "Reorder Required";
                                    else if (currentStock <= (reorderLevel * 1.5))
                                        status = "Low Stock";
                                }
                                newRow["Status"] = status;

                                // Get unit price
                                string unitPrice = "N/A";
                                if (dataTable.Columns.Contains("unit_price") && row["unit_price"] != DBNull.Value)
                                {
                                    if (decimal.TryParse(row["unit_price"].ToString(), out decimal price))
                                        unitPrice = string.Format("{0:C}", price);
                                }
                                newRow["UnitPrice"] = unitPrice;

                                filteredTable.Rows.Add(newRow);
                            }

                            dataTable = filteredTable;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }

                if (dataTable.Rows.Count > 0)
                {
                    // Format currency columns if not already formatted
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (dataTable.Columns.Contains("UnitPrice") && row["UnitPrice"] != DBNull.Value)
                        {
                            string priceStr = row["UnitPrice"].ToString();
                            if (!priceStr.StartsWith("$") && !priceStr.Contains("N/A"))
                            {
                                if (decimal.TryParse(priceStr, out decimal price))
                                {
                                    row["UnitPrice"] = string.Format("{0:C}", price);
                                }
                            }
                        }
                    }

                    reportDataGridView.DataSource = dataTable;

                    // Apply color formatting for status column
                    reportDataGridView.CellFormatting -= ReportDataGridView_CellFormatting; // Remove previous handler
                    reportDataGridView.CellFormatting += ReportDataGridView_CellFormatting;
                }
                else
                {
                    MessageBox.Show("No inventory data found.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reportDataGridView.DataSource = null;
                }
            }
        }

        // Add this method for status color formatting
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
                    // If the user_logs table doesn't exist or has different structure, try simpler query
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
                    // Create a new DataTable with properly formatted data
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

                        // Format date columns
                        newRow["FirstLogin"] = row["FirstLogin"] == DBNull.Value ? "N/A" :
                            Convert.ToDateTime(row["FirstLogin"]).ToString("yyyy-MM-dd HH:mm");
                        newRow["LastLogin"] = row["LastLogin"] == DBNull.Value ? "N/A" :
                            Convert.ToDateTime(row["LastLogin"]).ToString("yyyy-MM-dd HH:mm");

                        // Format session time with better error handling
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Updated query to work with products table instead of items
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
                    // If products table doesn't exist or has different structure, try alternative
                    if (ex.Message.Contains("Invalid object name") || ex.Message.Contains("Invalid column name"))
                    {
                        // Simplified query without products join
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
                    // Format currency and percentage columns
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["TotalRevenue"] != DBNull.Value)
                        {
                            if (decimal.TryParse(row["TotalRevenue"].ToString(), out decimal revenue))
                            {
                                row["TotalRevenue"] = string.Format("{0:C}", revenue);
                            }
                        }
                        if (dataTable.Columns.Contains("OrderPercentage") && row["OrderPercentage"] != DBNull.Value)
                        {
                            if (decimal.TryParse(row["OrderPercentage"].ToString(), out decimal percentage))
                            {
                                row["OrderPercentage"] = string.Format("{0:0.00}%", percentage);
                            }
                        }
                    }

                    reportDataGridView.DataSource = dataTable;
                }
                else
                {
                    MessageBox.Show("No sales data found for the selected date range.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    reportDataGridView.DataSource = null;
                }
            }
        }

        // Method to export report to Excel
        private void ExportReportButton_Click(object sender, EventArgs e)
            {
                if (reportDataGridView.DataSource == null || reportDataGridView.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export.", "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel Files (*.xlsx)|*.xlsx",
                        Title = "Save Report as Excel",
                        FileName = $"{reportTypeComboBox.SelectedItem}_{DateTime.Now:yyyyMMdd}.xlsx"
                    };

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                        return;

                    Cursor = Cursors.WaitCursor;

                    // Create a new Excel file
                    using (var workbook = new ClosedXML.Excel.XLWorkbook())
                    {
                        DataTable dt = (DataTable)reportDataGridView.DataSource;
                        var worksheet = workbook.Worksheets.Add("Report");

                        // Add header row
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;
                        }

                        // Add data rows
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                worksheet.Cell(i + 2, j + 1).Value = dt.Rows[i][j].ToString();
                            }
                        }

                        // Auto-fit columns
                        worksheet.Columns().AdjustToContents();

                        // Save workbook
                        workbook.SaveAs(saveFileDialog.FileName);
                    }

                    MessageBox.Show($"Report successfully exported to {saveFileDialog.FileName}", "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LogOperation("Report Export", $"Exported {reportTypeComboBox.SelectedItem} report to Excel");
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    MessageBox.Show($"Error exporting report: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }




        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["save_button"].Index)
            {
                if (MessageBox.Show("Save changes for this user?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SaveRowChanges(e.RowIndex);
                }
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

                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Username cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(_connectionString))
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
            Adduserbtn.Visible = true;
            AddRolebtn.Visible = true;
            dataGridView1.Visible = true;
        }
        private void Adduserbtn_Click(object sender, EventArgs e)
        {
            Form AddUser = new UserRegistrationForm();
            AddUser.Show();
        }

        private void AddRolebtn_Click(object sender, EventArgs e)
        {
            Form AddRole = new RoleCreationForm();
            AddRole.Show();
        }

        

       

        private void Backupbtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Configure SaveFileDialog for backup file
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Backup Files (*.bak)|*.bak";
                    saveFileDialog.Title = "Select Backup File Location";
                    saveFileDialog.FileName = $"CafemanagementDB_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                        return;

                    string backupPath = saveFileDialog.FileName;

                    // Perform backup
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        string backupQuery = @"
                            BACKUP DATABASE CafemanagementDB 
                            TO DISK = @backupPath 
                            WITH FORMAT, 
                                 NAME = 'CafemanagementDB Backup', 
                                 STATS = 10";
                        using (SqlCommand command = new SqlCommand(backupQuery, connection))
                        {
                            command.Parameters.AddWithValue("@backupPath", backupPath);
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show($"Backup successfully created at: {backupPath}", "Backup Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error during backup: {sqlEx.Message}", "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during backup: {ex.Message}", "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Restorebtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Warn user about overwriting
                DialogResult confirm = MessageBox.Show(
                    "Restoring the database will overwrite the current database. All current data will be lost. Are you sure you want to proceed?",
                    "Confirm Restore",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm != DialogResult.Yes)
                    return;

                // Disable UI elements
                Restorebtn.Enabled = false;
                Cursor = Cursors.WaitCursor;

                // Configure OpenFileDialog
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Backup Files (*.bak)|*.bak";
                    openFileDialog.Title = "Select Backup File to Restore";
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                        return;

                    string backupPath = openFileDialog.FileName;

                    // Validate file existence
                    if (!File.Exists(backupPath))
                    {
                        MessageBox.Show("Selected backup file does not exist.", "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Use master database for restore
                    string masterConnectionString = _connectionString.Replace("Initial Catalog=CafemanagementDB", "Initial Catalog=master");

                    // Clear connection pool to ensure no lingering connections to CafemanagementDB
                    SqlConnection.ClearAllPools();

                    using (SqlConnection connection = new SqlConnection(masterConnectionString))
                    {
                        await connection.OpenAsync();

                        // Ensure no other connections to CafemanagementDB
                        string killConnectionsQuery = @"
                    DECLARE @kill varchar(8000) = '';
                    SELECT @kill = @kill + 'KILL ' + CONVERT(varchar(10), spid) + ';'
                    FROM master..sysprocesses 
                    WHERE dbid = DB_ID('CafemanagementDB') AND spid > 50;
                    EXEC(@kill);";
                        using (SqlCommand command = new SqlCommand(killConnectionsQuery, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }

                        // Set database to single-user mode
                        string singleUserQuery = @"
                    ALTER DATABASE CafemanagementDB 
                    SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                        using (SqlCommand command = new SqlCommand(singleUserQuery, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }

                        // Restore database
                        string restoreQuery = @"
                    RESTORE DATABASE CafemanagementDB 
                    FROM DISK = @backupPath 
                    WITH REPLACE, RECOVERY";
                        using (SqlCommand command = new SqlCommand(restoreQuery, connection))
                        {
                            command.Parameters.AddWithValue("@backupPath", backupPath);
                            command.CommandTimeout = 600; // Increase timeout for large databases
                            await command.ExecuteNonQueryAsync();
                        }

                        // Set database back to multi-user mode
                        string multiUserQuery = @"
                    ALTER DATABASE CafemanagementDB 
                    SET MULTI_USER";
                        using (SqlCommand command = new SqlCommand(multiUserQuery, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    LogOperation("Restore", $"Database restored from {backupPath}");
                    MessageBox.Show("Database restored successfully.", "Restore Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh data
                    LoadData();
                }
            }
            catch (SqlException sqlEx)
            {
                LogError(sqlEx);
                MessageBox.Show($"Database error during restore: {sqlEx.Message}", "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LogError(ex);
                MessageBox.Show($"Error during restore: {ex.Message}", "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure database is in multi-user mode
                try
                {
                    string masterConnectionString = _connectionString.Replace("Initial Catalog=CafemanagementDB", "Initial Catalog=master");
                    using (SqlConnection connection = new SqlConnection(masterConnectionString))
                    {
                        await connection.OpenAsync();
                        string multiUserQuery = @"
                    ALTER DATABASE CafemanagementDB 
                    SET MULTI_USER";
                        using (SqlCommand command = new SqlCommand(multiUserQuery, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }

                Restorebtn.Enabled = true;
                Cursor = Cursors.Default;
            }
        }


        private void Reportbtn_Click(object sender, EventArgs e)
        {
            ShowReportOptions();
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
                using (SqlConnection connection = new SqlConnection(_connectionString))
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
        private void LogError(Exception ex)
        {
            try
            {
                File.AppendAllText("error.log", $"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}\n");
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
                File.AppendAllText("operation.log", $"{DateTime.Now}: {operation} - {details} (User: {_userId})\n");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

    }
}