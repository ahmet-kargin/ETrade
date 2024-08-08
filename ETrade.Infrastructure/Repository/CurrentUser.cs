using ETrade.Application.Interfaces;
using ETrade.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ETrade.Infrastructure.Repository;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public int Id => Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirstValue(CookieName.Id));

    public string FirstName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(CookieName.FirstName);

    public string LastName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(CookieName.LastName);

    public string Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public string Phone => _httpContextAccessor.HttpContext?.User?.FindFirstValue(CookieName.Phone);

    public string Address => _httpContextAccessor.HttpContext?.User?.FindFirstValue(CookieName.Address);


}
