using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Market_V1.Data
{
    public class Partiya
    {
        //Tovarning partiyasi
        public int Id { get; set; }
        public decimal BazaPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CountProduct { get; set; }
        public decimal CountProduct2 { get; set; }
        public DateTime TodayDate { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public List<Kassa> Kassas { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
    }
}
