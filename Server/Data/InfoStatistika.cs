namespace Server.Data
{
    internal class InfoStatistika
    {
        //DataGrid uchun
        public InfoStatistika()
        {
        }

        public string Shtrix { get; internal set; }
        public string Tovar { get; set; }
        public decimal Miqdor { get; set; }
        public decimal Foyda { get; internal set; }
        public string TypeName { get; internal set; }
        public string MassaName { get; internal set; }
    }
}