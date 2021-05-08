using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using TodoApp.Classes;

namespace TodoApp.Converters
{
    public class TaskStatusToImagePathConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DueDateToColorConverter : IMultiValueConverter
    {
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

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
