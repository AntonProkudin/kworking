
using Microsoft.Maui;

namespace kworking.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private ServiceProvider _serviceProvider;

    public LoginViewModel(ServiceProvider serviceProvider)
    {
        UserName = "Anton";
        Password = "Abc12345";
        IsProcessing = false;

        LoginCommand = new Command(() =>
        {
            if (IsProcessing) return;

            if (UserName.Trim() == "" || Password.Trim() == "") return;

            IsProcessing = true;
            Login().GetAwaiter().OnCompleted(() =>
            {
                IsProcessing = false;
            });
        });
        this._serviceProvider = serviceProvider;
    }

    public static int resp;
    public static class Holder
    {
        public static int _resp {
            get { return resp; }
            set { resp = value; }
}
    }
    async Task Login()
    {
        try
        {
            var request = new AuthenticateRequest
            {
                LoginId = UserName,
                Password = Password,
            };
            var response = await _serviceProvider.Authenticate(request);
            if (response.StatusCode == 200)
            { 
                resp = response.Id;
                await Shell.Current.GoToAsync($"ListChatPage?userId={response.Id}");
               
            }
            else
            {
                await AppShell.Current.DisplayAlert("ChatApp", response.StatusMessage, "OK");
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("ChatApp", ex.Message, "OK");
        }
    }

    private string userName;
    private string password;
    private bool isProcessing;

    public string UserName
    {
        get { return userName; }
        set { userName = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get { return password; }
        set { password = value; OnPropertyChanged(); }
    }

    public bool IsProcessing
    {
        get { return isProcessing; }
        set { isProcessing = value; OnPropertyChanged(); }
    }

    public ICommand LoginCommand { get; set; }
}