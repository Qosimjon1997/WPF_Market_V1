﻿using System.Collections.Generic;

namespace Kassa1.Data
{
    public class Provider
    {
        //Tovarni yetkazib beruvchi
        public int Id { get; set; }
        public string ProviderName { get; set; }

        public List<Partiya> Partiyas { get; set; }

    }
}
