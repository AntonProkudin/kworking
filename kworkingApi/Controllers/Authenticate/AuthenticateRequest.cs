using Microsoft.AspNetCore.Mvc;
using kworkingApi.Functions.User;

namespace kworkingApi.Controllers.Authenticate;

public class AuthenticateRequest
{
    public string LoginId { get; set; } = null!;
    public string Password { get; set; } = null!;
}