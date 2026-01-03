using Hangfire;
using Microsoft.EntityFrameworkCore;
using stayWithMeApi.Enums;
using stayWithMeApi.Models;
using System.Diagnostics.Contracts;
using static System.Net.WebRequestMethods;

namespace stayWithMeApi.Services
{
    public class OtpService(AppDbContext dbContext, EmailService emailService)
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
                    await createOTPAsync(email, otpType);
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
        public async Task createOTPAsync(string email , otpType otpType)
        {
            try
            {
                var request = await dbContext.otpRequests.Where(r => r.otpType == otpType && r.Email == email ).FirstOrDefaultAsync();
                if (request != null) await deleteRequestAsync(request);

                request = new otpRequest
                {
                    Email = email,
                    otpType = otpType,
                    Otp =  new Random().Next(100001, 999999).ToString() // might chage it later 
                };

                emailService.SendOtpMail(request.Otp, request.Email);

                dbContext.otpRequests.Add(request);
                await dbContext.SaveChangesAsync();

                BackgroundJob.Schedule(() => deleteRequestAsync(request.Id), TimeSpan.FromSeconds(330));
            }
            catch (Exception ex) {
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
        public async Task deleteRequestAsync(int otpRequestId)
        {
            try
            {
                var otpRequest = await dbContext.otpRequests.FindAsync(otpRequestId);

                if (otpRequest == null) return;

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
