namespace Connections.Models
{
    public class Line
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public bool HasIntValue(string varName)
        {
            switch (varName)
            {
                case "Id":
                    return Id.HasValue;
                default:
                    break;
            }
            return false;
        }
    }
}
