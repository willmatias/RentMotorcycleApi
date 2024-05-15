using MongoDB.Driver;
using RentMotorcycle.Business.Models.Documents;
using RentMotorcycle.Business.Models.Param;

namespace RentMotorcycle.Business.Interface.Service
{
    public interface IMotorsService
    {
        Task<List<Motors>> GetAllMotors(string placa);
        Task<bool> IsUniquePlaca(string placa);

        Task<Motors> InsertNewMotors(MotorcycleModel motorcycleModel);
        Task<UpdateResult> UpdateMotor(string motoId, string placa);
        Task<DeleteResult> DeleteMotor(string motoId);
    }
}
