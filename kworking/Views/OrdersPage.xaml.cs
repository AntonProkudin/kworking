using kworking.ViewModels;
using Microsoft.Maui.Graphics.Text;

namespace kworking.Views;

public partial class OrdersPage : ContentPage
{
    public OrdersPage(OrdersViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (this.BindingContext as OrdersViewModel).Initialize();
    }
}