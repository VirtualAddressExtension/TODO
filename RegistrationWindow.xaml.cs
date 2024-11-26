using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    public partial class RegistrationWindow : Window
    {
        private AppDbContext _context;

        public RegistrationWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
        }

        private static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();
            if (trimmedEmail.EndsWith('.'))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static string ComputeHash(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            byte[] hashedBytes = SHA256.HashData(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var email = EmailTextBox.Text;
            var password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (_context.Users.Any(u => u.Username == username))
            {
                MessageBox.Show("Username already exists.");
                return;
            }

            var passwordHash = ComputeHash(password); 

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Email is invalid");
                return;
            }

            var newUser = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            MessageBox.Show("Registration successful!");
            LoginWindow loginWindow = new();
            loginWindow.Show();
            this.Close();
        }
    }
}
