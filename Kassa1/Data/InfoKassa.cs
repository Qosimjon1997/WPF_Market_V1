namespace Kassa1.Data
{
    internal class InfoKassa
    {
        //Datagrid uchun 
        public InfoKassa()
        {
        }

        public int Id { get; set; }
        public string Shtrix { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string MassaName { get; set; }
        public decimal Price { get; set; }
        public decimal CountProduct { get; set; }
        public decimal AllPrice { get; set; }
    }
}