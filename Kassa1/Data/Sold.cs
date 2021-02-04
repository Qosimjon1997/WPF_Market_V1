using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Market_V1.Data
{
    public class Sold
    {
        //Sotildi
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string Shtrix { get; set; }
        public string NameOfProduct { get; set; }

        public int PartiyaId { get; set; }
        public decimal BazaPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CountProduct { get; set; }

        public int ProviderId { get; set; }
        public string ProviderName { get; set; }

        public int MassaId { get; set; }
        public string MassaName { get; set; }

        public int TypeId { get; set; }
        public string TypeName { get; set; }

        public decimal AllSumma { get; set; }
        public DateTime dateTimeNow { get; set; }

        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

    }
}
