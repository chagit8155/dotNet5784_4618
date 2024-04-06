using BO;
using System.Windows;
using System.Windows.Controls;
namespace PL.Task;

/// <summary>
/// Interaction logic for DependenciesWindow.xaml
/// </summary>
public partial class DependenciesWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.Task currentTask { get; set; }
    public BO.TaskInList DisplayTask { get; set; }
    public List<TaskInList> Add_UpdateDep { get; set; }

    public int EditIsClicked
    {
        get { return (int)GetValue(EditIsClickedProperty); }
        set { SetValue(EditIsClickedProperty, value); }
    }

    // Using a DependencyProperty as the backing store for EditIsClicked.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EditIsClickedProperty =
        DependencyProperty.Register("EditIsClicked", typeof(int), typeof(DependenciesWindow), new PropertyMetadata(1));
    public IEnumerable<BO.TaskInList> Dependencies
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(DependenciesProperty); }
        set { SetValue(DependenciesProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DependenciesProperty =
        DependencyProperty.Register("Dependencies", typeof(IEnumerable<BO.TaskInList>), typeof(DependenciesWindow), new PropertyMetadata(null));

    //public DependenciesWindow()
    //{
    //    InitializeComponent();
    //}
    public bool IsSelected(int id, int idTask)
    {

        return false;

    }
    public DependenciesWindow(BO.Task auTask)
    {
        DisplayTask = new TaskInList() { Id = auTask.Id, Alias = auTask.Alias, Description = auTask.Description, Status = auTask.Status };
        InitializeComponent();
        Dependencies = s_bl.Task.ReadAll(task => task.Id != auTask.Id).OrderBy(task => task.Id);
        currentTask = auTask;
        Add_UpdateDep = new List<TaskInList>();
        //ListBox lb=new ListBox();
        //foreach(var dep in task.Dependencies)
    }


    private void ListBoxItem_Loaded(object sender, RoutedEventArgs e)
    {
        TaskInList? depTask;
        if (sender as ListBoxItem is not null)
        {

            depTask = (sender as ListBoxItem)!.Content as TaskInList;
            if (depTask is not null)
            {
                (sender as ListBoxItem)!.IsSelected = true;
            }

        }
    }


    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        if (EditIsClicked == 1)
            EditIsClicked = 0;
        else
        {

            //if (Add_UpdateDep == currentTask.Dependencies)//אם נשאר באותו מצב
            //    return;
            currentTask.Dependencies = Add_UpdateDep;//+updated dep list
            //new TaskWindow(currentTask).Show();//
            this.Close();
        }
    }

    private void lb_selectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((sender as ListBox) is not null)
        {
            Add_UpdateDep.Clear();
            foreach (var t in ((sender as ListBox)!.SelectedItems))
            {
                Add_UpdateDep.Add((t as TaskInList)!);

            }

        }


    }

    private void ListBox_Loaded(object sender, RoutedEventArgs e)
    {
        if ((sender as ListBox) is not null && currentTask.Dependencies is not null)
        {
            for (int i = 0; i < (sender as ListBox)!.Items.Count; i++)
            {
                TaskInList? depTask = (sender as ListBox)!.Items[i] as TaskInList;
                if (depTask is not null)
                {
                    if (currentTask.Dependencies!.FirstOrDefault(x => x.Id == depTask.Id) is not null)//item נמצא ברשימה של התלויות של task
                    {
                        (sender as ListBox)!.SelectedItems.Add((sender as ListBox)!.Items[i]);
                    }
                }
            }
        }
        }
}

