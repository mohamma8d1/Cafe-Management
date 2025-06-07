using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CafeManagemnt
{
    public partial class StaffForm : Form
    {
        private readonly int _userId;
        private const string ConnectionString = @"Data Source=.;Initial Catalog=CafeManagementDB;Integrated Security=True;Encrypt=False";
        private Dictionary<int, int> currentOrderItems = new Dictionary<int, int>(); // Track quantities in current order

        public StaffForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
            this.FormClosing += StaffForm_FormClosing;
        }


        private void StaffForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void StaffForm_Load(object sender, EventArgs e)
        {
            ShowCreateOrderPanel();
        }

        private void ClearContentPanel()
        {
            if (contentPanel != null)
            {
                contentPanel.Controls.Clear();
            }
        }

        private int currentOrderId = -1;

        private void ShowCreateOrderPanel()
        {
            ClearContentPanel();
            contentPanel.Controls.Clear();

            DataTable productsTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = @"SELECT 
                                p.product_id, 
                                p.product_name, 
                                p.unit_price,
                                ISNULL(i.quantity_in_stock, 0) AS inventory_count
                             FROM dbo.products p
                             LEFT JOIN dbo.inventory i ON p.product_id = i.product_id
                             WHERE ISNULL(i.is_active, 1) = 1
                             ORDER BY p.product_name";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(productsTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create New Order Button
            Button createOrderButton = new Button
            {
                Text = "🛒 Create New Order",
                Size = new Size(200, 45),
                BackColor = Color.FromArgb(67, 40, 24),
                ForeColor = Color.FromArgb(176, 137, 104),
                Font = new Font("Calibri", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            createOrderButton.Location = new Point((contentPanel.Width - createOrderButton.Width) / 2, 15);
            createOrderButton.FlatAppearance.BorderSize = 2;
            createOrderButton.FlatAppearance.BorderColor = Color.FromArgb(176, 137, 104);

            createOrderButton.Click += (s, ev) =>
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        string query = @"INSERT INTO dbo.orders (user_id, order_date, total_amount, status, shipping_address, billing_address, payment_method, payment_status)
                            OUTPUT INSERTED.order_id
                            VALUES (@user_id, GETDATE(), 0.00, 'pending', 'N/A', 'N/A', 'Debit Card', 'unpaid')";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@user_id", _userId);
                            connection.Open();
                            currentOrderId = (int)command.ExecuteScalar();
                        }
                    }

                    // Reset order items tracking
                    currentOrderItems.Clear();

                    MessageBox.Show($"✅ New order created with ID: {currentOrderId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    createOrderButton.Text = $"📋 Current Order: #{currentOrderId}";
                    createOrderButton.BackColor = Color.DarkGreen;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            contentPanel.Controls.Add(createOrderButton);

            // Products Grid with Plus/Minus Controls
            int startY = 80;
            int itemWidth = 120; // Made wider for plus/minus buttons
            int itemHeight = 160; // Made taller for quantity controls
            int spacing = 15;
            int itemsPerRow = 5; // Reduced to fit wider items
            int totalWidth = (itemWidth * itemsPerRow) + (spacing * (itemsPerRow - 1));
            int startX = (contentPanel.Width - totalWidth) / 2;

            for (int i = 0; i < productsTable.Rows.Count; i++)
            {
                DataRow row = productsTable.Rows[i];
                int col = i % itemsPerRow;
                int rowIndex = i / itemsPerRow;

                int x = startX + col * (itemWidth + spacing);
                int y = startY + rowIndex * (itemHeight + spacing);

                Panel productPanel = new Panel
                {
                    Location = new Point(x, y),
                    Size = new Size(itemWidth, itemHeight),
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.White,
                    Cursor = Cursors.Hand
                };

                // Product Name Label
                Label nameLabel = new Label
                {
                    Text = row["product_name"].ToString(),
                    Location = new Point(5, 5),
                    Size = new Size(itemWidth - 10, 25),
                    Font = new Font("Calibri", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(67, 40, 24),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                // Price Label
                decimal price = Convert.ToDecimal(row["unit_price"]);
                Label priceLabel = new Label
                {
                    Text = $"${price:F2}",
                    Location = new Point(5, 30),
                    Size = new Size(itemWidth - 10, 20),
                    Font = new Font("Calibri", 10, FontStyle.Bold),
                    ForeColor = Color.DarkGreen,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                // Inventory Count Label
                int inventoryCount = Convert.ToInt32(row["inventory_count"]);
                Label inventoryLabel = new Label
                {
                    Text = $"Stock: {inventoryCount}",
                    Location = new Point(5, 50),
                    Size = new Size(itemWidth - 10, 18),
                    Font = new Font("Calibri", 9, FontStyle.Regular),
                    ForeColor = inventoryCount > 0 ? Color.FromArgb(0, 120, 0) : Color.Red,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                int productId = Convert.ToInt32(row["product_id"]);

                // Get current quantity in order
                int currentQty = currentOrderItems.ContainsKey(productId) ? currentOrderItems[productId] : 0;

                // Quantity Label
                Label qtyLabel = new Label
                {
                    Text = $"Qty: {currentQty}",
                    Location = new Point(5, 75),
                    Size = new Size(itemWidth - 10, 20),
                    Font = new Font("Calibri", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(67, 40, 24),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(245, 245, 220)
                };

                // Minus Button
                Button minusButton = new Button
                {
                    Text = "➖",
                    Location = new Point(5, 100),
                    Size = new Size(30, 25),
                    Font = new Font("Calibri", 8, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(220, 20, 60),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand,
                    Enabled = currentQty > 0
                };
                minusButton.FlatAppearance.BorderSize = 0;

                // Plus Button
                Button plusButton = new Button
                {
                    Text = "➕",
                    Location = new Point(itemWidth - 35, 100),
                    Size = new Size(30, 25),
                    Font = new Font("Calibri", 8, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(34, 139, 34),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand,
                    Enabled = inventoryCount > currentQty
                };
                plusButton.FlatAppearance.BorderSize = 0;

                // Add to Cart Button
                Button addToCartButton = new Button
                {
                    Text = currentQty > 0 ? "🛒 UPDATE" : "🛒 ADD",
                    Location = new Point(5, 130),
                    Size = new Size(itemWidth - 10, 25),
                    Font = new Font("Calibri", 9, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = currentQty > 0 ? Color.FromArgb(255, 140, 0) : Color.FromArgb(34, 139, 34),
                    ForeColor = Color.White,
                    Cursor = Cursors.Hand,
                    Enabled = currentQty > 0
                };
                addToCartButton.FlatAppearance.BorderSize = 0;

                // Plus Button Click
                plusButton.Click += (s, ev) =>
                {
                    if (currentOrderId == -1)
                    {
                        MessageBox.Show("⚠️ Please create a new order first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int currentQuantity = currentOrderItems.ContainsKey(productId) ? currentOrderItems[productId] : 0;
                    if (currentQuantity < inventoryCount)
                    {
                        currentOrderItems[productId] = currentQuantity + 1;
                        qtyLabel.Text = $"Qty: {currentOrderItems[productId]}";
                        minusButton.Enabled = true;
                        addToCartButton.Enabled = true;
                        addToCartButton.Text = "🛒 UPDATE";
                        addToCartButton.BackColor = Color.FromArgb(255, 140, 0);

                        if (currentOrderItems[productId] >= inventoryCount)
                            plusButton.Enabled = false;
                    }
                };

                // Minus Button Click
                minusButton.Click += (s, ev) =>
                {
                    if (currentOrderItems.ContainsKey(productId) && currentOrderItems[productId] > 0)
                    {
                        currentOrderItems[productId]--;
                        qtyLabel.Text = $"Qty: {currentOrderItems[productId]}";
                        plusButton.Enabled = true;

                        if (currentOrderItems[productId] == 0)
                        {
                            minusButton.Enabled = false;
                            addToCartButton.Enabled = false;
                            addToCartButton.Text = "🛒 ADD";
                            addToCartButton.BackColor = Color.FromArgb(34, 139, 34);
                        }
                    }
                };

                // Add to Cart Button Click
                addToCartButton.Click += (s, ev) =>
                {
                    if (currentOrderId == -1 || !currentOrderItems.ContainsKey(productId) || currentOrderItems[productId] == 0)
                        return;

                    try
                    {
                        int quantityToAdd = currentOrderItems[productId];

                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {
                            connection.Open();
                            using (SqlTransaction transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    // Check if item already exists in order
                                    string checkQuery = "SELECT quantity FROM dbo.order_items WHERE order_id = @order_id AND product_id = @product_id";
                                    int existingQty = 0;

                                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection, transaction))
                                    {
                                        checkCmd.Parameters.AddWithValue("@order_id", currentOrderId);
                                        checkCmd.Parameters.AddWithValue("@product_id", productId);
                                        var result = checkCmd.ExecuteScalar();
                                        if (result != null)
                                            existingQty = Convert.ToInt32(result);
                                    }

                                    if (existingQty > 0)
                                    {
                                        // Update existing item
                                        string updateQuery = @"UPDATE dbo.order_items 
                                                             SET quantity = @quantity 
                                                             WHERE order_id = @order_id AND product_id = @product_id";
                                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection, transaction))
                                        {
                                            updateCmd.Parameters.AddWithValue("@order_id", currentOrderId);
                                            updateCmd.Parameters.AddWithValue("@product_id", productId);
                                            updateCmd.Parameters.AddWithValue("@quantity", quantityToAdd);
                                            updateCmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        // Insert new item
                                        string insertQuery = @"INSERT INTO dbo.order_items (order_id, product_id, quantity, unit_price)
                                                             VALUES (@order_id, @product_id, @quantity, @unit_price)";
                                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection, transaction))
                                        {
                                            insertCmd.Parameters.AddWithValue("@order_id", currentOrderId);
                                            insertCmd.Parameters.AddWithValue("@product_id", productId);
                                            insertCmd.Parameters.AddWithValue("@quantity", quantityToAdd);
                                            insertCmd.Parameters.AddWithValue("@unit_price", price);
                                            insertCmd.ExecuteNonQuery();
                                        }
                                    }

                                    // Recalculate order total
                                    string totalQuery = @"UPDATE dbo.orders 
                                                        SET total_amount = (
                                                            SELECT SUM(quantity * unit_price) 
                                                            FROM dbo.order_items 
                                                            WHERE order_id = @order_id
                                                        ) 
                                                        WHERE order_id = @order_id";
                                    using (SqlCommand totalCmd = new SqlCommand(totalQuery, connection, transaction))
                                    {
                                        totalCmd.Parameters.AddWithValue("@order_id", currentOrderId);
                                        totalCmd.ExecuteNonQuery();
                                    }

                                    transaction.Commit();
                                }
                                catch
                                {
                                    transaction.Rollback();
                                    throw;
                                }
                            }
                        }

                        MessageBox.Show($"✅ {quantityToAdd}x {row["product_name"]} updated in order #{currentOrderId}!",
                                      "Cart Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Error updating cart: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                // Add border to product panel
                productPanel.Paint += (s, ev) =>
                {
                    using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                    {
                        ev.Graphics.DrawRectangle(pen, 0, 0, productPanel.Width - 1, productPanel.Height - 1);
                    }
                };

                productPanel.Controls.Add(nameLabel);
                productPanel.Controls.Add(priceLabel);
                productPanel.Controls.Add(inventoryLabel);
                productPanel.Controls.Add(qtyLabel);
                productPanel.Controls.Add(minusButton);
                productPanel.Controls.Add(plusButton);
                productPanel.Controls.Add(addToCartButton);
                contentPanel.Controls.Add(productPanel);
            }

            // Current order status label
            if (currentOrderId != -1)
            {
                Label statusLabel = new Label
                {
                    Text = $"📝 Working on Order #{currentOrderId} - Use +/- to adjust quantities, then click UPDATE",
                    Location = new Point(20, 60),
                    Size = new Size(contentPanel.Width - 40, 20),
                    Font = new Font("Calibri", 10, FontStyle.Italic),
                    ForeColor = Color.FromArgb(67, 40, 24),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(245, 245, 220)
                };
                contentPanel.Controls.Add(statusLabel);
            }
        }

        private void ShowOrderProcessingPanel()
        {
            ClearContentPanel();
            contentPanel.Controls.Clear();

            // Create a splitter panel to show orders list and order details
            SplitContainer splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 250,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Top panel - Orders list
            DataGridView ordersGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            // Bottom panel - Order details
            Panel orderDetailsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 220),
                Padding = new Padding(10)
            };

            Label detailsTitle = new Label
            {
                Text = "📋 Order Details - Select an order above to view items",
                Location = new Point(10, 10),
                Size = new Size(760, 30),
                Font = new Font("Calibri", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(67, 40, 24),
                TextAlign = ContentAlignment.MiddleLeft
            };

            ListView orderItemsList = new ListView
            {
                Location = new Point(10, 50),
                Size = new Size(760, 200),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                BackColor = Color.White,
                Font = new Font("Calibri", 11)
            };

            // Add columns to ListView
            orderItemsList.Columns.Add("Item", 300);
            orderItemsList.Columns.Add("Quantity", 100);
            orderItemsList.Columns.Add("Unit Price", 120);
            orderItemsList.Columns.Add("Subtotal", 120);

            Panel actionButtonsPanel = new Panel
            {
                Location = new Point(10, 260),
                Size = new Size(760, 50),
                BackColor = Color.Transparent
            };

            Button processOrderBtn = new Button
            {
                Text = "✅ Process Selected Order",
                Location = new Point(0, 10),
                Size = new Size(200, 35),
                BackColor = Color.FromArgb(34, 139, 34),
                ForeColor = Color.White,
                Font = new Font("Calibri", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            processOrderBtn.FlatAppearance.BorderSize = 0;

            Button cancelOrderBtn = new Button
            {
                Text = "❌ Cancel Selected Order",
                Location = new Point(220, 10),
                Size = new Size(200, 35),
                BackColor = Color.FromArgb(220, 20, 60),
                ForeColor = Color.White,
                Font = new Font("Calibri", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            cancelOrderBtn.FlatAppearance.BorderSize = 0;

            Label totalLabel = new Label
            {
                Text = "Total: $0.00",
                Location = new Point(450, 15),
                Size = new Size(150, 25),
                Font = new Font("Calibri", 14, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                TextAlign = ContentAlignment.MiddleRight
            };

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = @"
                SELECT 
                    order_id AS [Order ID], 
                    CONVERT(varchar, order_date, 108) AS [Time Created],
                    status AS [Status],
                    ISNULL(total_amount, 0.00) AS [Total Amount]
                FROM dbo.orders o
                WHERE user_id = @user_id AND status IN ('pending', 'processing')
                ORDER BY order_date DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", _userId);
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        ordersGridView.DataSource = dataTable;

                        // Format the Total Amount column
                        if (ordersGridView.Columns["Total Amount"] != null)
                        {
                            ordersGridView.Columns["Total Amount"].DefaultCellStyle.Format = "C2";
                            ordersGridView.Columns["Total Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Handle order selection
            ordersGridView.SelectionChanged += (s, ev) =>
            {
                if (ordersGridView.SelectedRows.Count > 0 && ordersGridView.SelectedRows[0].Cells["Order ID"].Value != null)
                {
                    try
                    {
                        int selectedOrderId = Convert.ToInt32(ordersGridView.SelectedRows[0].Cells["Order ID"].Value);

                        // Safely get the total amount
                        decimal orderTotal = 0;
                        var totalCell = ordersGridView.SelectedRows[0].Cells["Total Amount"].Value;
                        if (totalCell != null && totalCell != DBNull.Value)
                        {
                            orderTotal = Convert.ToDecimal(totalCell);
                        }

                        detailsTitle.Text = $"📋 Order #{selectedOrderId} Details";
                        totalLabel.Text = $"Total: {orderTotal:C2}";

                        // Enable action buttons
                        processOrderBtn.Enabled = true;
                        cancelOrderBtn.Enabled = true;

                        // Load order items
                        LoadOrderItems(selectedOrderId, orderItemsList);
                    }
                    catch (Exception ex)
                    {
                        // If there's any error with the selected row, reset to default state
                        detailsTitle.Text = "📋 Order Details - Select an order above to view items";
                        totalLabel.Text = "Total: $0.00";
                        processOrderBtn.Enabled = false;
                        cancelOrderBtn.Enabled = false;
                        orderItemsList.Items.Clear();
                    }
                }
                else
                {
                    detailsTitle.Text = "📋 Order Details - Select an order above to view items";
                    totalLabel.Text = "Total: $0.00";
                    processOrderBtn.Enabled = false;
                    cancelOrderBtn.Enabled = false;
                    orderItemsList.Items.Clear();
                }
            };

            // Process order button click
            processOrderBtn.Click += (s, ev) =>
            {
                if (ordersGridView.SelectedRows.Count > 0)
                {
                    int orderId = Convert.ToInt32(ordersGridView.SelectedRows[0].Cells["Order ID"].Value);
                    ProcessOrder(orderId);
                }
            };

            // Cancel order button click
            cancelOrderBtn.Click += (s, ev) =>
            {
                if (ordersGridView.SelectedRows.Count > 0)
                {
                    int orderId = Convert.ToInt32(ordersGridView.SelectedRows[0].Cells["Order ID"].Value);
                    CancelOrder(orderId);
                }
            };

            // Add controls to panels
            orderDetailsPanel.Controls.Add(detailsTitle);
            orderDetailsPanel.Controls.Add(orderItemsList);
            actionButtonsPanel.Controls.Add(processOrderBtn);
            actionButtonsPanel.Controls.Add(cancelOrderBtn);
            actionButtonsPanel.Controls.Add(totalLabel);
            orderDetailsPanel.Controls.Add(actionButtonsPanel);

            splitContainer.Panel1.Controls.Add(ordersGridView);
            splitContainer.Panel2.Controls.Add(orderDetailsPanel);
            contentPanel.Controls.Add(splitContainer);
        }

        private void LoadOrderItems(int orderId, ListView orderItemsList)
        {
            orderItemsList.Items.Clear();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = @"
                SELECT 
                    p.product_name,
                    oi.quantity,
                    oi.unit_price,
                    (oi.quantity * oi.unit_price) AS subtotal
                FROM dbo.order_items oi
                INNER JOIN dbo.products p ON oi.product_id = p.product_id
                WHERE oi.order_id = @order_id
                ORDER BY p.product_name";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@order_id", orderId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["product_name"].ToString());
                                item.SubItems.Add(reader["quantity"].ToString());
                                item.SubItems.Add(Convert.ToDecimal(reader["unit_price"]).ToString("C2"));
                                item.SubItems.Add(Convert.ToDecimal(reader["subtotal"]).ToString("C2"));

                                orderItemsList.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessOrder(int orderId)
        {
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to process Order #{orderId}?\n\nThis will:\n• Mark the order as delivered\n• Deduct items from inventory",
                "Confirm Process Order",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Deduct inventory
                            string deductInventoryQuery = @"
                        UPDATE i 
                        SET quantity_in_stock = quantity_in_stock - oi.quantity,
                            quantity_ordered = ISNULL(quantity_ordered, 0) + oi.quantity,
                            updated_at = GETDATE()
                        FROM dbo.inventory i
                        INNER JOIN dbo.order_items oi ON i.product_id = oi.product_id
                        WHERE oi.order_id = @order_id";

                            using (SqlCommand deductCmd = new SqlCommand(deductInventoryQuery, connection, transaction))
                            {
                                deductCmd.Parameters.AddWithValue("@order_id", orderId);
                                deductCmd.ExecuteNonQuery();
                            }

                            // Update order status
                            string updateOrderQuery = "UPDATE dbo.orders SET status = 'delivered', updated_at = GETDATE() WHERE order_id = @order_id";
                            using (SqlCommand updateCmd = new SqlCommand(updateOrderQuery, connection, transaction))
                            {
                                updateCmd.Parameters.AddWithValue("@order_id", orderId);
                                updateCmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                ShowOrderProcessingPanel(); // Refresh the entire panel
                MessageBox.Show($"✅ Order #{orderId} processed successfully!\n💰 Inventory has been deducted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelOrder(int orderId)
        {
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to cancel Order #{orderId}?",
                "Confirm Cancel Order",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = "UPDATE dbo.orders SET status = 'cancelled', updated_at = GETDATE() WHERE order_id = @order_id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@order_id", orderId);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            ShowOrderProcessingPanel(); // Refresh the entire panel
                            MessageBox.Show($"❌ Order #{orderId} cancelled successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cancelling order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateOrderbtn_Click(object sender, EventArgs e)
        {
            ShowCreateOrderPanel();
        }

        private void OrderProssbtn_Click(object sender, EventArgs e)
        {
            ShowOrderProcessingPanel();
        }
    }
}