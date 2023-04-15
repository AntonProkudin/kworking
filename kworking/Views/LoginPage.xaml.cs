using kworking.ViewModels;

namespace kworking.Views;
public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}
