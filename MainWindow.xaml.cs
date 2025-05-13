using System.Windows;

namespace TransportationSystem
{
    public partial class MainWindow : Window
    {
        private UserDashboardWindow userDashboardWindow;
        private string loggedInUserEmail;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow(this);
            loginWindow.Show();
            this.Hide();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.Show();
            this.Hide();
        }

        private void UserDashboardButton_Click(object sender, RoutedEventArgs e)
        {
            if (loggedInUserEmail == null)
            {
                MessageBox.Show("Please log in first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (userDashboardWindow == null || !userDashboardWindow.IsLoaded)
            {
                userDashboardWindow = new UserDashboardWindow(loggedInUserEmail);
                userDashboardWindow.Closed += (s, args) => userDashboardWindow = null; // Reset when closed
                userDashboardWindow.Show();
            }
            else
            {
                userDashboardWindow.Activate();
            }
        }

        public void SetLoggedInUser(string email)
        {
            loggedInUserEmail = email;
        }

        public void ShowMainWindow()
        {
            this.Show();
        }
    }
}