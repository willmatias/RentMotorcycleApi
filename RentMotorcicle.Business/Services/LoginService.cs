
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using RentMotorcycle.Business.Interface.Context;
using RentMotorcycle.Business.Interface.Security;
using RentMotorcycle.Business.Models.Documents;
using RentMotorcycle.Business.Models.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RentMotorcycle.Business.Services
{
    public class LoginService(IMongoContext mongoContext) : ILoginService
    {
        public readonly IMongoContext _mongoContext = mongoContext;
        public readonly Tokens _tokens;

        public async Task<(string mensagem, bool status)> Login(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password)) return ("Usuário ou senha não informado", false);           

            var user = await _mongoContext.GetRepository<User>().GetByFieldAsync("Email", username).ConfigureAwait(false);

            if(user.FirstOrDefault().Password != password) return ("Usuário ou senha incorretos", false);

            ClaimsIdentity claims = new ClaimsIdentity(new[]
                        {
                       new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                       new Claim(JwtRegisteredClaimNames.Jti, user.FirstOrDefault().Id.ToString()),
                       new Claim("username", user.FirstOrDefault().Nome),
                       new Claim("role", user.FirstOrDefault().Role),
                   });


            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokens.Key));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(_tokens.Issuer,
                    _tokens.Audience,
                    claims.Claims,
                    expires: DateTime.Now.AddMinutes(_tokens.Expires),
                    signingCredentials: creds);

            string tokenApplication = new JwtSecurityTokenHandler().WriteToken(token);

            user.FirstOrDefault().Token = tokenApplication;



            Expression<Func<User, string?>> fieldExpression = x => x.Token;
            await _mongoContext.GetRepository<User>().UpdateFieldAsync(ObjectId.Parse(user.FirstOrDefault().Id), fieldExpression ,tokenApplication).ConfigureAwait(false);

            return (tokenApplication, true);

        }
    }
}
