using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Data.Entity;

namespace AbstractShopService.ImplementationsBD
{
    public class MainServiceBD : IMainService
    {
        private AbstractDbContext context;

        public MainServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ZakazViewModel> GetList()
        {
            List<ZakazViewModel> result = context.Zakazs
                .Select(rec => new ZakazViewModel
                {
                    Id = rec.Id,
                    ZakazchikId = rec.ZakazchikId,
                    CommodityId = rec.CommodityId,
                    RabochiId = rec.RabochiId,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                    DateImplement = rec.DateImplement == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateImplement.Value),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    ZakazchikFIO = rec.Zakazchik.ZakazchikFIO,
                    CommodityName = rec.Commodity.CommodityName,
                    RabochiName = rec.Rabochi.RabochiFIO
                })
                .ToList();
            return result;
        }

        public void CreateZakaz(ZakazBindingModel model)
        {
            context.Zakazs.Add(new Zakaz
            {
                ZakazchikId = model.ZakazchikId,
                CommodityId = model.CommodityId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = ZakazStatus.Принят
            });
            context.SaveChanges();
        }

        public void TakeZakazInWork(ZakazBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    Zakaz element = context.Zakazs.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var CommodityDetalis = context.CommodityDetalis
                                                .Include(rec => rec.Detali)
                                                .Where(rec => rec.CommodityId == element.CommodityId);
                    // списываем
                    foreach (var CommodityDetali in CommodityDetalis)
                    {
                        int countOnStores = CommodityDetali.Count * element.Count;
                        var StoreDetalis = context.StoreDetalis
                                                    .Where(rec => rec.DetaliId == CommodityDetali.DetaliId);
                        foreach (var StoreDetali in StoreDetalis)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (StoreDetali.Count >= countOnStores)
                            {
                                StoreDetali.Count -= countOnStores;
                                countOnStores = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnStores -= StoreDetali.Count;
                                StoreDetali.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnStores > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                                CommodityDetali.Detali.DetaliName + " требуется " +
                                CommodityDetali.Count + ", не хватает " + countOnStores);
                        }
                    }
                    element.RabochiId = model.RabochiId;
                    element.DateImplement = DateTime.Now;
                    element.Status = ZakazStatus.Выполняется;
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void FinishZakaz(int id)
        {
            Zakaz element = context.Zakazs.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ZakazStatus.Готов;
            context.SaveChanges();
        }

        public void PayZakaz(int id)
        {
            Zakaz element = context.Zakazs.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = ZakazStatus.Оплачен;
            context.SaveChanges();
        }

        public void PutDetaliOnStore(StoreDetaliBindingModel model)
        {
            StoreDetali element = context.StoreDetalis
                                                .FirstOrDefault(rec => rec.StoreId == model.StoreId &&
                                                                    rec.DetaliId == model.DetaliId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.StoreDetalis.Add(new StoreDetali
                {
                    StoreId = model.StoreId,
                    DetaliId = model.DetaliId,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }
    }
}
