using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Market_V1.Data
{
    public class InvalidGoods
    {
        public int Id { get; set; }

        public string Shtrix { get; set; }
        public string NameOfProduct { get; set; }

        public string MassaName { get; set; }
        public string TypeName { get; set; }

        public decimal CountProduct { get; set; }

        public decimal BuyPrice { get; set; }

        public DateTime Sana { get; set; }
    }
}
