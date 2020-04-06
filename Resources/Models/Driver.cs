namespace BusDepot.Models
{
    public class Driver
    {
        public long? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDay { get; set; }
        public long? PhoneNmbr { get; set; }
        public string Email { get; set; }
        public string AddrStrtName { get; set; }
        public int? AddrBldngNmbr { get; set; }
        public int? AddrFlatNmbr { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public bool HasIntValue(string varName)
        {
            switch (varName)
            {
                case "Id":
                    return Id.HasValue;
                case "PhoneNmbr":
                    return PhoneNmbr.HasValue;
                case "AddrBldngNmbr":
                    return AddrBldngNmbr.HasValue;
                case "AddrFlatNmbr":
                    return AddrFlatNmbr.HasValue;
                default:
                    break;
            }
            return false;
        }
    }
}
