using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.Response
{
    public class ApiResponse 
    {
        public string Token { get; set; }

        public DateTime ExpiredDate { get; set; }
        public string Claims { get; set; }
    }
}
