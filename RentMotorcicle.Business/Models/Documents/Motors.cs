using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RentMotorcycle.Business.Utils;

namespace RentMotorcycle.Business.Models.Documents
{
    [BsonIgnoreExtraElements]
    [CollectionName("motors")]

    public class Motors
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public required int Identificador { get; set; }
        
        public required int Ano { get; set; }
        
        public required string Modelo { get; set; }
        public required string Placa { get; set; }

        public DateTime DateAdd { get; set; }
        public DateTime DateUpd { get; set; }
        public bool IsActive { get; set; }
    }
}
