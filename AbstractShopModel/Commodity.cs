using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopModel
{
    public class Commodity // товар
    {
        public int Id { get; set; }

        public string CommodityName { get; set; }

        public decimal Price { get; set; }
    }
}