namespace stayWithMeApi.Models
{
    public class Effect
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public double price { get; set; } = 0;

        public string storageLink { get; set; }
    }
}
