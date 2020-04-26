namespace Resources.Models
{
    public class Bus
    {
        public int? Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int? Axes { get; set; }
        public string VRN { get; set; }
        public int? ProdYear { get; set; }
        public int? PrchYear { get; set; }
        public int? PlcsAmnt { get; set; }
        public string CpctClss { get; set; }
        public string EN { get; set; }

        public bool HasIntValue(string varName)
        {
            switch (varName)
            {
                case "Id":
                    return Id.HasValue;
                case "Axes":
                    return Axes.HasValue;
                case "ProdYear":
                    return ProdYear.HasValue;
                case "PrchYear":
                    return PrchYear.HasValue;
                case "PlcsAmnt":
                    return PlcsAmnt.HasValue;
                default:
                    break;
            }
            return false;
        }
    }
}
