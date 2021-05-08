using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
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
                return Brushes.Green;
            }
            else if (time.Date < DateTime.Today)
            {
                return Brushes.Red;
            }
            else if (time.Date == DateTime.Today)
            {
                return Brushes.Orange;
            }
            else if (time.Date > DateTime.Today)
            {
                return Brushes.Blue;
            }
            else return Brushes.Black;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
