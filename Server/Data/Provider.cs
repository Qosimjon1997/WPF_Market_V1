using System.Collections.Generic;

namespace Server.Data
{
    public class Provider
    {
        //Tovarni yetkazib beruvchi
        public int Id { get; set; }
        public string ProviderName { get; set; }

        public List<Partiya> Partiyas { get; set; }

    }
}
