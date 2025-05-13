using System.Data.SqlClient;
using System.Windows;

namespace TransportationSystem
{
    public partial class SignUpWindow : Window
    {
        private string connectionString = "Server=XDTUF;Database=TransportationDB;Trusted_Connection=True;";

        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) || string.IsNullOrWhiteSpace(LastNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(EmailTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                    string.IsNullOrWhiteSpace(PhoneTextBox.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO [User] (Fname, Lname, Email, Password, Phone) VALUES (@Fname, @Lname, @Email, @Password, @Phone)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Fname", FirstNameTextBox.Text);
                        cmd.Parameters.AddWithValue("@Lname", LastNameTextBox.Text);
                        cmd.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                        cmd.Parameters.AddWithValue("@Password", PasswordBox.Password);
                        cmd.Parameters.AddWithValue("@Phone", PhoneTextBox.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Sign up successful! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is MainWindow mainWindow)
                    {
                        mainWindow.Show();
                        break;
                    }
                }
                this.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}