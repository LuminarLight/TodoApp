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
using Microsoft.Win32;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


namespace TodoApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<TaskGroup> groups;

        private string lastPath = "";

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

            groups = new ObservableCollection<TaskGroup>();

            groupList.ItemsSource = groups;
            statusComboBox.ItemsSource = TaskStatusValues;
        }

        private void taskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            Task t = listBox.SelectedItem as Task;

            BindingOperations.SetBinding(titleTextBox, TextBox.TextProperty, new Binding() { Source = t, Path = new PropertyPath("Title"), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            BindingOperations.SetBinding(descTextBox, TextBox.TextProperty, new Binding() { Source = t, Path = new PropertyPath("Description"), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            BindingOperations.SetBinding(dueDatePicker, DatePicker.SelectedDateProperty, new Binding() { Source = t, Path = new PropertyPath("DueDate"), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            BindingOperations.SetBinding(statusComboBox, ComboBox.SelectedItemProperty, new Binding() { Source = t, Path = new PropertyPath("Status"), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        }

        private void ButtonNewTask_Click(object sender, RoutedEventArgs e)
        {
            int i = groupList.SelectedIndex;
            if (i == -1)
            {
                MessageBox.Show("Please select a task group.");
                return;
            }
            groups[i].Tasks.Add(new Task() { Title = "newtask", Description = "", DueDate = DateTime.Now, Status = TaskStatus.Pending });
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

        private void ButtonUpGroup_Click(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as Button) as UIElement;
            var parent2 = VisualTreeHelper.GetParent(parent) as UIElement;
            Grid g = parent2 as Grid;
            TaskGroup t = g.DataContext as TaskGroup;

            for (int i = 0; i < groups.Count; i++)
            {
                bool isthis = groups[i] == t;
                if (isthis && i == 0)
                {
                    Debug.WriteLine("Index is 0, can't move up!");
                    return;
                }
                else if (isthis)
                {
                    TaskGroup temp = groups[i - 1];
                    groups[i - 1] = groups[i];
                    groups[i] = temp;
                    return;
                }
            }
        }

        private void ButtonDownGroup_Click(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as Button) as UIElement;
            var parent2 = VisualTreeHelper.GetParent(parent) as UIElement;
            Grid g = parent2 as Grid;
            TaskGroup t = g.DataContext as TaskGroup;

            for (int i = 0; i < groups.Count; i++)
            {
                bool isthis = groups[i] == t;
                if (isthis && i == groups.Count - 1)
                {
                    Debug.WriteLine("Index is max, can't move down!");
                    return;
                }
                else if (isthis)
                {
                    TaskGroup temp = groups[i + 1];
                    groups[i + 1] = groups[i];
                    groups[i] = temp;
                    return;
                }
            }
        }

        private void ButtonDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this task?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.No) return;

            var parent = VisualTreeHelper.GetParent(sender as Button) as UIElement;
            var parent2 = VisualTreeHelper.GetParent(parent) as UIElement;
            Grid g = parent2 as Grid;
            //MessageBox.Show(g.DataContext.GetType().GetProperty("Title").GetValue(g.DataContext).ToString());
            Task t = g.DataContext as Task;

            foreach (var group in groups)
            {
                if (group.Tasks.Remove(t))
                {
                    return;
                }               
            }
        }

        private void ButtonUpTask_Click(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as Button) as UIElement;
            var parent2 = VisualTreeHelper.GetParent(parent) as UIElement;
            Grid g = parent2 as Grid;
            Task t = g.DataContext as Task;

            foreach (var group in groups)
            {
                for (int i = 0; i < group.Tasks.Count; i++)
                {
                    bool isthis = group.Tasks[i] == t;
                    if (isthis && i == 0)
                    {
                        Debug.WriteLine("Index is 0, can't move up!");
                        return;
                    }
                    else if (isthis)
                    {
                        Task temp = group.Tasks[i - 1];
                        group.Tasks[i - 1] = group.Tasks[i];
                        group.Tasks[i] = temp;
                        return;
                    }
                }
            }           
        }

        private void ButtonDownTask_Click(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as Button) as UIElement;
            var parent2 = VisualTreeHelper.GetParent(parent) as UIElement;
            Grid g = parent2 as Grid;
            Task t = g.DataContext as Task;

            foreach (var group in groups)
            {
                for (int i = 0; i < group.Tasks.Count; i++)
                {
                    bool isthis = group.Tasks[i] == t;
                    if (isthis && i == group.Tasks.Count - 1)
                    {
                        Debug.WriteLine("Index is max, can't move down!");
                        return;
                    }
                    else if (isthis)
                    {
                        Task temp = group.Tasks[i + 1];
                        group.Tasks[i + 1] = group.Tasks[i];
                        group.Tasks[i] = temp;
                        return;
                    }
                }
            }
        }

        ListBox dragSource = null;

        private void taskList_Drop(object sender, DragEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            object data = e.Data.GetData(typeof(Task));
            ((IList<Task>)dragSource.ItemsSource).Remove((Task)data);
            ((IList<Task>)parent.ItemsSource).Add((Task)data);
        }

        private void taskList_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }

        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }

        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {
            groups.Clear();

            lastPath = "";
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { Filter = "TDAX Files (*.tdax)|*.tdax" };

            var result = dialog.ShowDialog();
            if (result == false) return;

            using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<TaskGroup>));

                    groups = (ObservableCollection<TaskGroup>)serializer.Deserialize(fs);

                    groupList.ItemsSource = groups;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error while trying to open the file: " + ex.Message);
                }
            }

            lastPath = dialog.FileName;
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(false);
        }

        private void MenuSaveas_Click(object sender, RoutedEventArgs e)
        {
            SaveFile(true);
        }

        private void SaveFile(bool showdiag)
        {
            string path;
            
            if (File.Exists(lastPath) && !showdiag)
            {
                path = lastPath;
            }
            else
            {
                SaveFileDialog dialog = new SaveFileDialog() { Filter = "TDAX Files (*.tdax)|*.tdax" }; // ToDo App Xml

                var result = dialog.ShowDialog();
                if (result == false) return;
                path = dialog.FileName;
            }

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<TaskGroup>));

                    serializer.Serialize(fs, groups);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error while trying to save the file: " + ex.Message);
                }
            }

            lastPath = path;
        }
    }

    public class TaskGroup
    {
        public string Title { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }

        public TaskGroup()
        {
            Tasks = new ObservableCollection<Task>();
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
