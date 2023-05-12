using kworking.Services.Profile;
using MvvmHelpers;
namespace kworking.ViewModels;
public class ProfileViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private ServiceProvider _serviceProvider;

    public ProfileViewModel(ServiceProvider serviceProvider)
    {
        UserInfo = new User();

        RefreshCommand = new Command(() =>
        {
            Task.Run(async () =>
            {
                IsRefreshing = true;
                await GetUser();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        });
        TakePhotoCommand = new Command(async () =>
        {
            TakePhoto();
        });


        _serviceProvider = serviceProvider;

    }

    private User userInfo = ListChatViewModel.USRInfo;
    private bool isRefreshing;

    async Task GetUser()
    {
        var response = await _serviceProvider.CallWebApi<int, ProfileInitializeResponse>
            ("/Profile/Initialize", HttpMethod.Post, LoginViewModel.Holder._resp);

        if (response.StatusCode == 200)
        {
            UserInfo = response.User;
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
            await GetUser();
        }).GetAwaiter().OnCompleted(() =>
        {
            IsRefreshing = false;
        });
    }

    public User UserInfo
    {
        get { return userInfo; }
        set { userInfo = value; OnPropertyChanged(); }
    }
  
    public bool IsRefreshing
    {
        get { return isRefreshing; }
        set { isRefreshing = value; OnPropertyChanged(); }
    }

    public ICommand RefreshCommand { get; set; }
    public ICommand TakePhotoCommand { get; set; }
    public async void TakePhoto()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                // save the file into local storage
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                var request = new PutPhoto {
                FromUserId = LoginViewModel.Holder._resp,
                Path = localFilePath,
                };
                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);
                await sourceStream.CopyToAsync(localFileStream);
                
                var response = await _serviceProvider.CallWebApi<PutPhoto, ProfileInitializeResponse>
                ("/Profile/PutPhoto", HttpMethod.Post, request);
            }
        }
    }
}
