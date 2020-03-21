using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Entity
{
    public class EntityBase
    {
        //Add common Properties here that will be used for all your entities
        public int? CreatedByID { get; set; }
        public int? ModifiedByID { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }

        public EntityBase()
        {
            ModifiedDateTime = DateTime.Now;
        }
    }
}
