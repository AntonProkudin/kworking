namespace kworking.Model;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string AvatarSourceName { get; set; } = null!;
    public bool IsOnline { get; set; }
    public DateTime LastLoginTime { get; set; }
    public bool IsAway { get; set; }
    public string AwayDuration { get; set; } = null!;
}
