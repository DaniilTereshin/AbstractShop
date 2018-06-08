using AbstractShopService.Attributies;
using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы со складами")]
    public interface IStoreService
    {
        [CustomMethod("Метод получения списка складов")]
        List<StoreViewModel> GetList();
        [CustomMethod("Метод получения склада по id")]
        StoreViewModel GetElement(int id);
        [CustomMethod("Метод добавления склада")]
        void AddElement(StoreBindingModel model);
        [CustomMethod("Метод изменения данных по складу")]
        void UpdElement(StoreBindingModel model);
        [CustomMethod("Метод удаления склада")]
        void DelElement(int id);
    }
}
