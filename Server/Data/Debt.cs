using System;

namespace Server.Data
{
    public class Debt
    {
        //Qarzdorlik puli
        public int Id { get; set; }
        public DateTime timeNow { get; set; }
        public decimal Price { get; set; }

        public int DebtInfoId { get; set; }
        public DebtInfo DebtInfo { get; set; }
    }
}
