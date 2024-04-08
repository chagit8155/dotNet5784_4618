namespace PL.Engineer;
using System.Windows;
/// <summary>
/// Interaction logic for EngineerWindow.xaml
/// </summary>
public partial class EngineerWindow : Window
{
    private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Gets or sets the command (Add/Update) for the engineer window.
    /// </summary>
    public string Command { get; init; }

    /// <summary>
    /// Gets or sets the current engineer displayed in the window.
    /// </summary>
    public BO.Engineer CurrentEngineer
    {
        get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
        set { SetValue(CurrentEngineerProperty, value); }
    }

    /// <summary>
    /// Using a DependencyProperty as the backing store for CurrentEngineer. This enables animation, styling, binding, etc.
    /// </summary>
    public static readonly DependencyProperty CurrentEngineerProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(new BO.Engineer()));

    /// <summary>
    /// Initializes a new instance of the EngineerWindow class.
    /// </summary>
    /// <param name="idEngineer">The ID of the engineer to update. Defaults to 0 for adding a new engineer.</param>
    public EngineerWindow(int idEngineer = 0)
    {
        InitializeComponent();
        Command = (idEngineer == 0) ? "Add" : "Update";
        try
        {
            if (Command == "Add")
                CurrentEngineer = new BO.Engineer();
            else
                CurrentEngineer = s_bl.Engineer.Read(idEngineer);
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

    /// <summary>
    /// Handles the Click event of the btnAddUpdate button.
    /// Adds or updates the current engineer based on the Command property.
    /// </summary>
    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Command == "Add")
            {
                s_bl.Engineer.Create(CurrentEngineer);
                s_bl.Engineer.ReadAll();
                MessageBoxResult successMsg = MessageBox.Show("The new engineer creation is done successfully.");
                this.Close();
            }
            else
            {
                s_bl.Engineer.Update(CurrentEngineer);
                MessageBoxResult successMsg = MessageBox.Show("The engineer update is done successfully.");
                s_bl.Engineer.ReadAll();
                this.Close();

            }
        }
        catch (BO.BlAlreadyExistsException ex)
        {
            MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            s_bl.Engineer.ReadAll();
        }
    }
}
