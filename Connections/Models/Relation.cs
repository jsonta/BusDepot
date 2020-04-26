namespace Connections.Models
{
    public class Relation
    {
        public int? Line { get; set; }
        public int? Id { get; set; }
        public int? Start { get; set; }
        public int? End { get; set; }

        public bool HasIntValue(string varName)
        {
            switch (varName)
            {
                case "Line":
                    return Line.HasValue;
                case "Id":
                    return Id.HasValue;
                case "Start":
                    return Start.HasValue;
                case "End":
                    return End.HasValue;
                default:
                    break;
            }
            return false;
        }
    }
}
