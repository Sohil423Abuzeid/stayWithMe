using Microsoft.AspNetCore.SignalR;
using stayWithMeApi.Services;

namespace stayWithMeApi.Hubs
{
    public class activityHub(UsersService usersService) :Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = int.Parse(Context.UserIdentifier);//F_T i think try parse is better while iam sure about it will work #imporatant 

            //change my state 
            await usersService.changeUserStateAsync(userId, true);


            //change state for my friends 
            var friendsIds = await usersService.strFriendsIDsAsync(userId);
            await Clients.Users(friendsIds).SendAsync("updateUserState", userId, true);



            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = int.Parse(Context.UserIdentifier);//F_T i think try parse is better while iam sure about it will work #imporatant 

            //change my state 
            await usersService.changeUserStateAsync(userId, false);


            //change state for my friends 
            var friendsIds = await usersService.strFriendsIDsAsync(userId);
            await Clients.Users(friendsIds).SendAsync("updateUserState", userId, false);




            await base.OnDisconnectedAsync(exception);
        }
    }
}
