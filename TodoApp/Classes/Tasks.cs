using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace TodoApp.Classes
{
    public class TaskGroup
    {
        public string Title { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }
        public static MainWindow window { get; set; }

        public TaskGroup()
        {
            Tasks = new ObservableCollection<Task>();
            Tasks.CollectionChanged += Tasks_CollectionChanged;
        }

        private void Tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            window.TasksCount = 0;
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
