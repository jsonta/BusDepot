namespace BusDepot.Models
{
    public class Driver
    {
        public long? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Birthday { get; set; }
        public long? Phone { get; set; }
        public string Email { get; set; }
        public string StreetName { get; set; }
        public int? BuildingNumber { get; set; }
        public int? ApartmentNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public bool HasIntValue(string varName)
        {
            switch (varName)
            {
                case "Id":
                    return Id.HasValue;
                case "Phone":
                    return Phone.HasValue;
                case "BuildingNumber":
                    return BuildingNumber.HasValue;
                case "ApartmentNumber":
                    return ApartmentNumber.HasValue;
                default:
                    break;
            }
            return false;
        }
    }
}
