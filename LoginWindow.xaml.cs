using System.Data.SqlClient;
using System.Windows;

namespace TransportationSystem
{
    public partial class LoginWindow : Window
    {
        private string connectionString = "Server=XDTUF;Database=TransportationDB;Trusted_Connection=True;";
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM [User] WHERE Email = @Email AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                        cmd.Parameters.AddWithValue("@Password", PasswordBox.Password); // Hash in production
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            UserDashboardWindow userDashboard = new UserDashboardWindow();
                            userDashboard.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Invalid email or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}