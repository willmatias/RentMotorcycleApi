using System.ComponentModel.DataAnnotations;

namespace RentMotorcycle.Business.Models.Param
{
    public class MotorcycleModel
    {
        [Required]
        public int Identificador { get; set; }
        [Required]
        public int Ano { get; set; }
        [Required]
        public string Modelo { get; set; }

        [Required]
        public string Placa { get; set; }
    }
}
