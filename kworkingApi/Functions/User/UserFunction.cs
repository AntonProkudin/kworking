using kworkingApi.Helpers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace kworkingApi.Functions.User;
public class UserFunction : IUserFunction
{
    private readonly KworkingContext _kworkingContext;

    public UserFunction(KworkingContext kworkingContext)
    {
        _kworkingContext = kworkingContext;
    }

    public User? Authenticate(string loginId, string password)
    {
        try
        {
            var entity = _kworkingContext.TblUsers.SingleOrDefault(x => x.LoginId == loginId);
            if (entity == null) return null;

            var isPasswordMatched = VertifyPassword(password, entity.StoredSalt, entity.Password);

            if (!isPasswordMatched) return null;

            var token = GenerateJwtToken(entity);

            return new User
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Token = token
            };

        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public User GetUserById(int id)
    {
        var entity = _kworkingContext.TblUsers
            .Where(x => x.Id == id)
            .FirstOrDefault();

        if (entity == null) return new User();

        var awayDuration = entity.IsOnline ? "" : Utilities.CalcAwayDuration(entity.LastLoginTime);
        return new User
        {
            UserName = entity.UserName,
            Id = entity.Id,
            AvatarSourceName = entity.AvatarSourceName,
            IsAway = awayDuration != "" ? true : false,
            AwayDuration = awayDuration,
            IsOnline = entity.IsOnline,
            LastLoginTime = entity.LastLoginTime
        };
    }


    private bool VertifyPassword(string enteredPassword, byte[] storedSalt, string storedPassword)
    {
        string encryptyedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: enteredPassword,
            salt: storedSalt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return encryptyedPassword.Equals(storedPassword);
    }

    private string GenerateJwtToken(TblUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("1234567890123456");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);

    }
}
