using RentMotorcycle.Business.Models.Param;

namespace RentMotorcycle.Business.Interface.Service
{
    public interface IRentAMotorcycleService
    {
        Task<(string mensagem, bool status)> InsertNewRentAMotorcycle(RentAMotorcycleParam rentAMotorcycle);
        Task<(string mensagem, bool status)> PostDateReturnMotorcycle(DateTime dateHoraReturn);
    }
}
