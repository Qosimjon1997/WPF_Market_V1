using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Market_V1.Data
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
