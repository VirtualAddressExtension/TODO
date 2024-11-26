using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TODO
{
    public partial class LoginWindow : Window
    {
        private AppDbContext _context;

        public LoginWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var passwordHash = RegistrationWindow.ComputeHash(PasswordBox.Password);

            User user;
            try
            {
                user = _context.Users.Single(u => u.Username == username);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("User with that username doesn't exist!");
                return;
            }

            if (user != null && VerifyPassword(passwordHash, user.PasswordHash))
            {
                MainWindow mainWindow = new(user);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return password == storedHash;
        }
    }

}
