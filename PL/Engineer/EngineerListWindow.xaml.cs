using System.Windows;
using System.Windows.Controls;
namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Gets or sets the selected engineer level filter.
    /// </summary>
    public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;

    /// <summary>
    /// Gets or sets the list of engineers displayed in the window.
    /// </summary>
    public IEnumerable<BO.Engineer> EngineerList
    {
        get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    /// <summary>
    /// Dependency property for the EngineerList property.
    /// </summary>
    public static readonly DependencyProperty EngineerListProperty =
    DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

    /// <summary>
    /// Initializes a new instance of the EngineerListWindow class.
    /// </summary>
    public EngineerListWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the SelectionChanged event of the cbEngineerExperienceSelector ComboBox.
    /// Updates the EngineerList based on the selected engineer level filter.
    /// </summary>
    private void cbEngineerExperienceSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        EngineerList = (Level == BO.EngineerExperience.None) ? s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == Level)!;
    }

    /// <summary>
    /// Handles the Click event of the btnAddNewEngineer button.
    /// Opens the EngineerWindow to add a new engineer.
    /// </summary>
    private void btnAddNewEngineer_Click(object sender, RoutedEventArgs e)
    {
        new EngineerWindow().ShowDialog();
    }

    /// <summary>
    /// Handles the MouseDoubleClick event of the mdcUpdeteCurrentEngineer ListView.
    /// Opens the EngineerWindow to update the selected engineer.
    /// </summary>
    private void mdcUpdeteCurrentEngineer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
        new EngineerWindow(engineer?.Id ?? 0).ShowDialog();
        EngineerList = s_bl?.Engineer.ReadAll()!;
    }

    /// <summary>
    /// Handles the Loaded event of the loadTheUpdatedEngineersList UserControl.
    /// Loads the updated list of engineers when the window is loaded.
    /// </summary>
    private void loadTheUpdatedEngineersList_Loaded(object sender, RoutedEventArgs e)
    {
        EngineerList = s_bl.Engineer.ReadAll();
    }
}
