using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService.ViewModels
{
    public class CommodityViewModel
    {
        public int Id { get; set; }

        public string CommodityName { get; set; }

        public decimal Price { get; set; }

        public List<CommodityDetaliViewModel> CommodityDetalis { get; set; }
    }
}
