using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CafeManagemnt
{
    public partial class AdminForm : Form
    {
        private readonly int _userId;
        private const string ConnectionString = @"Data Source=DESKTOP-5D6TADI;Initial Catalog=CafeManagementDB;Integrated Security=True;Encrypt=False";

        public AdminForm(int userId)
        {
            InitializeComponent();
            _userId = userId;
            this.FormClosing += AdminForm_FormClosing;
        }

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Show confirmation message
            DialogResult result = MessageBox.Show(
                "Are you sure you want to close and log out?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
            {
                // Cancel closing if user selects No
                e.Cancel = true;
                return;
            }

            // User selected Yes, log logout time
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
    }
}