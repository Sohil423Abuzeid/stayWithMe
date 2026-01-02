using stayWithMeApi.Enums;

namespace stayWithMeApi.Models
{
    public class otpRequest
    {
        public int Id { get; set; }
        
        public string Email { get; set; }
        public string Otp {  get; set; }

        public otpType otpType { get; set; }

        public int checkedCounter { get; set; } = 0;

    }
}
