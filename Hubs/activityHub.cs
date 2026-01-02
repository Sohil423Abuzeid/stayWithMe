using Microsoft.AspNetCore.SignalR;

namespace stayWithMeApi.Hubs
{
    public class activityHub :Hub
    {
        public override async Task OnConnectedAsync()
        {

            await base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {

            await base.OnDisconnectedAsync(exception);
        }
    }
}
