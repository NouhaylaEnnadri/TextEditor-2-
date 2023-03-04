using Microsoft.AspNetCore.SignalR;

namespace CollaborationApp.Hubs
{
    public class TextEditorHub : Hub
    {
        public async Task UpdateDocument(string content)
        {
            Console.WriteLine(content);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("updateDocument", content);
        }
    }
}
