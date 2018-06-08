using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractShopService.ViewModels
{
    [DataContract]
    public class StoresLoadViewModel
    {
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public List<StoresDetaliLoadViewModel> Detalis { get; set; }
    }
    [DataContract]
    public class StoresDetaliLoadViewModel
    {
        [DataMember]
        public string DetaliName { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}