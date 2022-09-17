using Microsoft.AspNetCore.SignalR;

namespace ITWitor.Services.Chat
{
    public class ChatHub : Hub
    {
        public async Task MessageFromClient(string message)
        {
            await this.Clients.All.SendAsync("MessageFromClient", message);
        }
        public async Task MessageFromServer(string message, string userName)
        {
            await Clients.All.SendAsync("MessageFromServer", message, userName);
        }
        public override async Task OnConnectedAsync()
        {
            var context = this.Context.GetHttpContext();
            if (context != null && context.Request.Cookies.ContainsKey("name"))
            {
                string? userName;
                if (context.Request.Cookies.TryGetValue("name", out userName))
                {
                    Console.WriteLine($"name = {userName}\tip = {context.Connection.RemoteIpAddress}");
                }
            }
            //Console.WriteLine($"UserAgent = {context.Request.Headers["User-Agent"]}");
            //Console.WriteLine($"RemoteIpAddress = {context.Connection.RemoteIpAddress.ToString()}");
            //await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} подключен к WS");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} отключен от WS");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
