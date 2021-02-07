using System.Collections.Generic;

namespace Server.Data
{
    public class Worker
    {
        //Ishchining ma'lumotlari
        public int Id { get; set; }
        public string Login { get; set; }
        public string Passw { get; set; }
        public string FIO { get; set; }
        public bool Active { get; set; }

        public List<Income> Incomes { get; set; }
        public List<Sold> Solds { get; set; }
    }
}
