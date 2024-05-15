using System.ComponentModel.DataAnnotations;

namespace RentMotorcycle.Business.Models.Param
{
    public class MotorcycleModel
    {        
        public required int Identificador { get; set; }
        
        public required int Ano { get; set; }
        
        public required string Modelo { get; set; }
        
        public required string Placa { get; set; }
    }
}
