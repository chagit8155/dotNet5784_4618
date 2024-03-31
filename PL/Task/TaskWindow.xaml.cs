using PL.Engineer;
using System.Windows;
namespace PL.Task;

/// <summary>
/// Interaction logic for TaskWindow.xaml
/// </summary>
public partial class TaskWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public string Command { get; init; }
    public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;
    public TaskWindow(int idTask = 0)
    {
        InitializeComponent();
        Command = (idTask == 0) ? "Add" : "Update";
        try
        {
            if (Command == "Add")
                CurrentTask = new BO.Task();
            else
                CurrentTask = s_bl.Task.Read(idTask);
        }
        catch (BO.BlDoesNotExistException ex)
        {
            MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            if (mbResult == MessageBoxResult.OK)
            {
                new EngineerListWindow().Show();
            }
        }
    }
    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(add_updateTaskProperty); }
        set { SetValue(add_updateTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for add_updateTask.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty add_updateTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(new BO.Task()));

    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Command == "Add")
            {
                s_bl.Task.Create(CurrentTask);
                MessageBoxResult successMsg = MessageBox.Show("The new task creation is done successfully.");
            }
            else
            {
                s_bl.Task.Update(CurrentTask);
                MessageBoxResult successMsg = MessageBox.Show("The task update is done successfully.");
            }
        }
        catch (BO.BlInvalidInputFormatException ex)
        {
            MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (BO.BlCannotBeUpdatedException ex)
        {
            MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        this.Close();
        s_bl.Task.ReadAll();
    }


}