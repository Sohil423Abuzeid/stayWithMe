using Microsoft.EntityFrameworkCore;
using stayWithMeApi.DTOS;
using stayWithMeApi.Models;
using stayWithMeApi.Enums;
using Org.BouncyCastle.Crypto.Generators;
namespace stayWithMeApi.Services
{
    public class UsersService(AppDbContext dbContext,AuthService authService,StorageService storageService,OtpService otpService)
    {
        public async Task<bool> checkUserName(string userName)
        {
            try
            {
                var user = await dbContext.Users.Where(u=>u.userName== userName).FirstOrDefaultAsync();
                if (user == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> checkUserEmail(string userEmail)
        {
            try
            {
                var user = await dbContext.Users.Where(u => u.Email == userEmail).FirstOrDefaultAsync();
                if (user == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> register(RegisterDto registerDto )
        {

            try
            {
                bool emailFound  = await checkUserEmail(registerDto.Email);
                if (emailFound) throw new Exception("this email used");

                bool otp = await otpService.checkRequestAsync(registerDto.otp, registerDto.Email, otpType.verificationEmail);
                if (!otp) throw new Exception("otp expired");

                string imageUrl = await storageService.uploadImage(registerDto.formFile);

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                var user = new User
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    hashedPassword = hashedPassword,
                    userName =registerDto.userName,
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
    }
}
