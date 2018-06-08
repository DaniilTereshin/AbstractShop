using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IStoreService
    {
        List<StoreViewModel> GetList();

        StoreViewModel GetElement(int id);

        void AddElement(StoreBindingModel model);

        void UpdElement(StoreBindingModel model);

        void DelElement(int id);
    }
}
