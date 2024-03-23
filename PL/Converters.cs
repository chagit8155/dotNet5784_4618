using BO;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
namespace PL;
//class Converters
//{
//}

/// <summary>
/// Converts an ID value to content for display purposes.
/// </summary>
public class ConvertIdToContent : IValueConverter
{
    /// <summary>
    /// Converts the specified value to content based on its ID.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>"Add" if the value is 0, "Update" otherwise.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a text value to visibility.
/// </summary>
public class ConvertTextToVisibility : IValueConverter
{
    /// <summary>
    /// Converts the specified text value to visibility.
    /// </summary>
    /// <param name="value">The text value to convert.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>Visibility.Hidden if the value is 0, Visibility.Visible otherwise.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((int)value == 0)
        {
            return Visibility.Hidden;
        }
        else
        {
            return Visibility.Visible;
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a text value to a boolean value indicating whether it should be enabled.
/// </summary>
public class ConvertTextToIsEnabled : IValueConverter
{
    /// <summary>
    /// Converts the specified text value to a boolean value indicating whether it should be enabled.
    /// </summary>
    /// <param name="value">The text value to convert.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>True if the value is 0, indicating that it should be enabled; otherwise, false.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts a task value to visibility.
/// </summary>
public class ConvertTaskToVisibility : IValueConverter
{
    private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Converts the specified task value to visibility.
    /// </summary>
    /// <param name="value">The task value to convert.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>Visibility.Visible if the project status is Execution; otherwise, Visibility.Hidden.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (s_bl.GetProjectStatus() == ProjectStatus.Execution)
        {
            return Visibility.Visible;
        }
        else
        {
            return Visibility.Hidden;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// /
    /// </summary>


    public class ConvertBooleanToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ConvertStatusToBackground : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Done":
                    return Brushes.LightGreen;
                case "Scheduled":
                    return Brushes.Yellow;
                case "OnTrack":
                    return Brushes.LightSkyBlue;
                case "InJeopredy":
                    return Brushes.LightPink;
                default:
                    return Brushes.White;
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ConvertStatusToForeground : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Done":
                    return Brushes.LightGreen;
                case "Scheduled":
                    return Brushes.Yellow;
                case "OnTrack":
                    return Brushes.LightSkyBlue;
                case "InJeopredy":
                    return Brushes.LightPink;
                case "None":
                    return Brushes.White;
                default:
                    return Brushes.Black;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertTaskInEngineerToText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.TaskInEngineer? engTask = (BO.TaskInEngineer)value;
            return engTask is not null ? engTask.ToString() : "you are not working on any task right now";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Converts text or integer value to visibility for UI purposes.
    /// </summary>
    public class ConvertTextToVisibility : IValueConverter
    {
        /// <summary>
        /// Converts the input value to visibility based on whether it represents "Add" or not.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">An optional parameter.</param>
        /// <param name="culture">The culture to use in the conversion.</param>
        /// <returns>Returns Visibility.Hidden if the value represents "Add" or 0, otherwise returns Visibility.Visible.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((int)value == 0)
            {
                return Visibility.Hidden;
            }
            else
            {
                return Visibility.Visible;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    /// <summary>
    /// Converts text or integer value to a boolean indicating whether an element should be enabled or disabled for UI purposes.
    /// </summary>
    public class ConvertText1ToIsEnabled : IValueConverter
    {
        /// <summary>
        /// Converts the input value to a boolean indicating whether the element should be enabled or disabled.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">An optional parameter.</param>
        /// <param name="culture">The culture to use in the conversion.</param>
        /// <returns>Returns true if the value is 0, indicating "Add"; otherwise, returns false, indicating "Update".</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not null)
                return (int)value == 0;
            return true;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertTextToIsEnabled2 : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 1;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class ConvertByItemIdtoIsSelected : IValueConverter
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.Task currentTask = s_bl.Task.Read((int)parameter);
            if (currentTask.Dependencies?.Count() == 0)
                return false;
            foreach (var dep in currentTask.Dependencies!)
                if (dep.Id == (int)value)
                    return true;
            return false;

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }


    /// <summary>
    /// Converts a list of engineers to visibility for UI purposes.
    /// </summary>
    public class ConvertListToVisibility : IValueConverter
    {

        /// <summary>
        /// Converts the input list of engineers to visibility based on whether it is empty or not.
        /// </summary>
        /// <param name="value">The input list of engineers.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">An optional parameter.</param>
        /// <param name="culture">The culture to use in the conversion.</param>
        /// <returns>Returns Visibility.Visible if the list is empty, otherwise returns Visibility.Hidden.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<BO.Engineer> lstEng = (IEnumerable<BO.Engineer>)value;
            if (lstEng.Count() == 0)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
