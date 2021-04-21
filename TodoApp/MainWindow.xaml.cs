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
