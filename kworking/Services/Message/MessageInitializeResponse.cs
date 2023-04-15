namespace kworking.Services.Message;

public class MessageInitializeResponse : BaseResponse
{
    public User FriendInfo { get; set; }
    public IEnumerable<Model.Message> Messages { get; set; }
}
