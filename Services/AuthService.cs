using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using stayWithMeApi.DTOS;
using stayWithMeApi.Enums;
using stayWithMeApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace stayWithMeApi.Services
{
    public class AuthService(IConfiguration _config,AppDbContext dbContext,UsersService usersService, OtpService otpService, StorageService storageService)
    {
        public async Task<bool> registerAsync(RegisterDto registerDto)
        {

            try
            {
                bool emailFound = await usersService.checkUserEmailAsync(registerDto.Email);
                if (emailFound) throw new Exception("this email used");

                bool userNameFound = await usersService.checkUserNameAsync(registerDto.userName);
                if (userNameFound) throw new Exception("this userName used");

                bool otp = await otpService.checkRequestAsync(registerDto.otp, registerDto.Email, otpType.verificationEmail);
                if (!otp) throw new Exception("otp expired");

                string imageUrl = "";//F_T fix this later

                //if (registerDto.formFile != null) 
                //imageUrl = await storageService.uploadImage(registerDto.formFile);

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                var user = new User
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    hashedPassword = hashedPassword,
                    userName = registerDto.userName,
                    imageUrl = imageUrl,
                    birthDate = registerDto.birthDate,
                };
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> login(LoginDto loginDto)
        {
            try
            {
                var user = await dbContext.Users.Where(u => u.Email == loginDto.userNameOrEmail || u.userName == loginDto.userNameOrEmail).FirstOrDefaultAsync();
                if (user == null) throw new Exception("email or user name not found or wrong password");

                if (!BCrypt.Net.BCrypt.Verify(loginDto.password, user.hashedPassword))
                    throw new Exception("email or user name not found or wrong password");

                return GenerateToken(user);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string GenerateToken(User user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? "");

            var claims = new List<Claim>
         {

             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ),
            // will continue later          

         };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(jwtSettings["TokenExpirationDays"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
