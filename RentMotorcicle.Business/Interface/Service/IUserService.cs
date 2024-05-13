using RentMotorcycle.Business.Models.Param;
using Microsoft.AspNetCore.Http;
using RentMotorcycle.Business.Models.Documents;

namespace RentMotorcycle.Business.Interface.Service
{
    public interface IUserService
    {
        Task<User> GetUserById(string v);
        Task<(string mensagem, bool status)> InsertUser(UserModel userModel, IFormFile file);
        Task<(string mensagem, bool status)> UpdateImageCNh(string userId, IFormFile file);
    }
}
