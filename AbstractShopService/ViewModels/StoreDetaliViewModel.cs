using System.Runtime.Serialization;

namespace AbstractShopService.ViewModels
{
    [DataContract]
    public class StoreDetaliViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int StoreId { get; set; }
        [DataMember]
        public int DetaliId { get; set; }
        [DataMember]
        public string DetaliName { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
