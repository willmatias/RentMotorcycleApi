using MongoDB.Bson;
using MongoDB.Driver;
using RentMotorcycle.Business.Interface.Context;
using RentMotorcycle.Business.Interface.Security;
using RentMotorcycle.Business.Interface.Service;
using RentMotorcycle.Business.Models.Documents;
using RentMotorcycle.Business.Models.Param;

namespace RentMotorcycle.Business.Services
{
    public class RentAMotorcycleService : IRentAMotorcycleService
    {
        private readonly IIdentity _identity;
        private readonly IUserService _userService;
        private readonly IMongoContext _mongoContext;
        public RentAMotorcycleService(IIdentity identity, IUserService userService, IMongoContext mongoContext)
        {
            _identity = identity;
            _userService = userService;
            _mongoContext = mongoContext;
        }
        private bool EntregadorEstaHabilitadoNaCategoriaA(TipoCNH tipoCNH)
        {
            if (tipoCNH == TipoCNH.B) return false;

            return true; 
        }

        public decimal CalcularCustoAluguel(int dias)
        {

            // Verifica qual é o plano de locação baseado no número de dias
            decimal custoDiario;
            switch (dias)
            {
                case <= 7:
                    custoDiario = 30;
                    break;
                case <= 15:
                    custoDiario = 28;
                    break;
                case <= 30:
                    custoDiario = 22;
                    break;
                case <= 45:
                    custoDiario = 20;
                    break;
                default:
                    custoDiario = 18;
                    break;
            }

            // Calcula o custo total do aluguel
            decimal custoTotal = custoDiario * dias;

            return custoTotal;
        }

        public async Task<(string mensagem, bool status)> InsertNewRentAMotorcycle(RentAMotorcycleParam rentAMotorcycle)
        {            
            var user = await _userService.GetUserById(_identity.UserId()).ConfigureAwait(false);

            if (!EntregadorEstaHabilitadoNaCategoriaA(user.TipoCnh)) return ("Usuário fora da catagoria", false);

            if (rentAMotorcycle.DataInicio == DateTime.Now) return ("Data tem que ser maior que a data atual", false);

            var calcRent = CalcularCustoAluguel(rentAMotorcycle.Dias);

            var renatAMotorcycleRepository = new RentAMotorcycleRepository
            {
                DataFim = rentAMotorcycle.DataFim,
                DataInicio = rentAMotorcycle.DataInicio,
                DataPrevisao = rentAMotorcycle.DataPrevisao,
                DateAdd = DateTime.Now,
                DateUpd = DateTime.Now,
                Dias = rentAMotorcycle.Dias,
                IsActive = true,
                UserId = user.Id,
                Valor = calcRent
            };

            await _mongoContext.GetRepository<RentAMotorcycleRepository>().InsertAsync(renatAMotorcycleRepository).ConfigureAwait(false);

            return ($"Aluguel cadastrado com sucesso, valor do alguel é {calcRent}", true);

        }

        public async Task<(string mensagem, bool status)> PostDateReturnMotorcycle(DateTime dateHoraReturn)
        {
            var motorcycleRegistration = await _mongoContext.GetRepository<RentAMotorcycleRepository>().GetByFieldAsync("UserId", ObjectId.Parse(_identity.UserId())).ConfigureAwait(false);

            if (motorcycleRegistration.Count() < 0) return ("Não foi encontrado nenhum registo de aluguel de moto para o usuário informado", false);

            var valorFinal = CalcularValorTotal(dateHoraReturn, motorcycleRegistration.FirstOrDefault().Dias, motorcycleRegistration.FirstOrDefault().DataPrevisao, motorcycleRegistration.FirstOrDefault().Valor);

            return ($"O valor final da locação é {valorFinal}", true);
        }



        private decimal CalcularValorTotal(DateTime dataDevolucao, int diasLocacao, DateTime dataPrevisaoTermino, decimal valorDiaria)
        {

            if (dataDevolucao < dataPrevisaoTermino)
            {
                // Calcula o número de dias não efetivados
                int diasNaoEfetivados = (int)(dataPrevisaoTermino - dataDevolucao).TotalDays;

                // Calcula o valor das diárias não efetivadas
                decimal valorMulta = 0;
                if (diasLocacao <= 7)
                {
                    valorMulta = valorDiaria * diasNaoEfetivados * 0.2m; // Multa de 20%
                }
                else if (diasLocacao <= 15)
                {
                    valorMulta = valorDiaria * diasNaoEfetivados * 0.4m; // Multa de 40%
                }

                return valorDiaria * diasLocacao + valorMulta;
            }
            // Verifica se a data de devolução é posterior à data prevista do término
            else if (dataDevolucao > dataPrevisaoTermino)
            {
                // Calcula o número de dias adicionais
                int diasAdicionais = (int)(dataDevolucao - dataPrevisaoTermino).TotalDays;

                // Calcula o valor adicional por diária
                decimal valorAdicionalDiaria = 50;

                return valorDiaria * diasLocacao + valorAdicionalDiaria * diasAdicionais;
            }
            else // Se a data de devolução for igual à data prevista do término
            {
                return valorDiaria * diasLocacao;
            }
        }
    }
}
