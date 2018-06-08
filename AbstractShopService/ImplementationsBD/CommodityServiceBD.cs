using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AbstractShopService.ImplementationsBD
{
    public class CommodityServiceBD : ICommodityService
    {
        private AbstractDbContext context;

        public CommodityServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<CommodityViewModel> GetList()
        {
            List<CommodityViewModel> result = context.Commoditys
                .Select(rec => new CommodityViewModel
                {
                    Id = rec.Id,
                    CommodityName = rec.CommodityName,
                    Price = rec.Price,
                    CommodityDetalis = context.CommodityDetalis
                            .Where(recPC => recPC.CommodityId == rec.Id)
                            .Select(recPC => new CommodityDetaliViewModel
                            {
                                Id = recPC.Id,
                                CommodityId = recPC.CommodityId,
                                DetaliId = recPC.DetaliId,
                                DetaliName = recPC.Detali.DetaliName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public CommodityViewModel GetElement(int id)
        {
            Commodity element = context.Commoditys.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new CommodityViewModel
                {
                    Id = element.Id,
                    CommodityName = element.CommodityName,
                    Price = element.Price,
                    CommodityDetalis = context.CommodityDetalis
                            .Where(recPC => recPC.CommodityId == element.Id)
                            .Select(recPC => new CommodityDetaliViewModel
                            {
                                Id = recPC.Id,
                                CommodityId = recPC.CommodityId,
                                DetaliId = recPC.DetaliId,
                                DetaliName = recPC.Detali.DetaliName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(CommodityBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Commodity element = context.Commoditys.FirstOrDefault(rec => rec.CommodityName == model.CommodityName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new Commodity
                    {
                        CommodityName = model.CommodityName,
                        Price = model.Price
                    };
                    context.Commoditys.Add(element);
                    context.SaveChanges();
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
                        context.CommodityDetalis.Add(new CommodityDetali
                        {
                            CommodityId = element.Id,
                            DetaliId = groupDetali.DetaliId,
                            Count = groupDetali.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdElement(CommodityBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Commodity element = context.Commoditys.FirstOrDefault(rec =>
                                        rec.CommodityName == model.CommodityName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.Commoditys.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.CommodityName = model.CommodityName;
                    element.Price = model.Price;
                    context.SaveChanges();

                    // обновляем существуюущие компоненты
                    var compIds = model.CommodityDetalis.Select(rec => rec.DetaliId).Distinct();
                    var updateDetalis = context.CommodityDetalis
                                                    .Where(rec => rec.CommodityId == model.Id &&
                                                        compIds.Contains(rec.DetaliId));
                    foreach (var updateDetali in updateDetalis)
                    {
                        updateDetali.Count = model.CommodityDetalis
                                                        .FirstOrDefault(rec => rec.Id == updateDetali.Id).Count;
                    }
                    context.SaveChanges();
                    context.CommodityDetalis.RemoveRange(
                                        context.CommodityDetalis.Where(rec => rec.CommodityId == model.Id &&
                                                                            !compIds.Contains(rec.DetaliId)));
                    context.SaveChanges();
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
                        CommodityDetali elementPC = context.CommodityDetalis
                                                .FirstOrDefault(rec => rec.CommodityId == model.Id &&
                                                                rec.DetaliId == groupDetali.DetaliId);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupDetali.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.CommodityDetalis.Add(new CommodityDetali
                            {
                                CommodityId = model.Id,
                                DetaliId = groupDetali.DetaliId,
                                Count = groupDetali.Count
                            });
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Commodity element = context.Commoditys.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // удаяем записи по компонентам при удалении изделия
                        context.CommodityDetalis.RemoveRange(
                                            context.CommodityDetalis.Where(rec => rec.CommodityId == id));
                        context.Commoditys.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}