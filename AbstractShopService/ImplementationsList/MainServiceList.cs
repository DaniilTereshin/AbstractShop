using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
            List<ZakazViewModel> result = source.Zakazs
                .Select(rec => new ZakazViewModel
                {
                    Id = rec.Id,
                    ZakazchikId = rec.ZakazchikId,
                    CommodityId = rec.CommodityId,
                    RabochiId = rec.RabochiId,
                    DateCreate = rec.DateCreate.ToLongDateString(),
                    DateImplement = rec.DateImplement?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    ZakazchikFIO = source.Zakazchiks
                                    .FirstOrDefault(recC => recC.Id == rec.ZakazchikId)?.ZakazchikFIO,
                    CommodityName = source.Commoditys
                                    .FirstOrDefault(recP => recP.Id == rec.CommodityId)?.CommodityName,
                    RabochiName = source.Rabochis
                                    .FirstOrDefault(recI => recI.Id == rec.RabochiId)?.RabochiFIO
                })
                .ToList();
            return result;
        }

        public void CreateZakaz(ZakazBindingModel model)
        {
            int maxId = source.Zakazs.Count > 0 ? source.Zakazs.Max(rec => rec.Id) : 0;
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
            Zakaz element = source.Zakazs.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            // смотрим по количеству компонентов на складах
            var CommodityDetalis = source.CommodityDetalis.Where(rec => rec.CommodityId == element.CommodityId);
            foreach (var CommodityDetali in CommodityDetalis)
            {
                int countOnStores = source.StoreDetalis
                                            .Where(rec => rec.DetaliId == CommodityDetali.DetaliId)
                                            .Sum(rec => rec.Count);
                if (countOnStores < CommodityDetali.Count * element.Count)
                {
                    var DetaliName = source.Detalis
                                    .FirstOrDefault(rec => rec.Id == CommodityDetali.DetaliId);
                    throw new Exception("Не достаточно компонента " + DetaliName?.DetaliName +
                        " требуется " + CommodityDetali.Count + ", в наличии " + countOnStores);
                }
            }
            // списываем
            foreach (var CommodityDetali in CommodityDetalis)
            {
                int countOnStores = CommodityDetali.Count * element.Count;
                var StoreDetalis = source.StoreDetalis
                                            .Where(rec => rec.DetaliId == CommodityDetali.DetaliId);
                foreach (var StoreDetali in StoreDetalis)
                {
                    // компонентов на одном слкаде может не хватать
                    if (StoreDetali.Count >= countOnStores)
                    {
                        StoreDetali.Count -= countOnStores;
                        break;
                    }
                    else
                    {
                        countOnStores -= StoreDetali.Count;
                        StoreDetali.Count = 0;
                    }
                }
            }
            element.RabochiId = model.RabochiId;
            element.DateImplement = DateTime.Now;
            element.Status = ZakazStatus.Выполняется;
        }

        public void FinishZakaz(int id)
        {
            Zakaz element = source.Zakazs.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ZakazStatus.Готов;
        }

        public void PayZakaz(int id)
        {
            Zakaz element = source.Zakazs.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ZakazStatus.Оплачен;
        }

        public void PutDetaliOnStore(StoreDetaliBindingModel model)
        {
            StoreDetali element = source.StoreDetalis
                                                .FirstOrDefault(rec => rec.StoreId == model.StoreId &&
                                                                    rec.DetaliId == model.DetaliId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.StoreDetalis.Count > 0 ? source.StoreDetalis.Max(rec => rec.Id) : 0;
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
}