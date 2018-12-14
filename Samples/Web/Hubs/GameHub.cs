using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OddMud.Core.Plugins;
using OddMud.SampleGame.Commands;
using OddMud.Web.Game;

namespace OddMud.Web.Hubs
{
    public class GameHub : Hub
    {
        GameHubProcessor CommandProcessor;

        public GameHub(GameHubProcessor commandProcessor)
        {
            CommandProcessor = commandProcessor;
            
        }

        // Incoming command  from clients
        public Task CommandStream(CommandModel data)
        {
            
           return CommandProcessor?.ProcessAsync(new ProcessorData<CommandModel>(data, Context.ConnectionId));
        }
    }
}