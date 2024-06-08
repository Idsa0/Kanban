using System.Windows;
using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Frontend.ViewModel;

namespace IntroSE.Kanban.Frontend.View;

public partial class LoginWindow
{
    private LoginVM vm;

    public LoginWindow()
    {
        InitializeComponent();
        vm = new LoginVM();
        Grid.DataContext = vm;
    }

    public LoginWindow(BackendController controller)
    {
        InitializeComponent();
        vm = new LoginVM(controller);
        Grid.DataContext = vm;
    }

    internal LoginWindow(LoginVM vm)
    {
        InitializeComponent();
        this.vm = vm;
        Grid.DataContext = vm;
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        UserModel? user = vm.Login();
        if (user == null)
        {
            MessageBox.Show(vm.ErrorMessage);
            return;
        }
        
        UserView uv = new UserView(user);
        uv.Show();
        Close();
    }

    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        RegisterView registerView = new RegisterView(vm);
        registerView.Show();
        Close();
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        vm.Password = PasswordBox.Password;
    }
}