using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService.ViewModels
{
    [DataContract]
    public class CommodityViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string CommodityName { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public List<CommodityDetaliViewModel> CommodityDetalis { get; set; }
    }
}
