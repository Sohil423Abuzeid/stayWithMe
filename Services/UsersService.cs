using Microsoft.EntityFrameworkCore;
using stayWithMeApi.Models;
namespace stayWithMeApi.Services
{
    public class UsersService(AppDbContext dbContext,StorageService storageService,OtpService otpService)
    {
        public async Task<bool> checkUserNameAsync(string userName)
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
        public async Task<bool> checkUserEmailAsync(string userEmail)
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
        public async Task<List<int>> friendsIDsAsync(int userID)
        {
            try
            {
                var user = await dbContext.Users.Include(u => u.Friends).Where(u=> u.Id== userID).FirstOrDefaultAsync(); 
                if(user == null) return(new List<int>());

                List<int> ids = user.Friends.Select(user => user.Id).ToList();
            
                return ids ;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<string>> strFriendsIDsAsync(int userID)
        {
            try
            {
                var user = await dbContext.Users.Include(u => u.Friends).Where(u => u.Id == userID).FirstOrDefaultAsync();
                if (user == null) return (new List<string>());

                List<string> ids = user.Friends.Select(user => user.Id.ToString()).ToList();

                return ids;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task changeUserStateAsync(int userID,bool onlineState)
        {
            try
            {
                var user = await dbContext.Users.Include(u => u.Friends).Where(u => u.Id == userID).FirstOrDefaultAsync();

                user.Online = onlineState;

                dbContext.Users.Update(user);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
