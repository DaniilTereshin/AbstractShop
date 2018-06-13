using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService.ViewModels
{
    public class CommodityDetaliViewModel
    {
        public int Id { get; set; }

        public int CommodityId { get; set; }

        public int DetaliId { get; set; }

        public string DetaliName { get; set; }

        public int Count { get; set; }
    }
}
