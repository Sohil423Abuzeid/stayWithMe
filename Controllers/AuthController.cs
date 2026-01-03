using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using stayWithMeApi.DTOS;
using stayWithMeApi.Services;

namespace stayWithMeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService authService , UsersService usersService,OtpService otpService) : ControllerBase
    {
        [HttpGet("/email/{email}")]
        public async Task<IActionResult> checkEmail([FromRoute] string email)
        {
            try
            {
                return Ok(new { valid = !(await usersService.checkUserEmailAsync(email)) });
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("/userName/{userName}")]
        public async Task<IActionResult> checkUserName([FromRoute] string userName)
        {
            try
            {
                return Ok(new { valid = !(await usersService.checkUserNameAsync(userName)) });
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal Server Error: {ex.Message}");
            }
        }
        [HttpPost("/register")]
        public async Task<IActionResult> register([FromBody]RegisterDto registerDto)
        {
            try
            {
                await authService.registerAsync(registerDto);

                return Ok(new { message = "User created" });
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("/login")]
        public async Task<IActionResult> login(string email, string password)
        {
            try
            {
                string token = await authService.login(new LoginDto { userNameOrEmail =email,password= password});

                return Ok(new { token = token });
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal Server Error: {ex.Message}");
            }
        }
        [HttpPost("otp/{email}/register")]
        public async Task<IActionResult> registerOtp([FromRoute]string email)
        {
            try
            {
                await otpService.createOTPAsync(email, Enums.otpType.verificationEmail);
                return Ok(new { message = "check your mail" });
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal Server Error: {ex.Message}");
            }
        }
        [HttpPost("otp/{email}/changePassword")]
        public async Task<IActionResult> changePasswordOtp([FromRoute] string email)
        {
            try
            {
                await otpService.createOTPAsync(email, Enums.otpType.changePassword);
                return Ok(new { message = "check your mail" });
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("{email}/{otp}/register")]
        public async Task<IActionResult> checkRegisterOtp([FromRoute]string email, [FromBody]string otp)
        {
            try
            {
                var vaild = await otpService.checkRequestAsync(otp ,email, Enums.otpType.verificationEmail);
                return Ok(new { vaild = vaild });
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal Server Error: {ex.Message}");
            }
        }
        [HttpGet("{email}/{otp}/changePassword")]
        public async Task<IActionResult> checkChangePasswordOtp([FromRoute] string email, [FromBody] string otp)
        {
            try
            {
                var vaild=await otpService.checkRequestAsync(otp, email, Enums.otpType.changePassword);
                return Ok(new { vaild = vaild });
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal Server Error: {ex.Message}");
            }
        }
    }
}
