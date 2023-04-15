using kworking.ViewModels;

namespace kworking.Views;
public partial class ChatPage : ContentPage
{
    public ChatPage(ChatViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (this.BindingContext as ChatViewModel).Initialize();
    }
}
