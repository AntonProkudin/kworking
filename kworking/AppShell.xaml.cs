
namespace kworking;
public partial class AppShell : Shell
{
	public AppShell(/*LoginPage loginPage*/)
	{
		InitializeComponent();

       Routing.RegisterRoute("ListChatPage", typeof(ListChatPage));
       Routing.RegisterRoute("ChatPage", typeof(ChatPage));
       Routing.RegisterRoute("OrdersPage", typeof(OrdersPage));
       Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));
       Routing.RegisterRoute("StockPage", typeof(StockPage));
       Routing.RegisterRoute("NewOrderPage", typeof(NewOrderPage));
        //this.CurrentItem = loginPage;
    }
}