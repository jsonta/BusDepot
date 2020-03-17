namespace BusDepot.Models
{
    public class Bus
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Axes { get; set; }
        public string VRN { get; set; }
        public int VId { get; set; }
        public int ProdYear { get; set; }
        public int PrchYear { get; set; }
        public int PlcsAmnt { get; set; }
        public string CpctClss { get; set; }
        public string EN { get; set; }
    }
}
