using System.Windows;
namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{  
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;
   
    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }
    public static readonly DependencyProperty EngineerListProperty =
    DependencyProperty.Register("CourseList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));
    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = s_bl?.Engineer.ReadAll()!;
    }
  

    private void cbEngineerExperienceSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        EngineerList = (Level == BO.EngineerExperience.None) ?
             s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == Level)!;

    }

    private void btnAddEngineer_Click(object sender, RoutedEventArgs e)
    {
        new EngineerWindow().ShowDialog();
    }
}
