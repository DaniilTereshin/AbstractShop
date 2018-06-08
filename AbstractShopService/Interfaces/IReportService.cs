using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System.Collections.Generic;

namespace AbstractShopService.Interfaces
{
    public interface IReportService
    {
        void SaveCommodityPrice(ReportBindingModel model);

        List<StoresLoadViewModel> GetStoresLoad();

        void SaveStoresLoad(ReportBindingModel model);

        List<ZakazchikZakazsModel> GetZakazchikZakazs(ReportBindingModel model);

        void SaveZakazchikZakazs(ReportBindingModel model);
    }
}