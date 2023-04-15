
using Microsoft.Maui;
using MvvmHelpers;

namespace kworking.ViewModels;

public partial class OrdersViewModel : INotifyPropertyChanged, IQueryAttributable
{
    public event PropertyChangedEventHandler PropertyChanged;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        FromUserId = int.Parse(HttpUtility.UrlDecode(query["fromUserId"].ToString()));
    }

    private ServiceProvider _serviceProvider;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public OrdersViewModel(ServiceProvider serviceProvider)
    {
        Orders = new ObservableCollection<Order>();
        _serviceProvider = serviceProvider;

        RefreshCommand = new Command(() =>
        {
            Task.Run(async () =>
            {
                IsRefreshing = true;
                await GetOrders();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        });
        NewOrderCommand = new Command(async() =>
        {
            await Shell.Current.GoToAsync($"NewOrderPage");
        });
        DeleteOrderCommand = new Command(async(param) =>
        {
            await DeleteOrder((int)param);
        });
    }
    async Task GetOrders()
    {
        var response = await _serviceProvider.CallWebApi<int, OrderInitializeResponse>
          ("/Order/Initialize", HttpMethod.Post, LoginViewModel.Holder._resp );

        if (response.StatusCode == 200)
        {
            Orders = new ObservableCollection<Order>(response.Orders);
        }
        else
        {
            await AppShell.Current.DisplayAlert("Kworking", response.StatusMessage, "OK");
        }

    }
    async Task DeleteOrder(int param)
    {
       // int Id = param.OrderId;
       // await AppShell.Current.DisplayAlert("Kworking","Нажато", "OK");
          var response = await _serviceProvider.CallWebApi<int, OrderAddResponse>
           ("/Order/Delete", HttpMethod.Post, param);
        if (response.StatusCode == 1)
        {
            Task.Run(async () =>
            {
                IsRefreshing = true;
                await GetOrders();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        }
    }
    public void Initialize()
    {
        Task.Run(async () =>
        {
            IsRefreshing = true;
            await GetOrders();
        }).GetAwaiter().OnCompleted(() =>
        {
            IsRefreshing = false;
        });
    }
    private int id;
    private int fromUserId;
    private string name;
    private string description;
    private string category;
    private string price;
    private ObservableCollection<Order> orders;
    private bool isRefreshing;
    private string order;

    public string Name
    {
        get { return name; }
        set { name = value; OnPropertyChanged(); }
    }
    public int Id
    {
        get { return id; }
        set { id = value; OnPropertyChanged(); }
    }
    public string Price
    {
        get { return price; }
        set { price = value; OnPropertyChanged(); }
    }
    public string Description
    {
        get { return description; }
        set { description = value; OnPropertyChanged(); }
    }
    public string Category
    {
        get { return category; }
        set { category = value; OnPropertyChanged(); }
    }
    public int FromUserId
    {
        get { return fromUserId; }
        set { fromUserId = value; OnPropertyChanged(); }
    }
    public ObservableCollection<Order> Orders
    {
        get { return orders; }
        set { orders = value; OnPropertyChanged(); }
    }

    public bool IsRefreshing
    {
        get { return isRefreshing; }
        set { isRefreshing = value; OnPropertyChanged(); }
    }

    public string Order
    {
        get { return order; }
        set { order = value; OnPropertyChanged(); }
    }
    public ICommand RefreshCommand { get; set; }
    public ICommand NewOrderCommand { get; set; }
    public ICommand DeleteOrderCommand { get; set; }
}