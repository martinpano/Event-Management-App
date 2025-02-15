
namespace EventManager.Core.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public int SoldTicketsNumber { get; set; }
    }
}
