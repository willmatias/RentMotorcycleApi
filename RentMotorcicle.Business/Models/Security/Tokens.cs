using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentMotorcycle.Business.Models.Security
{
    public class Tokens
    {
        public string Audience { get; set; }

        public int Expires { get; set; }

        public string Issuer { get; set; }

        public string Key { get; set; }
    }
}
