﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopModel
{
    public class Zakaz
    {
        public int Id { get; set; }

        public int ZakazchikId { get; set; }

        public int CommodityId { get; set; }

        public int? RabochiId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public ZakazStatus Status { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateImplement { get; set; }
    }
}
