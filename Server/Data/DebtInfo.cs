using System.Collections.Generic;

namespace Server.Data
{
    public class DebtInfo
    {
        //Qarzdorlar ro'yxati, ma'lumotlari
        public int Id { get; set; }
        public string FIO { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public List<Debt> Debts { get; set; }
    }
}
