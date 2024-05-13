namespace RentMotorcycle.Business.Models.Param
{
    public class RentAMotorcycleParam
    {
        public int Dias { get; set; }
        public required DateTime DataInicio { get; set; }
        public required DateTime DataFim { get; set; }
        public required DateTime DataPrevisao { get; set; }
    }
}
