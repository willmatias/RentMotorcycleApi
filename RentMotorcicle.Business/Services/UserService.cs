using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using RentMotorcycle.Business.Interface.Context;
using RentMotorcycle.Business.Interface.Service;
using RentMotorcycle.Business.Models.Documents;
using RentMotorcycle.Business.Models.Param;
using System.Linq.Expressions;

namespace RentMotorcycle.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoContext _mongoClient;
        public UserService(IMongoContext mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public Task<User> GetUserById(string userId)
        {
            return _mongoClient.GetRepository<User>().GetByIdAsync(ObjectId.Parse(userId));
        }

        public async Task<(string mensagem, bool status)> InsertUser(UserModel userModel, IFormFile file)
        {
            string mensagem = "Usuário cadastrado com sucesso";
            if (!string.IsNullOrEmpty(userModel.Cnpj))
            {
                var cnpj = await _mongoClient.GetRepository<User>().GetByFieldAsync("Cnpj", userModel.Cnpj).ConfigureAwait(false);

                if (cnpj.Count > 0) return (mensagem = "Cnpj já cadastrado", false);
            }
            else if (!string.IsNullOrEmpty(userModel.Cnh))
            {
                var cnh = await _mongoClient.GetRepository<User>().GetByFieldAsync("Cnh", userModel.Cnh).ConfigureAwait(false);
                if (cnh.Count > 0) return (mensagem = "CNH já cadastrada", false);
            }

            if (file != null)
            {
                string extensao = Path.GetExtension(file.FileName);
                if (extensao != ".png" && extensao != ".bmp")
                {
                    return (mensagem = "Formato de imagem inválido. Apenas arquivos .png ou .bmp são permitidos.", false);
                }

                string diretorioDownloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                diretorioDownloads = Path.Combine(diretorioDownloads, "Downloads");

                userModel.ImagemCnh = Path.Combine(diretorioDownloads, file.ContentType.Split('/')[1]); // Nome do arquivo baseado no nome do motorista

                using FileStream stream = new FileStream(userModel.ImagemCnh, FileMode.Create);
                file.CopyTo(stream);
            }

            var user = new User 
            { 
                Cnh = userModel.Cnh,
                Cnpj = userModel.Cnpj,
                DataNascimento = userModel.DataNascimento,
                ImagemCnh = userModel.ImagemCnh,
                IsActive = true,
                Nome = userModel.Nome,
                TipoCnh = userModel.TipoCnh,
                DateAdd = DateTime.Now,
                DateUpd = DateTime.Now,
            };

            await _mongoClient.GetRepository<User>().InsertAsync(user);

            return (mensagem, true);
        }

        public async Task<(string mensagem, bool status)> UpdateImageCNh(string userId, IFormFile file)
        {

            string mensagem = "Fotod do usuário editada com sucesso";

            try
            {

                string extensao = Path.GetExtension(file.FileName);
                if (extensao != ".png" && extensao != ".bmp")
                {
                    return (mensagem = "Formato de imagem inválido. Apenas arquivos .png ou .bmp são permitidos.", false);
                }

                string diretorioDownloads = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                diretorioDownloads = Path.Combine(diretorioDownloads, "Downloads");

                string caminhoImagem = Path.Combine(diretorioDownloads, file.ContentType.Split('/')[1]); // Nome do arquivo baseado no nome do motorista

                using FileStream stream = new FileStream(caminhoImagem, FileMode.Create);
                file.CopyTo(stream);

                Expression<Func<User, string?>> fieldExpression = x => x.ImagemCnh;
                await _mongoClient.GetRepository<User>().UpdateFieldAsync(ObjectId.Parse(userId), fieldExpression, caminhoImagem);

                return (mensagem, true);
            }
            catch (Exception ex)
            {
                return (mensagem = $"Erro ao processar imagem, ex: {ex.Message}", false);
            }


        }
    }
}
