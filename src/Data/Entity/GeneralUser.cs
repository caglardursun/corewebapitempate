using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Entity
{
    public class GeneralUser : EntityBase
    {
        public int ID { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public string EMail { get; set; }

        public string Token { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsManager { get; set; }

        public DateTime ExpiredDate { get; set; }

    }
}
