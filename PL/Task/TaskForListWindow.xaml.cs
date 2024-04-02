using BO;
using PL.Engineer;
using System.Windows;
using System.Windows.Controls;
namespace PL.Task;

/// <summary>
/// Interaction logic for TaskForListWindow.xaml
/// </summary>
public partial class TaskForListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.EngineerExperience Complexity { get; set; } = BO.EngineerExperience.None;
    public BO.Status Status { get; set; } = BO.Status.Unscheduled;
    public Func<BO.Task, bool>? Filter { get; set; }
    public int IdEngineer { get; set; }

    public TaskForListWindow()
    {
        IdEngineer = 0;
        InitializeComponent();
    }
    public TaskForListWindow(BO.Engineer engineer)
    {
        Filter = item => item.Engineer is null && s_bl.Task.AreAllPreviousTasksCompleted(item.Id);
        IdEngineer = engineer.Id;
        InitializeComponent();
    }

    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskInListProperty); }
        set { SetValue(TaskInListProperty, value); }
    }

    // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskInListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskForListWindow), new PropertyMetadata(null));

    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(SelectedTaskProperty); }
        set { SetValue(SelectedTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SelectedTask.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectedTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskForListWindow), new PropertyMetadata(null));

    public DateTime StartDate
    {
        get { return (DateTime)GetValue(FilterStartDateProperty); }
        set { SetValue(FilterStartDateProperty, value); }
    }

    // Using a DependencyProperty as the backing store for FilterStartDate.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilterStartDateProperty =
        DependencyProperty.Register("StartDate", typeof(DateTime), typeof(TaskForListWindow), new PropertyMetadata(null));


   

    private void lvSelectTaskToUpdate_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;

        MessageBoxResult mbResult = MessageBox.Show($"Are you sure you want to choose the task {task!.Alias}?", "Validation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        if (mbResult == MessageBoxResult.OK)
        {
            BO.Task updateTask = s_bl.Task.Read(task!.Id);
            BO.EngineerInTask engInTask = new BO.EngineerInTask() { Id = IdEngineer };
            updateTask!.Engineer = engInTask;
            s_bl.Task.Update(updateTask);
            this.Close();
            new EngineerMainWindow(IdEngineer).Show();
        }
        new TaskWindow(task?.Id ?? 0).ShowDialog();
        TaskList = s_bl.Task.ReadAll(Filter);
    }

    private void CbFilterByLevel_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        TaskList = Complexity == BO.EngineerExperience.None ? s_bl.Task.ReadAll(Filter) : s_bl.Task.ReadAll(task => task.Complexity == Complexity);
    }
    private void btnFilterByStartDate_Click(object sender, RoutedEventArgs e)
    {
        TaskList = s_bl.Task.ReadAll(task => task.StartDate == StartDate);
    }
    private void CbFilterByStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        TaskList = Status == BO.Status.Unscheduled ? s_bl.Task.ReadAll(Filter) : s_bl.Task.ReadAll(task => task.Status == Status);

    }

    private void btnDeleteTask_Click(object sender, RoutedEventArgs e)
    {
        if (CurrentTask is not null)
            s_bl.Task.Delete(CurrentTask.Id);
        MessageBoxResult successMsg = MessageBox.Show("The task was deleted successfully");
        TaskList = s_bl.Task.ReadAll();
    }

    private void wLoadTheUpdatedTasksList_Loaded(object sender, RoutedEventArgs e)
    {
        TaskList = s_bl.Task.ReadAll(Filter);
    }

    private void btnAddTask_Click(object sender, RoutedEventArgs e)
    {
        new TaskWindow().ShowDialog();
        TaskList = s_bl.Task.ReadAll();
    }

    private void selectTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
        if (task is not null)
            CurrentTask = s_bl.Task.Read(task.Id);
    }

   
}