using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Market_V1.Data
{
    public class Kassa
    {
        //Kassa DataGrid
        public int Id { get; set; }
        public decimal CountProduct { get; set; }
        public decimal AllPrice { get; set; }

        public int WorkerID { get; set; }

        public int PartiyaId { get; set; }
        public Partiya Partiya { get; set; }
    }
}
