using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopService.ViewModels
{
    public class StoreViewModel
    {
        public int Id { get; set; }

        public string StoreName { get; set; }

        public List<StoreDetaliViewModel> StoreDetalis { get; set; }
    }
}
