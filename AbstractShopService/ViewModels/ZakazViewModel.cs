﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService.ViewModels
{
    [DataContract]
    public class ZakazViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ZakazchikId { get; set; }
        [DataMember]
        public string ZakazchikFIO { get; set; }
        [DataMember]
        public int CommodityId { get; set; }
        [DataMember]
        public string CommodityName { get; set; }
        [DataMember]
        public int? RabochiId { get; set; }
        [DataMember]
        public string RabochiName { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string DateCreate { get; set; }
        [DataMember]
        public string DateImplement { get; set; }
    }
}
