using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using TodoApp.Classes;

namespace TodoApp.Converters
{
    /// <summary>
    /// This value converter converts a task status into an image path.
    /// </summary>
    public class TaskStatusToImagePathConverter : IValueConverter
    {
        /// <summary>
        /// Converts task status into image path.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case TaskStatus.Pending:
                    {
                        return "/Images/TaskPendingIcon.png";
                    }
                case TaskStatus.Complete:
                    {
                        return "/Images/TaskDoneIcon.png";
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// This multi-value converter converts a task status and datetime into a brush.
    /// </summary>
    public class DueDateToColorConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts datetime and task status into brush.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime time = (DateTime)values[0];
            TaskStatus status = (TaskStatus)values[1];

            if (status == TaskStatus.Complete)
            {
                return Brushes.Green; // Return green color if task is complete.
            }
            else if (time.Date < DateTime.Today)
            {
                return Brushes.Red; // Return red color if we are past the due date.
            }
            else if (time.Date == DateTime.Today)
            {
                return Brushes.Orange; // Return orange color if due date is today.
            }
            else if (time.Date > DateTime.Today)
            {
                return Brushes.Blue; // Return blue color if we still have time.
            }
            else return Brushes.Black; // Return black color if none of the above is true, which is impossible.
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
