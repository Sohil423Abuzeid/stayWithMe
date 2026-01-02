using Microsoft.EntityFrameworkCore;
using stayWithMeApi.Enums;
using stayWithMeApi.Models;

namespace stayWithMeApi.Services
{
    public class OtpService(AppDbContext dbContext)
    {
        public async Task<bool> checkRequestAsync(string otp, string email, otpType otpType)
        {
            try
            {
                var request =await dbContext.otpRequests.Where(r=>r.otpType == otpType && r.Email ==email&& r.Otp==otp).FirstOrDefaultAsync();
                if (request == null) return false;

                if (request.checkedCounter > 1)
                {
                    await deleteRequestAsync(request);
                    return false;
                }
                else if (request.checkedCounter ==1)
                {
                    await deleteRequestAsync(request);
                }
                else
                {
                    request.checkedCounter++;
                    dbContext.Update(request);
                    await dbContext.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task deleteRequestAsync(otpRequest otpRequest)
        {
            try
            {
                dbContext.otpRequests.Remove(otpRequest);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
