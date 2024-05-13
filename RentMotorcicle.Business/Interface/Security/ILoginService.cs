namespace RentMotorcycle.Business.Interface.Security
{
    public interface ILoginService
    {
        Task<(string mensagem, bool status)> Login(string username, string password);
    }
}
