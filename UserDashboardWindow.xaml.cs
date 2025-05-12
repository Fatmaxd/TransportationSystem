using System;
using System.Data.SqlClient;
using System.Windows;
using System.Collections.ObjectModel;

namespace TransportationSystem
{
    public partial class UserDashboardWindow : Window
    {
        private string userEmail;
        private int userId;
        private string connectionString = "Server=XDTUF;Database=TransportationDB;Trusted_Connection=True;";
        private ObservableCollection<Trip> trips = new ObservableCollection<Trip>();

        public UserDashboardWindow(string email)
        {
            InitializeComponent();
            userEmail = email;
            TripListView.ItemsSource = trips;
            LoadUserData();
            LoadTripHistory();
        }

        private void LoadUserData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ID, Fname, Lname FROM [User] WHERE Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", userEmail);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = (int)reader["ID"];
                                string firstName = reader["Fname"].ToString();
                                string lastName = reader["Lname"].ToString();
                                WelcomeTextBlock.Text = $"Welcome, {firstName} {lastName}!";
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTripHistory()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT PickUp, Destination, PickDate, Duration FROM Trip WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                trips.Add(new Trip
                                {
                                    PickUp = reader["PickUp"].ToString(),
                                    Destination = reader["Destination"].ToString(),
                                    PickDate = (DateTime)reader["PickDate"],
                                    Duration = reader["Duration"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BookTripButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PickUpTextBox.Text) || string.IsNullOrWhiteSpace(DestinationTextBox.Text) || PickDatePicker.SelectedDate == null || string.IsNullOrWhiteSpace(PickTimeTextBox.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string time = PickTimeTextBox.Text;
                if (!TimeSpan.TryParse(time, out TimeSpan parsedTime))
                {
                    MessageBox.Show("Invalid time format. Use HH:mm (e.g., 08:00).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DateTime pickDate = PickDatePicker.SelectedDate.Value.Date.Add(parsedTime);
                DateTime arriveDate = pickDate.AddMinutes(45);
                int fees = 50;
                int payId = 1;
                int driverSsn = 1;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Trip (Fees, PickUp, Destination, PickDate, ArriveDate, PayID, DrSSN, UserID) " +
                                   "VALUES (@Fees, @PickUp, @Destination, @PickDate, @ArriveDate, @PayID, @DrSSN, @UserID)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Fees", fees);
                        cmd.Parameters.AddWithValue("@PickUp", PickUpTextBox.Text);
                        cmd.Parameters.AddWithValue("@Destination", DestinationTextBox.Text);
                        cmd.Parameters.AddWithValue("@PickDate", pickDate);
                        cmd.Parameters.AddWithValue("@ArriveDate", arriveDate);
                        cmd.Parameters.AddWithValue("@PayID", payId);
                        cmd.Parameters.AddWithValue("@DrSSN", driverSsn);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Trip booked successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                trips.Clear();
                LoadTripHistory();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class Trip
    {
        public string PickUp { get; set; }
        public string Destination { get; set; }
        public DateTime PickDate { get; set; }
        public string Duration { get; set; }
    }
}