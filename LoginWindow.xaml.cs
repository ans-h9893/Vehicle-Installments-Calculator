using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace A2AnshChhabra
{

    public partial class LoginWindow : Window
    {
            private readonly Dictionary<string, string> users = new Dictionary<string, string>
        {
            { "jhondonald", "123" },
            { "sickboy", "12345" }
        };

        public LoginWindow()
        {
            InitializeComponent();
            usernameBox.Focus();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameBox.Text;
            string password = passwordBox.Text;

            if (users.ContainsKey(username) && users[username] == password)
            {
                ApplicationWindow loanWindow = new ApplicationWindow();
                loanWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                usernameBox.Clear();
                passwordBox.Clear();
                usernameBox.Focus();
            }
        }

        private void CancelButton_Click(Object sender, RoutedEventArgs e)
        {
            usernameBox.Clear();
            passwordBox.Clear();
            this.Close();
        }
    }
}