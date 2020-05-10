namespace Resources.Models
{
    public class Driver
    {
        public long? id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string bday_date { get; set; }
        public long? phone { get; set; }
        public string email { get; set; }
        public string addr_strtname { get; set; }
        public int? addr_bldgnmbr { get; set; }
        public int? addr_apmtnmbr { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public bool in_service { get; set; }
    }
}
