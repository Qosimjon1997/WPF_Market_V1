using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Market_V1.Data
{
    public class Types
    {
        //Tovarning turi
        public int Id { get; set; }
        public string TypeName { get; set; }

        public List<Product> Products { get; set; }
    }
}
