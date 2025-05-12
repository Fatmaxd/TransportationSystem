using System.Windows;

namespace TransportationSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

        // Temporarily comment out these methods
        /*
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.Show();
        }

        private void UserDashboardButton_Click(object sender, RoutedEventArgs e)
        {
            UserDashboardWindow userDashboard = new UserDashboardWindow();
            userDashboard.Show();
        }

        private void DriverDashboardButton_Click(object sender, RoutedEventArgs e)
        {
            DriverDashboardWindow driverDashboard = new DriverDashboardWindow();
            driverDashboard.Show();
        }
        */
    }
}