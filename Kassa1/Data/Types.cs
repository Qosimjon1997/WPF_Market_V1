using System.Collections.Generic;

namespace Kassa1.Data
{
    public class Types
    {
        //Tovarning turi
        public int Id { get; set; }
        public string TypeName { get; set; }

        public List<Product> Products { get; set; }
    }
}
