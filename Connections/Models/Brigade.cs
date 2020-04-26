using System;

namespace Connections.Models
{
    public class Brigade
    {
        public int? Line { get; set; }
        public int? Id { get; set; }
        public int? Relation { get; set; }
        public DateTime DepTime { get; set; }
        public DateTime ArrTime { get; set; }
        public char Remarks { get; set; }

        public bool HasIntValue(string varName)
        {
            switch (varName)
            {
                case "Line":
                    return Line.HasValue;
                case "Id":
                    return Id.HasValue;
                case "Relation":
                    return Relation.HasValue;
                default:
                    break;
            }
            return false;
        }
    }
}
