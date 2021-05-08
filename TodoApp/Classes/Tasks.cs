using System;
using System.Collections.ObjectModel;

namespace TodoApp.Classes
{
    public class TaskGroup
    {
        public string Title { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }
        public static MainWindow Window { get; set; } // A static property, in which we store reference to the main window. Should be set from the main window itself.

        public TaskGroup()
        {
            Tasks = new ObservableCollection<Task>();
            Tasks.CollectionChanged += Tasks_CollectionChanged;
        }

        private void Tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Window.NotifyPropertyChanged("TasksCount"); // Need to update task count when the tasks collection of any group changes.
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
        Complete
    }
}
