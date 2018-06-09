using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsList
{
    public class CommodityServiceList : ICommodityService
    {
        private DataListSingleton source;

        public CommodityServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<CommodityViewModel> GetList()
        {
            List<CommodityViewModel> result = source.Commoditys
                .Select(rec => new CommodityViewModel
                {
                    Id = rec.Id,
                    CommodityName = rec.CommodityName,
                    Price = rec.Price,
                    CommodityDetalis = source.CommodityDetalis
                            .Where(recPC => recPC.CommodityId == rec.Id)
                            .Select(recPC => new CommodityDetaliViewModel
                            {
                                Id = recPC.Id,
                                CommodityId = recPC.CommodityId,
                                DetaliId = recPC.DetaliId,
                                DetaliName = source.Detalis
                                    .FirstOrDefault(recC => recC.Id == recPC.DetaliId)?.DetaliName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public CommodityViewModel GetElement(int id)
        {
            Commodity element = source.Commoditys.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new CommodityViewModel
                {
                    Id = element.Id,
                    CommodityName = element.CommodityName,
                    Price = element.Price,
                    CommodityDetalis = source.CommodityDetalis
                            .Where(recPC => recPC.CommodityId == element.Id)
                            .Select(recPC => new CommodityDetaliViewModel
                            {
                                Id = recPC.Id,
                                CommodityId = recPC.CommodityId,
                                DetaliId = recPC.DetaliId,
                                DetaliName = source.Detalis
                                        .FirstOrDefault(recC => recC.Id == recPC.DetaliId)?.DetaliName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(CommodityBindingModel model)
        {
            Commodity element = source.Commoditys.FirstOrDefault(rec => rec.CommodityName == model.CommodityName);
            if (element != null)
            {
                throw new Exception("Уже есть самолет с таким названием");
            }
            int maxId = source.Commoditys.Count > 0 ? source.Commoditys.Max(rec => rec.Id) : 0;
            source.Commoditys.Add(new Commodity
            {
                Id = maxId + 1,
                CommodityName = model.CommodityName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = source.CommodityDetalis.Count > 0 ?
                                    source.CommodityDetalis.Max(rec => rec.Id) : 0;
            // убираем дубли по компонентам
            var groupDetalis = model.CommodityDetalis
                                        .GroupBy(rec => rec.DetaliId)
                                        .Select(rec => new
                                        {
                                            DetaliId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            // добавляем компоненты
            foreach (var groupDetali in groupDetalis)
            {
                source.CommodityDetalis.Add(new CommodityDetali
                {
                    Id = ++maxPCId,
                    CommodityId = maxId + 1,
                    DetaliId = groupDetali.DetaliId,
                    Count = groupDetali.Count
                });
            }
        }

        public void UpdElement(CommodityBindingModel model)
        {
            Commodity element = source.Commoditys.FirstOrDefault(rec =>
                                        rec.CommodityName == model.CommodityName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть самолет с таким названием");
            }
            element = source.Commoditys.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.CommodityName = model.CommodityName;
            element.Price = model.Price;

            int maxPCId = source.CommodityDetalis.Count > 0 ? source.CommodityDetalis.Max(rec => rec.Id) : 0;
            // обновляем существуюущие компоненты
            var compIds = model.CommodityDetalis.Select(rec => rec.DetaliId).Distinct();
            var updateDetalis = source.CommodityDetalis
                                            .Where(rec => rec.CommodityId == model.Id &&
                                           compIds.Contains(rec.DetaliId));
            foreach (var updateDetali in updateDetalis)
            {
                updateDetali.Count = model.CommodityDetalis
                                                .FirstOrDefault(rec => rec.Id == updateDetali.Id).Count;
            }
            source.CommodityDetalis.RemoveAll(rec => rec.CommodityId == model.Id &&
                                       !compIds.Contains(rec.DetaliId));
            // новые записи
            var groupDetalis = model.CommodityDetalis
                                        .Where(rec => rec.Id == 0)
                                        .GroupBy(rec => rec.DetaliId)
                                        .Select(rec => new
                                        {
                                            DetaliId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupDetali in groupDetalis)
            {
                CommodityDetali elementPC = source.CommodityDetalis
                                        .FirstOrDefault(rec => rec.CommodityId == model.Id &&
                                                        rec.DetaliId == groupDetali.DetaliId);
                if (elementPC != null)
                {
                    elementPC.Count += groupDetali.Count;
                }
                else
                {
                    source.CommodityDetalis.Add(new CommodityDetali
                    {
                        Id = ++maxPCId,
                        CommodityId = model.Id,
                        DetaliId = groupDetali.DetaliId,
                        Count = groupDetali.Count
                    });
                }
            }
        }

        public void DelElement(int id)
        {
            Commodity element = source.Commoditys.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // удаяем записи по компонентам при удалении изделия
                source.CommodityDetalis.RemoveAll(rec => rec.CommodityId == id);
                source.Commoditys.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}