namespace kworkingApi.Functions.User;
public interface IUserFunction
{
    User? Authenticate(string loginId, string password);
    User GetUserById(int id);
    void PutPhotoById(int id, string path);
}
