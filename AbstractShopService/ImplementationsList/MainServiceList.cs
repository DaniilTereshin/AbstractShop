using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractShopService.ImplementationsList
{
    public class MainServiceList : IMainService
    {
        private DataListSingleton source;

        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<ZakazViewModel> GetList()
        {
            List<ZakazViewModel> result = new List<ZakazViewModel>();
            for (int i = 0; i < source.Zakazs.Count; ++i)
            {
                string zakazchikFIO = string.Empty;
                for (int j = 0; j < source.Zakazchiks.Count; ++j)
                {
                    if (source.Zakazchiks[j].Id == source.Zakazs[i].ZakazchikId)
                    {
                        zakazchikFIO = source.Zakazchiks[j].ZakazchikFIO;
                        break;
                    }
                }
                string CommodityTip = string.Empty;
                for (int j = 0; j < source.Commoditys.Count; ++j)
                {
                    if (source.Commoditys[j].Id == source.Zakazs[i].CommodityId)
                    {
                        CommodityTip = source.Commoditys[j].CommodityName;
                        break;
                    }
                }
                string rabochiFIO = string.Empty;
                if (source.Zakazs[i].RabochiId.HasValue)
                {
                    for (int j = 0; j < source.Rabochis.Count; ++j)
                    {
                        if (source.Rabochis[j].Id == source.Zakazs[i].RabochiId.Value)
                        {
                            rabochiFIO = source.Rabochis[j].RabochiFIO;
                            break;
                        }
                    }
                }
                result.Add(new ZakazViewModel
                {
                    Id = source.Zakazs[i].Id,
                    ZakazchikId = source.Zakazs[i].ZakazchikId,
                    ZakazchikFIO = zakazchikFIO,
                    CommodityId = source.Zakazs[i].CommodityId,
                    CommodityName = CommodityTip,
                    RabochiId = source.Zakazs[i].RabochiId,
                    RabochiName = rabochiFIO,
                    Count = source.Zakazs[i].Count,
                    Sum = source.Zakazs[i].Sum,
                    DateCreate = source.Zakazs[i].DateCreate.ToLongDateString(),
                    DateImplement = source.Zakazs[i].DateImplement?.ToLongDateString(),
                    Status = source.Zakazs[i].Status.ToString()
                });
            }
            return result;
        }

        public void CreateZakaz(ZakazBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Zakazs.Count; ++i)
            {
                if (source.Zakazs[i].Id > maxId)
                {
                    maxId = source.Zakazchiks[i].Id;
                }
            }
            source.Zakazs.Add(new Zakaz
            {
                Id = maxId + 1,
                ZakazchikId = model.ZakazchikId,
                CommodityId = model.CommodityId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = ZakazStatus.Принят
            });
        }

        public void TakeZakazInWork(ZakazBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Zakazs.Count; ++i)
            {
                if (source.Zakazs[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            for (int i = 0; i < source.CommodityDetalis.Count; ++i)
            {
                if (source.CommodityDetalis[i].CommodityId == source.Zakazs[index].CommodityId)
                {
                    int countOnStores = 0;
                    for (int j = 0; j < source.StoreDetalis.Count; ++j)
                    {
                        if (source.StoreDetalis[j].DetaliId == source.CommodityDetalis[i].DetaliId)
                        {
                            countOnStores += source.StoreDetalis[j].Count;
                        }
                    }
                    if (countOnStores < source.CommodityDetalis[i].Count * source.Zakazs[index].Count)
                    {
                        for (int j = 0; j < source.Detalis.Count; ++j)
                        {
                            if (source.Detalis[j].Id == source.CommodityDetalis[i].DetaliId)
                            {
                                throw new Exception("Не достаточно компонента " + source.Detalis[j].DetaliName +
                                    " требуется " + source.CommodityDetalis[i].Count + ", в наличии " + countOnStores);
                            }
                        }
                    }
                }
            }
            // списываем
            for (int i = 0; i < source.CommodityDetalis.Count; ++i)
            {
                if (source.CommodityDetalis[i].CommodityId == source.Zakazs[index].CommodityId)
                {
                    int countOnStores = source.CommodityDetalis[i].Count * source.Zakazs[index].Count;
                    for (int j = 0; j < source.StoreDetalis.Count; ++j)
                    {
                        if (source.StoreDetalis[j].DetaliId == source.CommodityDetalis[i].DetaliId)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (source.StoreDetalis[j].Count >= countOnStores)
                            {
                                source.StoreDetalis[j].Count -= countOnStores;
                                break;
                            }
                            else
                            {
                                countOnStores -= source.StoreDetalis[j].Count;
                                source.StoreDetalis[j].Count = 0;
                            }
                        }
                    }
                }
            }
            source.Zakazs[index].RabochiId = model.RabochiId;
            source.Zakazs[index].DateImplement = DateTime.Now;
            source.Zakazs[index].Status = ZakazStatus.Выполняется;
        }

        public void FinishZakaz(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Zakazs.Count; ++i)
            {
                if (source.Zakazchiks[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Zakazs[index].Status = ZakazStatus.Готов;
        }

        public void PayZakaz(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Zakazs.Count; ++i)
            {
                if (source.Zakazchiks[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Zakazs[index].Status = ZakazStatus.Оплачен;
        }

        public void PutDetaliOnStore(StoreDetaliBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.StoreDetalis.Count; ++i)
            {
                if (source.StoreDetalis[i].StoreId == model.StoreId &&
                    source.StoreDetalis[i].DetaliId == model.DetaliId)
                {
                    source.StoreDetalis[i].Count += model.Count;
                    return;
                }
                if (source.StoreDetalis[i].Id > maxId)
                {
                    maxId = source.StoreDetalis[i].Id;
                }
            }
            source.StoreDetalis.Add(new StoreDetali
            {
                Id = ++maxId,
                StoreId = model.StoreId,
                DetaliId = model.DetaliId,
                Count = model.Count
            });
        }
    }
}
