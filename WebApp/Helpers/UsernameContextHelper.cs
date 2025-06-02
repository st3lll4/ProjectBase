using Base.Contracts;

namespace WebApp.Helpers;

public class UsernameContextHelper : IUsernameContextHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UsernameContextHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string CurrentUserName => _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "system";
}