using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using TodoApp.Classes;

namespace TodoApp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<TaskGroup> groups;

        public event PropertyChangedEventHandler PropertyChanged;

        public int TasksCount
        {
            get
            {
                int sum = 0;
                foreach (var group in groups)
                {
                    sum += group.Tasks.Count;
                }
                return sum; // Returns sum of all tasklists of all task groups.
            }
        }

        public void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string lastPath = "";

        public string LastPath
        {
            get
            {
                return lastPath;
            }
            set
            {
                lastPath = value;

                if (lastPath == "")
                {
                    this.Title = "Todo App";
                }     
                else
                {
                    this.Title = $"Todo App ({lastPath})";
                }
            }
        }


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

            TaskGroup.Window = this; // Setting the 'Window' static property of the TaskGroup class - all TaskGroup instances will use this.

            groups = new ObservableCollection<TaskGroup>();

            groupList.ItemsSource = groups;
            statusComboBox.ItemsSource = TaskStatusValues;

            statusBar.DataContext = this;
        }

        private void taskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            Task t = listBox.SelectedItem as Task;

            editPanel.DataContext = t;

            if (t == null)
            {
                editPanel.Visibility = Visibility.Hidden; // Hide the edit panel if no task is selected.
            }
            else
            {
                editPanel.Visibility = Visibility.Visible;
            }
        }

        private void ButtonNewTask_Click(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as Button) as UIElement;
            var parent2 = VisualTreeHelper.GetParent(parent) as UIElement;
            Grid g = parent2 as Grid;
            TaskGroup t = g.DataContext as TaskGroup;

            t.Tasks.Add(new Task() { Title = "newtask", Description = "", DueDate = DateTime.Now, Status = TaskStatus.Pending });
        }

        private void ButtonNewTaskGroup_Click(object sender, RoutedEventArgs e)
        {
            groups.Add(new TaskGroup() { Title = "newgroup" });
        }

        private void ButtonDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as Button) as UIElement;
            var parent2 = VisualTreeHelper.GetParent(parent) as UIElement;
            Grid g = parent2 as Grid;
            TaskGroup t = g.DataContext as TaskGroup;

            if (t.Tasks.Count > 0) // Only ask for confirmation if the group has tasks in it.
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this group? This will also delete all the tasks in it.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (result == MessageBoxResult.No) return;
            }

            groups.Remove(t);

            NotifyPropertyChanged("TasksCount"); // Need to re-calculate tasks count.
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
            Task t = g.DataContext as Task;

            foreach (var group in groups)
            {
                if (group.Tasks.Remove(t))
                {
                    return; // Stop looking if the task was found and successfully deleted.
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
            if (groups.Count > 0) // Only ask for confirmation if there is something in the application.
            {
                MessageBoxResult result = MessageBox.Show("You are about to delete all groups and tasks. This action is irreversible. Are you sure you want to do this?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (result == MessageBoxResult.No) return;
            }

            foreach (var group in groups)
            {
                group.Tasks.Clear();
            }

            groups.Clear();

            LastPath = "";
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

            LastPath = dialog.FileName;
            NotifyPropertyChanged("TasksCount"); // Need to re-calculate task count after opening a file.
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

            if (File.Exists(LastPath) && !showdiag) // No need to show file dialog if person clicked on Save and the last path is valid.
            {
                path = LastPath;
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

            LastPath = path;
        }

        private ComboBox combo = new ComboBox(); // Had to create a hidden ComboBox for this to work properly:

        private void TaskImage_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as DependencyObject) as UIElement;
            Grid g = parent as Grid;

            Task t = g.DataContext as Task;

            BindingOperations.SetBinding(combo, ComboBox.SelectedItemProperty, new Binding() { Source = t, Path = new PropertyPath("Status"), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            if (t.Status == TaskStatus.Pending) combo.SelectedItem = TaskStatus.Complete;
            else if (t.Status == TaskStatus.Complete) combo.SelectedItem = TaskStatus.Pending;
        }
    }    
}
