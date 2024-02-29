namespace PL.Engineer;
using System.Windows;
/// <summary>
/// Interaction logic for EngineerWindow.xaml
/// </summary>
public partial class EngineerWindow : Window
{
    private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public string State { get; init; }
    public BO.Engineer CurrentEngineer
    {
        get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
        set { SetValue(CurrentEngineerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentEngineerProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(new BO.Engineer()));


    public EngineerWindow(int idEngineer = 0)
    {
        InitializeComponent();
        try
        {
            if (idEngineer == 0)
            {
                State = "Add";
                CurrentEngineer = new BO.Engineer();
            }
            else
            {
                State = "Update";
                CurrentEngineer = s_bl.Engineer.Read(idEngineer);
            }
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

    private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (State == "Add")
            {
                s_bl.Engineer.Create(CurrentEngineer);
                MessageBoxResult successMsg = MessageBox.Show("The new engineer creation is done successfully.");
            }
            else
            {
                s_bl.Engineer.Update(CurrentEngineer);
                MessageBoxResult successMsg = MessageBox.Show("The engineer update is done successfully.");
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
        this.Close();
        s_bl.Engineer.ReadAll();
    }
}
