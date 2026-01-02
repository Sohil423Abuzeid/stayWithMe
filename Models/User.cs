using System.Data.SqlTypes;

namespace stayWithMeApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string hashedPassword { get; set; }

        public string userName { get; set; }

        public DateTime birthDate { get; set; }

        public string? imageUrl { get; set; } //nullable for develop only will change this later 

        public bool Online { get; set; } = false;

        public int? brainTreeId { get; set; } // i will drain your money bro hehe

        public List<User> Friends { get; set; } = new();

    }
}
