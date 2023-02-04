using System.Security.Claims;

namespace NetKubernetes.Token;

public class UserSession : IUserSession
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public UserSession(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public string GetUserSession()
  {
    var userName = _httpContextAccessor.HttpContext!.User?.Claims?
      .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

    return userName ?? string.Empty;
  }
}