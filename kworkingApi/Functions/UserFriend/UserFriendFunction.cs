namespace kworkingApi.Functions.UserFriend;

public class UserFriendFunction : IUserFriendFunction
{
    KworkingContext _kworkingContext;
    IUserFunction _userFunction;
    public UserFriendFunction(KworkingContext kworkingContext, IUserFunction userFunction)
    {
        _kworkingContext = kworkingContext;
        _userFunction = userFunction;
    }
    public async Task<IEnumerable<User.User>> GetListUserFriend(int userId)
    {
        var entities = await _kworkingContext.TblUserFriends
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var result = entities.Select(x => _userFunction.GetUserById(x.FriendId));

        if (result == null) result = new List<User.User>();

        return result;
    }
}