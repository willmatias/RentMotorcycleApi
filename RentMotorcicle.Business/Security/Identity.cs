using Microsoft.AspNetCore.Http;
using RentMotorcycle.Business.Interface.Security;

namespace RentMotorcycle.Business.Security
{
    public class Identity : IIdentity
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Identity(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId()
        {
            return _httpContextAccessor?.HttpContext?.User?.FindFirst("id")?.Value;
        }
    }
}
