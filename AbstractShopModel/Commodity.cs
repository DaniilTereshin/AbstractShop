using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractShopModel
{
    public class Commodity // товар
    {
        public int Id { get; set; }

        [Required]
        public string CommodityName { get; set; }

        [Required]
        public decimal Price { get; set; }
        [ForeignKey("CommodityId")]
        public virtual List<Zakaz> Zakazs { get; set; }

        [ForeignKey("CommodityId")]
        public virtual List<CommodityDetali> CommodityDetalis { get; set; }
    }
}