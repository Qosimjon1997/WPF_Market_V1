using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Market_V1.Data
{
    public class Product
    {
        //Tovarlar
        public int Id { get; set; }
        public string Shtrix { get; set; }
        public string NameOfProduct { get; set; }

        public List<Partiya> Partiyas { get; set; }

        public int MassaId { get; set; }
        public Massa Massa { get; set; }

        public int TypesId { get; set; }
        public Types Types { get; set; }


    }
}
