using Microsoft.AspNetCore.SignalR;
namespace EpitTrack.Services
{
    public class ProgressHub : Hub
    {
        public async Task SendProgress(string message)
        {
            await Clients.All.SendAsync("ReceiveProgress", message);
        }
    }
}
