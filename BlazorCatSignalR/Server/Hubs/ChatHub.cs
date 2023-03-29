using Microsoft.AspNetCore.SignalR;

namespace BlazorCatSignalR.Server.Hubs;

public class ChatHub : Hub
{
	private static Dictionary<string, string> _users = new();
	
	public override async Task OnConnectedAsync()
	{
		var username = Context.GetHttpContext().Request.Query["username"];
		_users.Add(Context.ConnectionId, username);
		await SendMessage(string.Empty, $"{username} Connected!");
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		var username = _users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
		await SendMessage(string.Empty, $"{username} Disconnected!");
		await base.OnDisconnectedAsync(exception);
	}
	
	public async Task SendMessage(string user, string message)
	{
		await Clients.All.SendAsync("ReceiveMessage", user, message);
	}
}
