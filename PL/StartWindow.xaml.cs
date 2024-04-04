using Microsoft.VisualBasic;
using PL.Engineer;
using System.Windows;
using System.Windows.Controls;
namespace PL;

/// <summary>
/// Interaction logic for StartWindow.xaml
/// </summary>
public partial class StartWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


    public StartWindow()
    {
        InitializeComponent();
        CurrentTime = s_bl.Clock;

    }

    //public string EngineerId
    //{
    //    get { return (string)GetValue(EngineerIdProperty); }
    //    set { SetValue(EngineerIdProperty, value); }
    //}

    //// Using a DependencyProperty as the backing store for EngineerId.  This enables animation, styling, binding, etc...
    //public static readonly DependencyProperty EngineerIdProperty =
    //    DependencyProperty.Register("EngineerId", typeof(string), typeof(StartWindow), new PropertyMetadata(""));



    public BO.ProjectStatus CurrentProjectStatus
    {
        get { return (BO.ProjectStatus)GetValue(CurrentProjectStatusProperty); }
        set { SetValue(CurrentProjectStatusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentProjectStatus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentProjectStatusProperty =
        DependencyProperty.Register("CurrentProjectStatus", typeof(BO.ProjectStatus), typeof(StartWindow), new PropertyMetadata(BO.ProjectStatus.Planing));


    public DateTime CurrentTime
    {
        get { return (DateTime)GetValue(CurrentTimeProperty); }
        set { SetValue(CurrentTimeProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTimeProperty =
        DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(StartWindow), new PropertyMetadata(null));

    private void btnAddHour_click(object sender, RoutedEventArgs e)
    {
        s_bl.PromoteTime(BO.Time.Hour);
        CurrentTime = s_bl.Clock;
    }

    private void btnAddDay_click(object sender, RoutedEventArgs e)
    {
        s_bl.PromoteTime(BO.Time.Day);
        CurrentTime = s_bl.Clock;
    }

    private void btnAddYear_click(object sender, RoutedEventArgs e)
    {
        s_bl.PromoteTime(BO.Time.Year);
        CurrentTime = s_bl.Clock;
    }

  
    

    private void BtnLogInAsAdmin_Click(object sender, RoutedEventArgs e)
    {
        new AdminWindow().Show();
    }

  

    private void BtnLogInAsEngineer_Click(object sender, RoutedEventArgs e)
    {
        string id = Interaction.InputBox("Enter your Id", "Hello engineer!", "0");
        try
        {
            if (!string.IsNullOrEmpty(id))
            {
                BO.Engineer engineer = s_bl.Engineer.Read(t => id == t.Id.ToString()); //if not exist thorws ex
                new EngineerMainWindow(engineer.Id).Show();
            }
        }
        catch (BO.BlDoesNotExistException ex)
        {
            MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //if (mbResult == MessageBoxResult.OK)
            //{
            //    new StartWindow().Show();
            //}
        }

    }

    private void Button_StylusButtonDown(object sender, System.Windows.Input.StylusButtonEventArgs e)
    {

    }
}
