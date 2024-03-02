using PL.Engineer;
using System.Windows;
namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Initializes a new instance of the MainWindow class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

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
}