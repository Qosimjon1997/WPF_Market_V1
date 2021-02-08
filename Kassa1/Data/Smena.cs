using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa1.Data
{
    public class Smena
    {
        public int Id { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public int WorkerId { get; set; }

    }
}
