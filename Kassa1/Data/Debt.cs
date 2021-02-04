using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Market_V1.Data
{
    public class Debt
    {
        //Qarzdorlik puli
        public int Id { get; set; }
        public DateTime dateTimeFrom { get; set; }
        public DateTime dateTimeUntil { get; set; }
        public DateTime dateTimePay { get; set; }
        public decimal Price { get; set; }

        public int DebtInfoId { get; set; }
        public DebtInfo DebtInfo { get; set; }
    }
}
