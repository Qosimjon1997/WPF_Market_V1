using System;

namespace Server.Data
{
    internal class InfoDebt
    {
        //Datagrid uchun
        public InfoDebt()
        {
        }

        public int Id { get; set; }
        public string FIO { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime dateTimeFrom { get; set; }
        public DateTime dateTimeUntil { get; set; }
        public DateTime dateTimeLastPay { get; set; }
        public decimal Price { get; set; }
    }
}