using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService.BindingModels
{
    [DataContract]
    public class StoreDetaliBindingModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int StoreId { get; set; }
        [DataMember]
        public int DetaliId { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
