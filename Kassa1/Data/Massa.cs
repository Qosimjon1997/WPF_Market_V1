﻿using System.Collections.Generic;

namespace Kassa1.Data
{
    public class Massa
    {
        //Tovarni o'lchash turi
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
