using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RentMotorcycle.Business.Utils;
using System.ComponentModel.DataAnnotations;

namespace RentMotorcycle.Business.Models.Documents
{
    [BsonIgnoreExtraElements]
    [CollectionName("motors")]

    public class Motors
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public int Identificador { get; set; }
        [Required]
        public int Ano { get; set; }
        [Required]
        public string Modelo { get; set; }

        [Required]
        public string Placa { get; set; }

        public DateTime DateAdd { get; set; }
        public DateTime DateUpd { get; set; }
        public bool IsActive { get; set; }
    }
}
