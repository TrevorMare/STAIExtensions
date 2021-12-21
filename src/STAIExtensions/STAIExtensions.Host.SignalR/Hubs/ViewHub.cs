using Microsoft.AspNetCore.SignalR;

namespace STAIExtensions.Host.SignalR.Hubs;

public class ViewHub : Hub
{
    
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    
    
}