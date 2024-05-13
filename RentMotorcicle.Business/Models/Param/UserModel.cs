using System.ComponentModel.DataAnnotations;

namespace RentMotorcycle.Business.Models.Param
{

    public enum TipoCNH
    {
        A,
        B,
        AB

    }
    public class UserModel
    {        
        public string Nome { get; set; }

        
        public required string Cnpj { get; set; }
        
        public required string Cnh { get; set; }
        public DateTime DataNascimento { get; set; }

        private TipoCNH _cnh;
        public TipoCNH TipoCnh
        {
            get { return _cnh; }
            set
            {
                if (value == TipoCNH.A || value == TipoCNH.B || value == TipoCNH.AB)
                {
                    _cnh = value;
                }
                else
                {
                    throw new ArgumentException("Tipo de CNH inválido. Apenas A, B ou AB são permitidos.");
                }
            }
        }

        public string? ImagemCnh { get; internal set; }
    }
}
