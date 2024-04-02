using PL.Task;
using System.Windows;
using System.Windows.Input;
namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerMainWindow.xaml
/// </summary>
public partial class EngineerMainWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public EngineerMainWindow(int EngineerId)
    {
        InitializeComponent();
        CurrentEngineer = s_bl.Engineer.Read(EngineerId);
    }

    public BO.Engineer CurrentEngineer
    {
        get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
        set { SetValue(CurrentEngineerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentEngineerProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerMainWindow), new PropertyMetadata(null));


    private void btnCompleteTask_click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult mbResult = MessageBox.Show(
            "Have you finished the task completely?", "Validation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (mbResult == MessageBoxResult.Yes)
        {
            BO.Engineer updateEng = CurrentEngineer;
            updateEng.Task = null;
            s_bl.Engineer.Update(CurrentEngineer);
            CurrentEngineer = s_bl.Engineer.Read(updateEng.Id);
        }
    }

    private void btnChooseNewTask_click(object sender, RoutedEventArgs e)
    {
        if (CurrentEngineer.Task is not null)
        {
            MessageBoxResult mbResult = MessageBox.Show(
                "This action will End your current Mission, are you sure you want to choose new task?", "Validation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (mbResult == MessageBoxResult.No)
            {
                return;
            }
            btnCompleteTask_click(sender, e);
        }
        new TaskForListWindow(CurrentEngineer).Show();
        this.Close();
        
    }



}
