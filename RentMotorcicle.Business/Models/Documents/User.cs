using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using RentMotorcycle.Business.Models.Param;
using RentMotorcycle.Business.Utils;

namespace RentMotorcycle.Business.Models.Documents
{
    [BsonIgnoreExtraElements]
    [CollectionName("user")]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? Nome { get; set; }

        public required string Cnpj { get; set; }
        
        public required string Cnh { get; set; }
        public DateTime DataNascimento { get; set; }
        
        public TipoCNH TipoCnh { get; set; }
        public string? ImagemCnh { get; internal set; }

        public DateTime DateAdd { get; set; }
        public DateTime DateUpd { get; set; }
        public bool IsActive { get; set; }

        public string? Token { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }

    }
}
