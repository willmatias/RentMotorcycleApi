using MongoDB.Bson;
using MongoDB.Driver;
using RentMotorcycle.Business.Interface.Context;
using RentMotorcycle.Business.Interface.Service;
using RentMotorcycle.Business.Models.Documents;
using RentMotorcycle.Business.Models.Param;
using System.Linq.Expressions;

namespace RentMotorcycle.Business.Services
{
    public class MotorsService : IMotorsService
    {
        private readonly IMongoContext _mongoClient;
        private readonly IRabbitService _rabbitService;
        public MotorsService(IMongoContext mongoClient, IRabbitService rabbitService)
        {
            _mongoClient = mongoClient;
            _rabbitService = rabbitService;
        }

        public async Task<List<Motors>> GetAllMotors(string placa)
        {
            if (!string.IsNullOrEmpty(placa))
                return await _mongoClient.GetRepository<Motors>().GetAllAsync().ConfigureAwait(false);
            else
                return await _mongoClient.GetRepository<Motors>().GetByFieldAsync("Placa", placa).ConfigureAwait(false);
        }

        public async Task<bool> IsUniquePlaca(string placa)
        {
            var placaExist = await _mongoClient.GetRepository<Motors>().GetByFieldAsync("Placa", placa).ConfigureAwait(false);
            return placaExist.Count() > 0 ?  true : false;
        }

        public async Task<Motors> InsertNewMotors(MotorcycleModel motorcycleModel)
        {
            var motors = new Motors
            { 
                Ano = motorcycleModel.Ano,
                Identificador = motorcycleModel.Identificador,
                Modelo = motorcycleModel.Modelo,
                Placa = motorcycleModel.Placa,
                DateAdd = DateTime.Now,
                DateUpd = DateTime.Now
            };            

            _rabbitService.PublishMessageToRabbitQueue(motors, "motors_save");
            return motors;
        }

        public async Task<UpdateResult> UpdateMotor(string motoId, string placa)
        {
            Expression<Func<Motors, string?>> fieldExpression = x => x.Placa;
            return await _mongoClient.GetRepository<Motors>().UpdateFieldAsync(ObjectId.Parse(motoId), fieldExpression, placa).ConfigureAwait(false);
        }

        public async Task<DeleteResult> DeleteMotor(string motoId)
        {   
           return await _mongoClient.GetRepository<Motors>().DeleteAsync(ObjectId.Parse(motoId)).ConfigureAwait(false);
        }
    }
}
