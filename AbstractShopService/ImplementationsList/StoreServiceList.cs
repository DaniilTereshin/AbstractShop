using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractShopService.ImplementationsList
{
    public class StoreServiceList : IStoreService
    {
        private DataListSingleton source;

        public StoreServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<StoreViewModel> GetList()
        {
            List<StoreViewModel> result = new List<StoreViewModel>();
            for (int i = 0; i < source.Stores.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<StoreDetaliViewModel> StoreDetalis = new List<StoreDetaliViewModel>();
                for (int j = 0; j < source.StoreDetalis.Count; ++j)
                {
                    if (source.StoreDetalis[j].StoreId == source.Stores[i].Id)
                    {
                        string detaliDlyaDvigatelya = string.Empty;
                        for (int k = 0; k < source.Detalis.Count; ++k)
                        {
                            if (source.CommodityDetalis[j].DetaliId == source.Detalis[k].Id)
                            {
                                detaliDlyaDvigatelya = source.Detalis[k].DetaliName;
                                break;
                            }
                        }
                        StoreDetalis.Add(new StoreDetaliViewModel
                        {
                            Id = source.StoreDetalis[j].Id,
                            StoreId = source.StoreDetalis[j].StoreId,
                            DetaliId = source.StoreDetalis[j].DetaliId,
                            DetaliName = detaliDlyaDvigatelya,
                            Count = source.StoreDetalis[j].Count
                        });
                    }
                }
                result.Add(new StoreViewModel
                {
                    Id = source.Stores[i].Id,
                    StoreName = source.Stores[i].StoreName,
                    StoreDetalis = StoreDetalis
                });
            }
            return result;
        }

        public StoreViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Stores.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<StoreDetaliViewModel> StoreDetalis = new List<StoreDetaliViewModel>();
                for (int j = 0; j < source.StoreDetalis.Count; ++j)
                {
                    if (source.StoreDetalis[j].StoreId == source.Stores[i].Id)
                    {
                        string detaliDlyaDvigatelya = string.Empty;
                        for (int k = 0; k < source.Detalis.Count; ++k)
                        {
                            if (source.CommodityDetalis[j].DetaliId == source.Detalis[k].Id)
                            {
                                detaliDlyaDvigatelya = source.Detalis[k].DetaliName;
                                break;
                            }
                        }
                        StoreDetalis.Add(new StoreDetaliViewModel
                        {
                            Id = source.StoreDetalis[j].Id,
                            StoreId = source.StoreDetalis[j].StoreId,
                            DetaliId = source.StoreDetalis[j].DetaliId,
                            DetaliName = detaliDlyaDvigatelya,
                            Count = source.StoreDetalis[j].Count
                        });
                    }
                }
                if (source.Stores[i].Id == id)
                {
                    return new StoreViewModel
                    {
                        Id = source.Stores[i].Id,
                        StoreName = source.Stores[i].StoreName,
                        StoreDetalis = StoreDetalis
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(StoreBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Stores.Count; ++i)
            {
                if (source.Stores[i].Id > maxId)
                {
                    maxId = source.Stores[i].Id;
                }
                if (source.Stores[i].StoreName == model.StoreName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            source.Stores.Add(new Store
            {
                Id = maxId + 1,
                StoreName = model.StoreName
            });
        }

        public void UpdElement(StoreBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Stores.Count; ++i)
            {
                if (source.Stores[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Stores[i].StoreName == model.StoreName &&
                    source.Stores[i].Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Stores[index].StoreName = model.StoreName;
        }

        public void DelElement(int id)
        {
            // при удалении удаляем все записи о компонентах на удаляемом складе
            for (int i = 0; i < source.StoreDetalis.Count; ++i)
            {
                if (source.StoreDetalis[i].StoreId == id)
                {
                    source.StoreDetalis.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Stores.Count; ++i)
            {
                if (source.Stores[i].Id == id)
                {
                    source.Stores.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
