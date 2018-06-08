using AbstractShopService.Attributies;
using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с изделиями")]
    public interface ICommodityService
    {
        [CustomMethod("Метод получения списка изделий")]
        List<CommodityViewModel> GetList();
        [CustomMethod("Метод получения изделия по id")]
        CommodityViewModel GetElement(int id);
        [CustomMethod("Метод добавления изделия")]
        void AddElement(CommodityBindingModel model);
        [CustomMethod("Метод изменения данных по изделию")]
        void UpdElement(CommodityBindingModel model);
        [CustomMethod("Метод удаления изделия")]
        void DelElement(int id);
    }
}
