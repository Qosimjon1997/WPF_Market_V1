using System;

namespace Server.Data
{
    public class Income
    {
        //Kirimlar 
        public int Id { get; set; }
        //Tovar narxi
        public decimal SaleProductPrice { get; set; }
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
