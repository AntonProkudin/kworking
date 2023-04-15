using kworkingApi.Entities;

namespace kworkingApi.Functions.Message;

public class MessageFunction : IMessageFunction
{
    KworkingContext _kworkingContext;
    IUserFunction _userFunction;
    public MessageFunction(KworkingContext kworkingContext, IUserFunction userFunction)
    {
        _kworkingContext = kworkingContext;
        _userFunction = userFunction;
    }

    public async Task<int> AddMessage(int fromUserId, int toUserId, string message)
    {
        var entity = new TblMessage
        {
            FromUserId = fromUserId,
            ToUserId = toUserId,
            Content = message,
            SendDateTime = DateTime.Now,
            IsRead = false
        };

        _kworkingContext.TblMessages.Add(entity);
        var result = await _kworkingContext.SaveChangesAsync();

        return result;
    }

    public async Task<IEnumerable<LastestMessage>> GetLastestMessage(int userId)
    {
        var result = new List<LastestMessage>();

        var userFriends = await _kworkingContext.TblUserFriends
            .Where(x => x.UserId == userId).ToListAsync();

        foreach (var userFriend in userFriends)
        {
            var lastMessage = await _kworkingContext.TblMessages
                .Where(x => (x.FromUserId == userId && x.ToUserId == userFriend.FriendId)
                         || (x.FromUserId == userFriend.FriendId && x.ToUserId == userId))
                .OrderByDescending(x => x.SendDateTime)
                .FirstOrDefaultAsync();

            if (lastMessage != null)
            {
                result.Add(new LastestMessage
                {
                    UserId = userId,
                    Content = lastMessage.Content,
                    UserFriendInfo = _userFunction.GetUserById(userFriend.FriendId),
                    Id = lastMessage.Id,
                    IsRead = lastMessage.IsRead,
                    SendDateTime = lastMessage.SendDateTime
                });
            }
        }
        return result;
    }

    public async Task<IEnumerable<Message>> GetMessages(int fromUserId, int toUserId)
    {
        var entities = await _kworkingContext.TblMessages
            .Where(x => (x.FromUserId == fromUserId && x.ToUserId == toUserId)
                || (x.FromUserId == toUserId && x.ToUserId == fromUserId))
            .OrderBy(x => x.SendDateTime)
            .ToListAsync();

        return entities.Select(x => new Message
        {
            Id = x.Id,
            Content = x.Content,
            FromUserId = x.FromUserId,
            ToUserId = x.ToUserId,
            SendDateTime = x.SendDateTime,
            IsRead = x.IsRead,
        });
    }
}