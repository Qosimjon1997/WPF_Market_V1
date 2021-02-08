using System;

namespace Server.Data
{
    internal class InfoPartiyaKirim
    {
        //DataGrid uchun
        public InfoPartiyaKirim()
        {
        }

        public int Id { get; set; }
        public string TovarNomi { get; set; }
        public string TovarTuri { get; set; }
        public string TovarMassaTuri { get; set; }
        public decimal BazaviyNarxi { get; set; }
        public decimal SotishNarxi { get; set; }
        public decimal OlibKelingan { get; set; }
        public decimal Qolgan { get; set; }
        public string YetkazibBeruvchi { get; set; }
        public DateTime Chislo { get; set; }
        public string Shtrix { get; internal set; }
    }
}