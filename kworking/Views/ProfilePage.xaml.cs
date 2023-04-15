using kworking.ViewModels;

namespace kworking.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfileViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }
    object _resp = LoginViewModel.resp;

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
       
        (this.BindingContext as ProfileViewModel).Initialize();
    }
}
