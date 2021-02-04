using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Market_V1.Data
{
    public class Provider
    {
        //Tovarni yetkazib beruvchi
        public int Id { get; set; }
        public string ProviderName { get; set; }

        public List<Partiya> Partiyas { get; set; }

    }
}
