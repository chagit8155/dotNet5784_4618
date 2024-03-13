using System.Windows;
namespace PL.Task;

/// <summary>
/// Interaction logic for TaskForListWindow.xaml
/// </summary>
public partial class TaskForListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public TaskForListWindow()
    {
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








}
