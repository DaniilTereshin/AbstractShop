using AbstractShopModel;
using System.Collections.Generic;

namespace AbstractShopService
{
    class DataListSingleton
    {
        private static DataListSingleton instance;

        public List<Zakazchik> Zakazchiks { get; set; }

        public List<Detali> Detalis { get; set; }

        public List<Rabochi> Rabochis { get; set; }

        public List<Zakaz> Zakazs { get; set; }

        public List<Commodity> Commoditys { get; set; }

        public List<CommodityDetali> CommodityDetalis { get; set; }

        public List<Store> Stores { get; set; }

        public List<StoreDetali> StoreDetalis { get; set; }

        private DataListSingleton()
        {
            Zakazchiks = new List<Zakazchik>();
            Detalis = new List<Detali>();
            Rabochis = new List<Rabochi>();
            Zakazs = new List<Zakaz>();
            Commoditys = new List<Commodity>();
            CommodityDetalis = new List<CommodityDetali>();
            Stores = new List<Store>();
            StoreDetalis = new List<StoreDetali>();
        }

        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }

            return instance;
        }
    }
}

