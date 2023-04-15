namespace kworking.ViewModels;

public class NewOrderViewModel : INotifyPropertyChanged, IQueryAttributable
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

    public NewOrderViewModel(ServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        NewOrdersCommand = new Command(async () =>
        {
            await NewOrder();
        });
    }
    async Task NewOrder()
    {
        //if (Name!=null && Description!=null && Category!=null && Price!=null)
        //{ 
           var request = new OrderAddRequest
              {
                  FromUserId = LoginViewModel.Holder._resp,
                  Name = Name,
                  Description = Description,
                  Category = Category,
                  Price = Convert.ToDecimal(Price)
    };
          var response = await _serviceProvider.CallWebApi<OrderAddRequest, OrderAddResponse>
          ("/Order/Add", HttpMethod.Post, request);
        if (response != null) {
        Shell.Current.SendBackButtonPressed();
          }
       // }
    }
   

    private int fromUserId;
    private string name;
    private string description;
    private string category;
    private string price;
    private string order;

    public string Name
    {
        get { return name; }
        set { name = value; OnPropertyChanged(); }
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

    public string Order
    {
        get { return order; }
        set { order = value; OnPropertyChanged(); }
    }
    public ICommand NewOrdersCommand { get; set; }
}