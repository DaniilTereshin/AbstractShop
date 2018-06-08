using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService.ViewModels
{
    [DataContract]
    public class CommodityDetaliViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CommodityId { get; set; }
        [DataMember]
        public int DetaliId { get; set; }
        [DataMember]
        public string DetaliName { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
