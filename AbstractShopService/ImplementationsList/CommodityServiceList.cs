using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<CommodityViewModel> result = new List<CommodityViewModel>();
            for (int i = 0; i < source.Commoditys.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<CommodityDetaliViewModel> commodityDetalis = new List<CommodityDetaliViewModel>();
                for (int j = 0; j < source.CommodityDetalis.Count; ++j)
                {
                    if (source.CommodityDetalis[j].CommodityId == source.Commoditys[i].Id)
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
                        commodityDetalis.Add(new CommodityDetaliViewModel
                        {
                            Id = source.CommodityDetalis[j].Id,
                            CommodityId = source.CommodityDetalis[j].CommodityId,
                            DetaliId = source.CommodityDetalis[j].DetaliId,
                            DetaliName = detaliDlyaDvigatelya,
                            Count = source.CommodityDetalis[j].Count
                        });
                    }
                }
                result.Add(new CommodityViewModel
                {
                    Id = source.Commoditys[i].Id,
                    CommodityName = source.Commoditys[i].CommodityName,
                    Price = source.Commoditys[i].Price,
                    CommodityDetalis = commodityDetalis
                });
            }
            return result;
        }

        public CommodityViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Commoditys.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<CommodityDetaliViewModel> commodityDetalis = new List<CommodityDetaliViewModel>();
                for (int j = 0; j < source.CommodityDetalis.Count; ++j)
                {
                    if (source.CommodityDetalis[j].CommodityId == source.Commoditys[i].Id)
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
                        commodityDetalis.Add(new CommodityDetaliViewModel
                        {
                            Id = source.CommodityDetalis[j].Id,
                            CommodityId = source.CommodityDetalis[j].CommodityId,
                            DetaliId = source.CommodityDetalis[j].DetaliId,
                            DetaliName = detaliDlyaDvigatelya,
                            Count = source.CommodityDetalis[j].Count
                        });
                    }
                }
                if (source.Commoditys[i].Id == id)
                {
                    return new CommodityViewModel
                    {
                        Id = source.Commoditys[i].Id,
                        CommodityName = source.Commoditys[i].CommodityName,
                        Price = source.Commoditys[i].Price,
                        CommodityDetalis = commodityDetalis
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddElement(CommodityBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Commoditys.Count; ++i)
            {
                if (source.Commoditys[i].Id > maxId)
                {
                    maxId = source.Commoditys[i].Id;
                }
                if (source.Commoditys[i].CommodityName == model.CommodityName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.Commoditys.Add(new Commodity
            {
                Id = maxId + 1,
                CommodityName = model.CommodityName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = 0;
            for (int i = 0; i < source.CommodityDetalis.Count; ++i)
            {
                if (source.Commoditys[i].Id > maxPCId)
                {
                    maxPCId = source.CommodityDetalis[i].Id;
                }
            }
            // убираем дубли по компонентам
            for (int i = 0; i < model.CommodityDetalis.Count; ++i)
            {
                for (int j = 1; j < model.CommodityDetalis.Count; ++j)
                {
                    if (model.CommodityDetalis[i].DetaliId ==
                        model.CommodityDetalis[j].DetaliId)
                    {
                        model.CommodityDetalis[i].Count +=
                            model.CommodityDetalis[j].Count;
                        model.CommodityDetalis.RemoveAt(j--);
                    }
                }
            }
            // добавляем компоненты
            for (int i = 0; i < model.CommodityDetalis.Count; ++i)
            {
                source.CommodityDetalis.Add(new CommodityDetali
                {
                    Id = ++maxPCId,
                    CommodityId = maxId + 1,
                    DetaliId = model.CommodityDetalis[i].DetaliId,
                    Count = model.CommodityDetalis[i].Count
                });
            }
        }

        public void UpdElement(CommodityBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Commoditys.Count; ++i)
            {
                if (source.Commoditys[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Commoditys[i].CommodityName == model.CommodityName &&
                    source.Commoditys[i].Id != model.Id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Commoditys[index].CommodityName = model.CommodityName;
            source.Commoditys[index].Price = model.Price;
            int maxPCId = 0;
            for (int i = 0; i < source.CommodityDetalis.Count; ++i)
            {
                if (source.CommodityDetalis[i].Id > maxPCId)
                {
                    maxPCId = source.CommodityDetalis[i].Id;
                }
            }
            // обновляем существуюущие компоненты
            for (int i = 0; i < source.CommodityDetalis.Count; ++i)
            {
                if (source.CommodityDetalis[i].CommodityId == model.Id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.CommodityDetalis.Count; ++j)
                    {
                        // если встретили, то изменяем количество
                        if (source.CommodityDetalis[i].Id == model.CommodityDetalis[j].Id)
                        {
                            source.CommodityDetalis[i].Count = model.CommodityDetalis[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    // если не встретили, то удаляем
                    if (flag)
                    {
                        source.CommodityDetalis.RemoveAt(i--);
                    }
                }
            }
            // новые записи
            for (int i = 0; i < model.CommodityDetalis.Count; ++i)
            {
                if (model.CommodityDetalis[i].Id == 0)
                {
                    // ищем дубли
                    for (int j = 0; j < source.CommodityDetalis.Count; ++j)
                    {
                        if (source.CommodityDetalis[j].CommodityId == model.Id &&
                            source.CommodityDetalis[j].DetaliId == model.CommodityDetalis[i].DetaliId)
                        {
                            source.CommodityDetalis[j].Count += model.CommodityDetalis[i].Count;
                            model.CommodityDetalis[i].Id = source.CommodityDetalis[j].Id;
                            break;
                        }
                    }
                    // если не нашли дубли, то новая запись
                    if (model.CommodityDetalis[i].Id == 0)
                    {
                        source.CommodityDetalis.Add(new CommodityDetali
                        {
                            Id = ++maxPCId,
                            CommodityId = model.Id,
                            DetaliId = model.CommodityDetalis[i].DetaliId,
                            Count = model.CommodityDetalis[i].Count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            // удаяем записи по компонентам при удалении изделия
            for (int i = 0; i < source.CommodityDetalis.Count; ++i)
            {
                if (source.CommodityDetalis[i].CommodityId == id)
                {
                    source.CommodityDetalis.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Commoditys.Count; ++i)
            {
                if (source.Commoditys[i].Id == id)
                {
                    source.Commoditys.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
