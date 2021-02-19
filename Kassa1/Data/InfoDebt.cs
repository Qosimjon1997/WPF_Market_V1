using System;

namespace Kassa1.Data
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
        public decimal Price { get; set; }
    }
}