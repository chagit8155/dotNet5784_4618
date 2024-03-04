using BO;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
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
}
