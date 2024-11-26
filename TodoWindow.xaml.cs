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

namespace TODO
{
    public partial class MainWindow : Window
    {
        private AppDbContext _context;
        private User _currentUser;

        public MainWindow(User user)
        {
            InitializeComponent();
            _context = new AppDbContext();
            _currentUser = user;
            LoadTasks();
        }

        private void LoadTasks()
        {
            var tasks = _context.Tasks.Where(t => t.UserId == _currentUser.UserId).ToList();
            TasksListView.ItemsSource = tasks;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var newTask = new Task
            {
                Title = TitleTextBox.Text,
                Description = DescriptionTextBox.Text,
                IsCompleted = false,
                CreatedAt = DateTime.Now,
                UserId = _currentUser.UserId
            };

            _context.Tasks.Add(newTask);
            _context.SaveChanges();
            LoadTasks();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var task = (sender as CheckBox)?.DataContext as Task;
            if (task != null)
            {
                task.IsCompleted = true;
                _context.SaveChanges();
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var task = (sender as CheckBox)?.DataContext as Task;
            if (task != null)
            {
                task.IsCompleted = false;
                _context.SaveChanges();
            }
        }

        private void DeleteCompletedTasksButton_Click(object sender, RoutedEventArgs e)
        {
            var completedTasks = _context.Tasks.Where(t => t.UserId == _currentUser.UserId && t.IsCompleted).ToList();
            _context.Tasks.RemoveRange(completedTasks);
            _context.SaveChanges();
            LoadTasks();
            MessageBox.Show("Completed tasks deleted successfully.");
        }
    }
}