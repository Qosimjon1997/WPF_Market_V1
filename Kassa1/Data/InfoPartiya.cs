using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa1.Data
{

    internal class InfoPartiya
    {
        //DataGrid uchun
        public InfoPartiya()
        {
        }
    
        public int Id { get; set; }
        public string Shtrix { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string MassaName { get; set; }
        public decimal Price { get; set; }
    }
    
}
