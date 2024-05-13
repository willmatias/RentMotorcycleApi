using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentMotorcycle.Business.Interface.Security;
using RentMotorcycle.Business.Interface.Service;
using RentMotorcycle.Business.Models.Param;

namespace RentMotorcycleApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MotorcycleRegistration : ControllerBase
    {
        private readonly IMotorsService _motorsService;
        private readonly ILogger _logger;

        public MotorcycleRegistration(IRabbitService rabbitService, IMotorsService motorsService, ILogger<MotorcycleRegistration> logger)
        {
            _motorsService = motorsService;
            _logger = logger;
        }

        [HttpGet]
        [TypeFilter(typeof(CanAccess), Arguments = new[] { "ADMIN, USER" })]
        public async Task<ActionResult> Get(string placa)
        {
            try
            {
                return Ok(await _motorsService.GetAllMotors(placa).ConfigureAwait(false));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar motos cadastradas, error: {ex.Message}");
                return BadRequest($"Erro ao buscar motos cadastradas, error: {ex.Message}");
            }

        }
        
        [HttpPost]        
        [TypeFilter(typeof(CanAccess), Arguments = new[] { "ADMIN" })]
        public async Task<ActionResult> PostRecordMotorcycle([FromBody] MotorcycleModel motorcycleModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                if (await _motorsService.IsUniquePlaca(motorcycleModel.Placa).ConfigureAwait(false))
                    return BadRequest(new { Mensagem = "Já existe uma moto com a placa informada" });

                var motors = await _motorsService.InsertNewMotors(motorcycleModel).ConfigureAwait(false);

                return Ok(motors);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao tentar salvar uma nova moto, ex: {ex.Message}");
                return BadRequest($"Erro ao tentar salvar uma nova moto, ex: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("motoId/{motoId}/placa/{placa}")]        
        [TypeFilter(typeof(CanAccess), Arguments = new[] { "ADMIN" })]
        public async Task<ActionResult> PutMotorcycle([FromRoute] string motoId, string placa)
        {
            try
            {
                await _motorsService.UpdateMotor(motoId, placa).ConfigureAwait(false);
                return Ok("Moto atualizada com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao editar a moto, ex: {ex.Message}");
                return BadRequest($"Erro ao editar a moto, ex: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("motoId/{motoId}")]
        [TypeFilter(typeof(CanAccess), Arguments = new[] { "ADMIN" })]
        public async Task<ActionResult> DeleteMotors([FromRoute] string motoId)
        {
            try
            {
                await _motorsService.DeleteMotor(motoId).ConfigureAwait(false);
                return Ok("Moto Deletada com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao excluir a moto, ex: {ex.Message}");
                return BadRequest($"Erro ao excluira moto, ex: {ex.Message}");
            }
        }
    }
}
