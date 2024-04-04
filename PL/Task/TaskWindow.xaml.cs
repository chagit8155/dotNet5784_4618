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
      
        Command = (idTask == 0) ? "Add" : "Update";
        try
        {
            if (Command == "Add")
            {
                CurrentTask = new BO.Task();
                Add_updateEng = new BO.EngineerInTask();

            }

            else
            { 
                CurrentTask = s_bl.Task.Read(idTask);
                Add_updateEng = CurrentTask.Engineer ?? new BO.EngineerInTask();
            }
        }
        catch (BO.BlDoesNotExistException ex)
        {
            MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            if (mbResult == MessageBoxResult.OK)
            {
                //  new EngineerListWindow().Show();
                new TaskForListWindow().Show();
            }
        }
        InitializeComponent();
    }

    public TaskWindow(BO.Task au_Task)
    {
        Command = au_Task.Id == 0 ? "Add" : "Update";
        // flagDependencyUpdated = true;
        //אם מגיעים לפה אז addupdateTask בטוח מאותחל
        CurrentTask = au_Task;
        InitializeComponent();
    }

    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(add_updateTaskProperty); }
        set { SetValue(add_updateTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for add_updateTask.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty add_updateTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(new BO.Task()));

    public BO.EngineerInTask Add_updateEng
    {
        get { return (BO.EngineerInTask)GetValue(Add_updateEngProperty); }
        set { SetValue(Add_updateEngProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Add_updateEng.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty Add_updateEngProperty =
        DependencyProperty.Register("Add_updateEng", typeof(BO.EngineerInTask), typeof(TaskWindow), new PropertyMetadata(null));

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
                if (Add_updateEng.Id != 0)
                {
                    BO.Engineer eng = s_bl.Engineer.Read(t=> t.Id == Add_updateEng.Id);
                    CurrentTask.Engineer = new BO.EngineerInTask() { Id = eng.Id, Name = eng.Name };
                }

                s_bl.Task.Update(CurrentTask);
                MessageBoxResult successMsg = MessageBox.Show("The Task updated successfully!");

                //s_bl.Task.Update(CurrentTask);
                //MessageBoxResult successMsg = MessageBox.Show("The task update is done successfully.");
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
        finally
        {
            this.Close();
            s_bl.Task.ReadAll();
        }
    }



    private void AddDependency_Click(object sender, RoutedEventArgs e)
    {
        new DependenciesWindow(CurrentTask, this).ShowDialog();
    }
}