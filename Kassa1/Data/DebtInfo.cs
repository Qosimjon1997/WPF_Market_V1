using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Market_V1.Data
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
