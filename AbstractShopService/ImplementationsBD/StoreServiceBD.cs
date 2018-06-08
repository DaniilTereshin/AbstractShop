using AbstractShopModel;
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractShopService.ImplementationsBD
{
    public class StoreServiceBD : IStoreService
    {
        private AbstractDbContext context;

        public StoreServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<StoreViewModel> GetList()
        {
            List<StoreViewModel> result = context.Stores
                .Select(rec => new StoreViewModel
                {
                    Id = rec.Id,
                    StoreName = rec.StoreName,
                    StoreDetalis = context.StoreDetalis
                            .Where(recPC => recPC.StoreId == rec.Id)
                            .Select(recPC => new StoreDetaliViewModel
                            {
                                Id = recPC.Id,
                                StoreId = recPC.StoreId,
                                DetaliId = recPC.DetaliId,
                                DetaliName = recPC.Detali.DetaliName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public StoreViewModel GetElement(int id)
        {
            Store element = context.Stores.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new StoreViewModel
                {
                    Id = element.Id,
                    StoreName = element.StoreName,
                    StoreDetalis = context.StoreDetalis
                            .Where(recPC => recPC.StoreId == element.Id)
                            .Select(recPC => new StoreDetaliViewModel
                            {
                                Id = recPC.Id,
                                StoreId = recPC.StoreId,
                                DetaliId = recPC.DetaliId,
                                DetaliName = recPC.Detali.DetaliName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(StoreBindingModel model)
        {
            Store element = context.Stores.FirstOrDefault(rec => rec.StoreName == model.StoreName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            context.Stores.Add(new Store
            {
                StoreName = model.StoreName
            });
            context.SaveChanges();
        }

        public void UpdElement(StoreBindingModel model)
        {
            Store element = context.Stores.FirstOrDefault(rec =>
                                        rec.StoreName == model.StoreName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = context.Stores.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.StoreName = model.StoreName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Store element = context.Stores.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // при удалении удаляем все записи о компонентах на удаляемом складе
                        context.StoreDetalis.RemoveRange(
                                            context.StoreDetalis.Where(rec => rec.StoreId == id));
                        context.Stores.Remove(element);
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