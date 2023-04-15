using MvvmHelpers;

namespace kworking.ViewModels;

public class ListChatViewModel : INotifyPropertyChanged, IQueryAttributable //: BaseViewModel
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private ServiceProvider _serviceProvider;
    private ChatHub _chatHub;

    public ListChatViewModel(ServiceProvider serviceProvider, ChatHub chatHub)
    {
        UserInfo = new User();
        UserFriends = new ObservableCollection<User>();
        LastestMessages = new ObservableCollection<LastestMessage>();

        RefreshCommand = new Command(() =>
        {
            Task.Run(async () =>
            {
                IsRefreshing = true;
                await GetListFriends();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        });

        OpenChatPageCommand = new Command<int>(async (param) =>
        {
            await Shell.Current.GoToAsync($"ChatPage?fromUserId={UserInfo.Id}&toUserId={param}");
        });

        _serviceProvider = serviceProvider;
        _chatHub = chatHub;
        _chatHub.Connect();
        _chatHub.AddReceivedMessageHandler(OnReceivedMessage);

        MessagingCenter.Send<string>("StartService", "MessageForegroundService");
        MessagingCenter.Send<string, string[]>("StartService", "MessageNotificationService", new string[] { });

    }

    private User userInfo;
    private ObservableCollection<User> userFriends;
    private ObservableCollection<LastestMessage> lastestMessages;
    private bool isRefreshing;
    public static User USRInfo;
    async Task GetListFriends()
    {
        var response = await _serviceProvider.CallWebApi<int, ListChatInitializeResponse>
            ("/ListChat/Initialize", HttpMethod.Post, UserInfo.Id);

        if (response.StatusCode == 200)
        {
            UserInfo = response.User;
            USRInfo = UserInfo;
            UserFriends = new ObservableCollection<User>(response.UserFriends);
            LastestMessages = new ObservableCollection<LastestMessage>(response.LastestMessages);
        }
        else
        {
            await AppShell.Current.DisplayAlert("Kworking", response.StatusMessage, "OK");
        }
    }

    public void Initialize()
    {
        Task.Run(async () =>
        {
            IsRefreshing = true;
            await GetListFriends();
        }).GetAwaiter().OnCompleted(() =>
        {
            IsRefreshing = false;
        });
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query == null || query.Count == 0) return;

        UserInfo.Id = int.Parse(HttpUtility.UrlDecode(query["userId"].ToString()));
    }

    void OnReceivedMessage(int fromUserId, string message)
    {
        var lastestMessage = LastestMessages.Where(x => x.UserFriendInfo.Id == fromUserId).FirstOrDefault();
        if (lastestMessage != null)
            LastestMessages.Remove(lastestMessage);

        var newLastestMessage = new LastestMessage
        {
            UserId = userInfo.Id,
            Content = message,
            UserFriendInfo = UserFriends.Where(x => x.Id == fromUserId).FirstOrDefault()
        };

        LastestMessages.Insert(0, newLastestMessage);
        OnPropertyChanged("LastestMessages");

        MessagingCenter.Send<string, string[]>("Notify", "MessageNotificationService",
            new string[] { newLastestMessage.UserFriendInfo.UserName, newLastestMessage.Content });
    }

    public User UserInfo
    {
        get { return userInfo; }
        set { userInfo = value; OnPropertyChanged(); }
    }
    public ObservableCollection<User> UserFriends
    {
        get { return userFriends; }
        set { userFriends = value; OnPropertyChanged(); }
    }

    public ObservableCollection<LastestMessage> LastestMessages
    {
        get { return lastestMessages; }
        set { lastestMessages = value;
            OnPropertyChanged("LastestMessages");
            LastestMessage = lastestMessages.FirstOrDefault();
        }
    }

    LastestMessage lastestMessage1 = null;
    public LastestMessage LastestMessage
    {
        get { return lastestMessage1; }
        set {
            lastestMessage1 = value;
            OnPropertyChanged("LastestMessage");
        }
    }
    public bool IsRefreshing
    {
        get { return isRefreshing; }
        set { isRefreshing = value; OnPropertyChanged(); }
    }

    public ICommand RefreshCommand { get; set; }

    public ICommand OpenChatPageCommand { get; set; }
}
