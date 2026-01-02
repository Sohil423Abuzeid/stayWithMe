namespace stayWithMeApi.DTOS
{
    public class RegisterDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string userName { get; set; }
        public string Password { get; set; }

        public string otp {  get; set; }

        public DateTime birthDate { get; set; }

        public  IFormFile formFile { get; set; }
    }
}
