
namespace kworking.Views;

public partial class NewOrderPage : ContentPage
{
	public NewOrderPage(NewOrderViewModel viewModel)
	{
		InitializeComponent();
        this.BindingContext = viewModel;
    }
}