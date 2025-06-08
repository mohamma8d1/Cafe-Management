using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace CafeManagemnt
{
    public partial class MenuItemForm : Form
    {
        private readonly int _productId;
        private const string ConnectionString = @"Data Source=.;Initial Catalog=CafeManagementDB;Integrated Security=True;Encrypt=False";

        public MenuItemForm(int productId)
        {
            _productId = productId;
            InitializeComponent(); // فراخوانی متد تولیدشده توسط Designer
            if (_productId > 0)
            {
                LoadProductData();
                Text = "Edit Menu Item";
            }
            else
            {
                Text = "Add New Menu Item";
            }
        }

        private void LoadProductData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string query = @"SELECT p.product_name, p.category, p.unit_price, p.cost_price, p.is_active, i.quantity_in_stock
                                 FROM dbo.products p
                                 LEFT JOIN dbo.inventory i ON p.product_id = i.product_id
                                 WHERE p.product_id = @product_id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@product_id", _productId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtProductName.Text = reader["product_name"].ToString();
                                txtCategory.Text = reader["category"] != DBNull.Value ? reader["category"].ToString() : "";
                                txtUnitPrice.Text = Convert.ToDecimal(reader["unit_price"]).ToString("F2");
                                txtCostPrice.Text = reader["cost_price"] != DBNull.Value ? Convert.ToDecimal(reader["cost_price"]).ToString("F2") : "";
                                chkIsActive.Checked = Convert.ToBoolean(reader["is_active"]);
                                txtStock.Text = reader["quantity_in_stock"] != DBNull.Value ? Convert.ToInt32(reader["quantity_in_stock"]).ToString() : "0";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            int productId = _productId;
                            if (_productId == 0)
                            {
                                // Insert new product
                                string insertProductQuery = @"INSERT INTO dbo.products (product_name, category, unit_price, cost_price, is_active, created_at, updated_at)
                                                         OUTPUT INSERTED.product_id
                                                         VALUES (@product_name, @category, @unit_price, @cost_price, @is_active, GETDATE(), GETDATE())";
                                using (SqlCommand command = new SqlCommand(insertProductQuery, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@product_name", txtProductName.Text.Trim());
                                    command.Parameters.AddWithValue("@category", string.IsNullOrEmpty(txtCategory.Text.Trim()) ? (object)DBNull.Value : txtCategory.Text.Trim());
                                    command.Parameters.AddWithValue("@unit_price", Convert.ToDecimal(txtUnitPrice.Text));
                                    command.Parameters.AddWithValue("@cost_price", string.IsNullOrEmpty(txtCostPrice.Text) ? (object)DBNull.Value : Convert.ToDecimal(txtCostPrice.Text));
                                    command.Parameters.AddWithValue("@is_active", chkIsActive.Checked);
                                    productId = (int)command.ExecuteScalar();
                                }

                                // Insert into inventory
                                string insertInventoryQuery = @"INSERT INTO dbo.inventory (product_id, quantity_in_stock, quantity_reserved, quantity_ordered, minimum_stock_level, is_active, created_at, updated_at)
                                                           VALUES (@product_id, @quantity_in_stock, 0, 0, 10, 1, GETDATE(), GETDATE())";
                                using (SqlCommand command = new SqlCommand(insertInventoryQuery, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@product_id", productId);
                                    command.Parameters.AddWithValue("@quantity_in_stock", Convert.ToInt32(txtStock.Text));
                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // Update existing product
                                string updateProductQuery = @"UPDATE dbo.products
                                                         SET product_name = @product_name,
                                                             category = @category,
                                                             unit_price = @unit_price,
                                                             cost_price = @cost_price,
                                                             is_active = @is_active,
                                                             updated_at = GETDATE()
                                                         WHERE product_id = @product_id";
                                using (SqlCommand command = new SqlCommand(updateProductQuery, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@product_id", _productId);
                                    command.Parameters.AddWithValue("@product_name", txtProductName.Text.Trim());
                                    command.Parameters.AddWithValue("@category", string.IsNullOrEmpty(txtCategory.Text.Trim()) ? (object)DBNull.Value : txtCategory.Text.Trim());
                                    command.Parameters.AddWithValue("@unit_price", Convert.ToDecimal(txtUnitPrice.Text));
                                    command.Parameters.AddWithValue("@cost_price", string.IsNullOrEmpty(txtCostPrice.Text) ? (object)DBNull.Value : Convert.ToDecimal(txtCostPrice.Text));
                                    command.Parameters.AddWithValue("@is_active", chkIsActive.Checked);
                                    command.ExecuteNonQuery();
                                }

                                // Update inventory
                                string updateInventoryQuery = @"UPDATE dbo.inventory
                                                           SET quantity_in_stock = @quantity_in_stock,
                                                               updated_at = GETDATE()
                                                           WHERE product_id = @product_id";
                                using (SqlCommand command = new SqlCommand(updateInventoryQuery, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@product_id", _productId);
                                    command.Parameters.AddWithValue("@quantity_in_stock", Convert.ToInt32(txtStock.Text));
                                    command.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                            MessageBox.Show("Menu item saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving menu item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Product name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUnitPrice.Text) || !decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) || unitPrice <= 0)
            {
                MessageBox.Show("Please enter a valid unit price greater than 0.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnitPrice.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtCostPrice.Text) && (!decimal.TryParse(txtCostPrice.Text, out decimal costPrice) || costPrice < 0))
            {
                MessageBox.Show("Please enter a valid cost price (or leave empty).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCostPrice.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtStock.Text) || !int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Please enter a valid stock quantity (0 or greater).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}