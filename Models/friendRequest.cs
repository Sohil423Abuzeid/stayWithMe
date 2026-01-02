namespace stayWithMeApi.Models
{
    public class friendRequest
    {
        public string Id { get; set; }
        public DateTime requestDate { get; set; } = DateTime.Now;
        public int reciverId { get; set; }
        public int senderId { get; set; }

    }
}
