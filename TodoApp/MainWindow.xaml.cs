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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace TodoApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<TaskGroup> groups;

        public IEnumerable<TaskStatus> TaskStatusValues
        {
            get
            {
                return Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            groups = new ObservableCollection<TaskGroup>()
            {
                new TaskGroup() {Title = "Cooking"},
                new TaskGroup() {Title = "Cleaning"}
            };

            groupList.ItemsSource = groups;
            statusComboBox.ItemsSource = TaskStatusValues;
        }

        private void TaskGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid g = sender as Grid;

            BindingOperations.SetBinding(titleTextBox, TextBox.TextProperty, new Binding() { Source = g.DataContext, Path = new PropertyPath("Title"), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            BindingOperations.SetBinding(descTextBox, TextBox.TextProperty, new Binding() { Source = g.DataContext, Path = new PropertyPath("Description"), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            BindingOperations.SetBinding(dueDatePicker, DatePicker.SelectedDateProperty, new Binding() { Source = g.DataContext, Path = new PropertyPath("DueDate"), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            BindingOperations.SetBinding(statusComboBox, ComboBox.SelectedItemProperty, new Binding() { Source = g.DataContext, Path = new PropertyPath("Status"), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        }

        private void GroupGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Grid g = sender as Grid;
        }

        private void ButtonNewTask_Click(object sender, RoutedEventArgs e)
        {
            int i = groupList.SelectedIndex;
            if (i == -1)
            {
                MessageBox.Show("Please select a task group.");
                return;
            }
            groups[i].Tasks.Add(new Task() { Title = "newtask", Description = "newdesc", DueDate = new DateTime(2025, 2, 1), Status = TaskStatus.Pending });
        }

        private void ButtonNewTaskGroup_Click(object sender, RoutedEventArgs e)
        {
            groups.Add(new TaskGroup() { Title = "newgroup" });
        }

        private void ButtonDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this group? This will also delete all the tasks in it.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.No) return;

            var parent = VisualTreeHelper.GetParent(sender as Button) as UIElement;
            Grid g = parent as Grid;
            //MessageBox.Show(g.DataContext.GetType().GetProperty("Title").GetValue(g.DataContext).ToString());
            TaskGroup t = g.DataContext as TaskGroup;
            groups.Remove(t);
        }

        private void ButtonDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this task?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.No) return;

            var parent = VisualTreeHelper.GetParent(sender as Button) as UIElement;
            Grid g = parent as Grid;
            //MessageBox.Show(g.DataContext.GetType().GetProperty("Title").GetValue(g.DataContext).ToString());
            Task t = g.DataContext as Task;
            foreach (var group in groups)
            {
                if (group.Tasks.Remove(t))
                {
                    return;
                }               
            }

            // Should not happen.
            MessageBox.Show("Failed to delete task.");
        }
    }

    public class TaskGroup
    {
        public string Title { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }

        public TaskGroup()
        {
            Tasks = new ObservableCollection<Task>()
            {
                new Task() {Title = "Title1", Description = "desc1", DueDate = new DateTime(2020, 12, 11), Status = TaskStatus.Pending},
                new Task() {Title = "Title2", Description = "desc2", DueDate = new DateTime(2021, 5, 3), Status = TaskStatus.Running}
            };
        }
    }

    public class Task
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
    }

    public enum TaskStatus
    {
        Pending,
        Running,
        Complete
    }
    
}
