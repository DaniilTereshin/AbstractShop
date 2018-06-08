using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopModel
{
    public class StoreDetali
    {
        public int Id { get; set; }

        public int StoreId { get; set; }

        public int DetaliId { get; set; }

        public int Count { get; set; }
        public virtual Store Store { get; set; }

        public virtual Detali Detali { get; set; }
    }
}
