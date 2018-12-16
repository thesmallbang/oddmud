using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OddMud.Core.Plugins;
using OddMud.BasicGame.Commands;
using OddMud.Web.Game;
using OddMud.Core.Interfaces;

namespace OddMud.Web.Hubs
{
    public class GameHub : Hub
    {
        GameHubProcessor CommandProcessor;
        ITransport Transport;

        public GameHub(GameHubProcessor commandProcessor, ITransport transport)
        {
            CommandProcessor = commandProcessor;
            Transport = transport;
        }

        // Incoming command  from clients
        public Task CommandStream(CommandModel data)
        {
            
           return CommandProcessor?.ProcessAsync(new ProcessorData<CommandModel>(data, Context.ConnectionId));
        }


        public override async Task OnConnectedAsync()
        {
            await Transport.AddConnectionAsync(Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Transport.RemoveConnectionAsync(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}