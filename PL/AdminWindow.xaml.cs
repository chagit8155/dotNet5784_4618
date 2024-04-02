using PL.Engineer;
using PL.Gant;
using PL.Task;
using System.Windows;
namespace PL;

/// <summary>
/// Interaction logic for AdminWindow.xaml
/// </summary>
public partial class AdminWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public AdminWindow()
    {
        InitializeComponent();
        startProject = s_bl.StartProjectDate;
    }

    public DateTime? startProject
    {
        get { return (DateTime?)GetValue(startProjectProperty); }
        set { SetValue(startProjectProperty, value); }
    }

    // Using a DependencyProperty as the backing store for startProject.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty startProjectProperty =
        DependencyProperty.Register("startProject", typeof(DateTime?), typeof(AdminWindow), new PropertyMetadata(null));

    /// <summary>
    /// Handles the Click event of the btnEngineers button.
    /// Opens the EngineerListWindow.
    /// </summary>
    private void btnEngineers_Click(object sender, RoutedEventArgs e)
    {
        new EngineerListWindow().Show();
    }

    /// <summary>
    /// Handles the Click event of the btnInitialize button.
    /// Asks the user if they would like to create initial data, and initializes the database if they confirm.
    /// </summary>
    private void btnInitialize_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult mbResult = MessageBox.Show("Would you like to create Initial data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (mbResult == MessageBoxResult.Yes)
        {
            s_bl.ResetDB();
            s_bl.InitializeDB();
        }
    }

    /// <summary>
    /// Handles the Click event of the btnReset button.
    /// Asks the user if they would like to reset the data, and resets the database if they confirm.
    /// </summary>
    private void btnReset_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult mbResult = MessageBox.Show("Would you like to reset the data?", "Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (mbResult == MessageBoxResult.Yes)
        {
            s_bl.ResetDB();
        }
    }

    private void btnTasks_Click(object sender, RoutedEventArgs e)
    {
        new TaskForListWindow().Show();
    }

    private void btnGant_Click(object sender, RoutedEventArgs e)
    {
        new GantWindow().ShowDialog();
    }

    private void btnSchadule_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            s_bl.StartProjectDate = startProject;
            MessageBoxResult mbResult = MessageBox.Show("Would you like to create the schedule automatically?", "Create Schedule Option", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (mbResult == MessageBoxResult.Yes)
            {
                try
                {
                    s_bl.CreateSchedule(s_bl.Clock, BO.CreateScheduleOption.Automatically);
                    MessageBox.Show("The scheduled has created successfuly!");
                }
                catch (BO.BlInvalidInputFormatException ex)
                {
                    MessageBoxResult mbResultEx = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                catch (BO.BlDoesNotExistException ex)
                {
                    MessageBoxResult mbResultEx = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch
                {
                    MessageBoxResult mbResultEx = MessageBox.Show("UnKnown error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (mbResult == MessageBoxResult.No)
            {
                new ScheduleWindow().ShowDialog();
            }
        }
        catch (BO.BlCannotBeUpdatedException ex)

        {
            MessageBoxResult mbResultEx = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
