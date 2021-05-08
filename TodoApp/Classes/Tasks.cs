using System;
using System.Collections.ObjectModel;

namespace TodoApp.Classes
{
    /// <summary>
    /// Represents a group of tasks.
    /// </summary>
    public class TaskGroup
    {
        /// <summary>
        /// The title of the task group.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// A collection that contains the tasks inside this task group.
        /// </summary>
        public ObservableCollection<Task> Tasks { get; set; }
        /// <summary>
        /// A static property, in which we store a reference to the main window. Should be set from the main window itself.
        /// </summary>
        public static MainWindow Window { get; set; } 

        /// <summary>
        /// Constructor, initializes the class.
        /// </summary>
        public TaskGroup()
        {
            Tasks = new ObservableCollection<Task>();
            Tasks.CollectionChanged += Tasks_CollectionChanged;
        }

        private void Tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Window.NotifyPropertyChanged("TasksCount"); // Need to update task count when the task collection of any group changes.
        }
    }

    /// <summary>
    /// Represents a task.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// The name of the task.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The description of the task.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The due date of the task.
        /// </summary>
        public DateTime DueDate { get; set; }
        /// <summary>
        /// The current status of the task.
        /// </summary>
        public TaskStatus Status { get; set; }
    }

    /// <summary>
    /// An enumerable that contains all the possible task statuses.
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// The task is pending.
        /// </summary>
        Pending,
        /// <summary>
        /// The task was completed.
        /// </summary>
        Complete
    }
}
