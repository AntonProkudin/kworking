using kworking.ViewModels;

namespace kworking.Views
{
    public partial class StockPage : ContentPage
    {
        public StockPage(StockViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            (this.BindingContext as StockViewModel).Initialize();
        }
    }
}
