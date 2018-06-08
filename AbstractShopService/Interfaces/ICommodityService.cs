using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface ICommodityService
    {
        List<CommodityViewModel> GetList();

        CommodityViewModel GetElement(int id);

        void AddElement(CommodityBindingModel model);

        void UpdElement(CommodityBindingModel model);

        void DelElement(int id);
    }
}
