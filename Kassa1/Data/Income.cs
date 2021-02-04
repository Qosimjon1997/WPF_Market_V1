using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Market_V1.Data
{
    public class Income
    {
        //Kirimlar 
        public int Id { get; set; }
        //Tovar narxi
        public decimal SaleProductPrice { get; set; }
        //Tovar narxi skidka bilan
        public decimal SaleProductWithDiscountPrice { get; set; }
        //naxt
        public decimal CashIncome { get; set; }
        //plastik
        public decimal PlasticIncome { get; set; }
        //qarzning qaytishi
        public decimal DebtIncome { get; set; }
        //qarzga berish
        public decimal DebtOut { get; set; }
        //tovar vozvrati
        public decimal Vozvrat { get; set; }
        //vaqt
        public DateTime DateTimeNow { get; set; }
        
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

    }
}
