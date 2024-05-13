using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RentMotorcycle.Business.Utils;

namespace RentMotorcycle.Business.Models.Documents
{
    [BsonIgnoreExtraElements]
    [CollectionName("rent_motorcycle")]
    public class RentAMotorcycleRepository
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int Dias { get; set; }
        public required DateTime DataInicio { get; set; }
        public required DateTime DataFim { get; set; }
        public required DateTime DataPrevisao { get; set; }

        public DateTime DateAdd { get; set; }
        public DateTime DateUpd { get; set; }
        public bool IsActive { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string  UserId { get; set; }
        public decimal Valor { get; set; }
    }
}
