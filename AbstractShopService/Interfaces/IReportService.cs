using AbstractShopService.Attributies;
using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    [CustomInterface("Интерфейс для работы с отчетами")]
    public interface IReportService
    {
        [CustomMethod("Метод сохранения списка изделий в doc-файл")]
        void SaveCommodityPrice(ReportBindingModel model);
        [CustomMethod("Метод получения списка складов с количество компонент на них")]
        List<StoresLoadViewModel> GetStoresLoad();
        [CustomMethod("Метод сохранения списка списка складов с количество компонент на них в xls-файл")]
        void SaveStoresLoad(ReportBindingModel model);
        [CustomMethod("Метод получения списка заказов клиентов")]
        List<ZakazchikZakazsModel> GetZakazchikZakazs(ReportBindingModel model);
        [CustomMethod("Метод сохранения списка заказов клиентов в pdf-файл")]
        void SaveZakazchikZakazs(ReportBindingModel model);
    }
}
