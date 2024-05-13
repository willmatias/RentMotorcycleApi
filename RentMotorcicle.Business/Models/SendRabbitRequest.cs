using System.Text.Json.Serialization;

namespace RentMotorcycle.Business.Models
{
    public class SendRabbitRequest
    {   
        public string Message { get; set; }
        public int Status { get; set; }        
    }
}
