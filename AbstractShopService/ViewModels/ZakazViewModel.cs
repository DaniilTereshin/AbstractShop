using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService.ViewModels
{
    public class ZakazViewModel
    {
        public int Id { get; set; }

        public int ZakazchikId { get; set; }

        public string ZakazchikFIO { get; set; }

        public int CommodityId { get; set; }

        public string CommodityName { get; set; }

        public int? RabochiId { get; set; }

        public string RabochiName { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public string Status { get; set; }

        public string DateCreate { get; set; }

        public string DateImplement { get; set; }
    }
}
