namespace Connections.Models
{
    public class Timetable
    {
        public int id { get; set; }
        public string brigade { get; set; }
        public string relation { get; set; }
        public char remarks { get; set; }
        public string dep_time { get; set; }
        public string arr_time { get; set; }
    }
}
