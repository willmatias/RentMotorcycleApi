using Microsoft.AspNetCore.Mvc;
using RentMotorcycle.Business.Interface.Security;
using RentMotorcycle.Business.Interface.Service;
using RentMotorcycle.Business.Models.Param;

namespace RentMotorcycleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly IIdentity _identity;
        public User(IUserService userService, ILogger<User> logger, IIdentity identity)
        {
            _userService = userService;
            _logger = logger;
            _identity = identity;
        }


        [HttpPost]        
        [TypeFilter(typeof(CanAccess), Arguments = new[] { "ADMIN, USER" })]
        public async Task<ActionResult> PostUser(UserModel user, IFormFile file)
        {
            try
            {
                (string mensagem, bool status) = await _userService.InsertUser(user, file).ConfigureAwait(false);
                
                if (status)
                    return Ok(mensagem);
                else
                    return BadRequest(mensagem);
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex.Message);
                return BadRequest($"Erro ao inserir o usuário, ex: {ex.Message}");
            }
        }

        [HttpPut]
        [TypeFilter(typeof(CanAccess), Arguments = new[] { "ADMIN, USER" })]
        public async Task<ActionResult> PutImageCnhUser(IFormFile file)
        {
            try
            {
                if (file == null) return BadRequest("Arquivo de imagem não informado");

                string userId = _identity.UserId();
                (string mensagem, bool status) = await _userService.UpdateImageCNh(userId, file).ConfigureAwait(false);

                if (status)
                    return Ok(mensagem);
                else
                    return BadRequest(mensagem);
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex.Message);
                return BadRequest($"Erro ao editar foto do usuários, ex: {ex.Message}");
            }
        }

    }
}
