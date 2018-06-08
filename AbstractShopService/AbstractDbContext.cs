using AbstractShopModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService
{
    //[Table("AbstractDatabase3")]
    public class AbstractDbContext : DbContext
    {
        public AbstractDbContext(): base ("AbstractDatabase3777")
        {
            //настройки конфигурации для entity
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<Zakazchik> Zakazchiks { get; set; }

        public virtual DbSet<Detali> Detalis { get; set; }

        public virtual DbSet<Rabochi> Rabochis { get; set; }

        public virtual DbSet<Zakaz> Zakazs { get; set; }

        public virtual DbSet<Commodity> Commoditys { get; set; }

        public virtual DbSet<CommodityDetali> CommodityDetalis { get; set; }

        public virtual DbSet<Store> Stores { get; set; }

        public virtual DbSet<StoreDetali> StoreDetalis { get; set; }
        /// <summary>
        /// Перегружаем метод созранения изменений. Если возникла ошибка - очищаем все изменения
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (Exception)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Deleted:
                            entry.Reload();
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                    }
                }
                throw;
            }
        }
    }
}