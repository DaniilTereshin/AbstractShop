using AbstractShopService;
using AbstractShopService.ImplementationsBD;
using AbstractShopService.ImplementationsList;
using AbstractShopService.Interfaces;
using System;
using System.Data.Entity;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace AbstractShopView
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormMain>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, AbstractDbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IZakazchikService, ZakazchikServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IDetaliService, DetaliServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IRabochiService, RabochiServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICommodityService, CommodityServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IStoreService, StoreServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReportService, ReportServiceBD>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}