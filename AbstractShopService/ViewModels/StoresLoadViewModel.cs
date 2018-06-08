using System;
using System.Collections.Generic;

namespace AbstractShopService.ViewModels
{
    public class StoresLoadViewModel
    {
        public string StoreName { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<Tuple<string, int>> Detalis { get; set; }
    }
}