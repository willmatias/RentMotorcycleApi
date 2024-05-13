using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RentMotorcycle.Business.Interface.Context;
using RentMotorcycle.Business.Models.Documents;

namespace RentMotorcycle.Business.Interface.Security
{
    public class CanAccess : IAsyncActionFilter
    {
        private readonly IIdentity _identity;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMongoContext _mongoContext;
        public CanAccess(IIdentity identity, IHttpContextAccessor httpContextAccessor, IMongoContext mongoContext)
        {
            _identity = identity;
            _httpContextAccessor = httpContextAccessor;
            _mongoContext = mongoContext;
             
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_identity.UserId() == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string currentToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token").ConfigureAwait(false);

            if (currentToken == null) { context.Result = new UnauthorizedResult(); return; }

            var tokenUser = await _mongoContext.GetRepository<User>().GetByFieldAsync("_id", _identity.UserId()).ConfigureAwait(false);

            if (currentToken != tokenUser.FirstOrDefault().Token) { context.Result = new UnauthorizedResult(); return; }

            await next().ConfigureAwait(false);
        }
    }
}
