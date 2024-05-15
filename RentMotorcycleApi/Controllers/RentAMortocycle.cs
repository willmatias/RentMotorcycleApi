using Microsoft.AspNetCore.Mvc;
using RentMotorcycle.Business.Interface.Security;
using RentMotorcycle.Business.Interface.Service;
using RentMotorcycle.Business.Models.Param;

namespace RentMotorcycleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentAMortocycle : Controller
    {
        private readonly ILogger _logger;
        private readonly IRentAMotorcycleService _rentAMotorcycle;
        public RentAMortocycle(ILogger<RentAMortocycle> logger, IRentAMotorcycleService rentAMotorcycle)
        {
            _logger = logger;
            _rentAMotorcycle = rentAMotorcycle;
        }

        [HttpPost("PostRentAMotorcycle")]
        [TypeFilter(typeof(CanAccess), Arguments = new[] { "ADMIN, USER" })]
        public async Task<ActionResult> PostRentAMotorcycle([FromBody] RentAMotorcycleParam rentAMotorcycle)
        {
            try
            {
                if(!ModelState.IsValid) return BadRequest(ModelState);

                (string mensagem, bool status) = await _rentAMotorcycle.InsertNewRentAMotorcycle(rentAMotorcycle).ConfigureAwait(false);

                if(status)
                    return Ok(mensagem);
                else
                    return BadRequest(mensagem);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção ao tentar alugar uma moto, ex: {ex.Message}");
                return BadRequest($"Exceção ao tentar alugar uma moto, ex: {ex.Message}");
            }
        }

        [HttpPost("PostDateReturnMotorcycle")]
        [TypeFilter(typeof(CanAccess), Arguments = new[] { "ADMIN, USER" })]
        public async Task<ActionResult> PostDateReturnMotorcycle(DateTime dateHoraReturn)
        {
            try
            {
                (string mensagem, bool status) = await _rentAMotorcycle.PostDateReturnMotorcycle(dateHoraReturn).ConfigureAwait(false);

                if (status)
                    return Ok(mensagem);
                else
                    return BadRequest(mensagem);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção ao tentar adicionar data de devolução da moto, ex: {ex.Message}");
                return BadRequest($"Exceção ao tentar adicionar data de devolução da moto, ex: {ex.Message}");
            }
        }

    }
}
