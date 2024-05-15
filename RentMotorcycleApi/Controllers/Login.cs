using Microsoft.AspNetCore.Mvc;
using RentMotorcycle.Business.Interface.Security;

namespace RentMotorcycleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Login : Controller
    {
        private readonly ILogger<Login> _logger;
        private readonly ILoginService _loginService;
        public Login(ILogger<Login> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<ActionResult> LoginUser(RentMotorcycle.Business.Models.Documents.User user)
        {
            try
            {
                if(!ModelState.IsValid) return BadRequest(ModelState);

                (string mensagem, bool status) = await _loginService.Login(user?.Email, user?.Password).ConfigureAwait(false);

                if (status)
                    return Ok(mensagem);
                else
                    return BadRequest(mensagem);
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Excessão ao tentar logar na plataforma, ex: {ex.Message}");
                return BadRequest($"Excessão ao tentar logar na plataforma, ex: {ex.Message}");
            }
        }
    }
}
