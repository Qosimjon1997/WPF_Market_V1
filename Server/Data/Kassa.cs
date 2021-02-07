namespace Server.Data
{
    public class Kassa
    {
        //Kassa DataGrid
        public int Id { get; set; }
        public decimal CountProduct { get; set; }
        public decimal AllPrice { get; set; }

        public int WorkerID { get; set; }

        public int PartiyaId { get; set; }
        public Partiya Partiya { get; set; }
    }
}
