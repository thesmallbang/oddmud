using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Transport.SignalR
{
    public class SignalRHubTransport<THub> : ITransport
        where THub : Hub
    {
        private readonly ILogger<SignalRHubTransport<THub>> _logger;
        private readonly IHubContext<THub> _hub;

        public SignalRHubTransport(ILogger<SignalRHubTransport<THub>> logger, IHubContext<THub> hub)
        {
            _logger = logger;
            _hub = hub;
            _logger.LogDebug($"ICommunication Injection");
        }


        public async Task AddPlayerToMapGroupAsync(IPlayer player, IMap map)
        {
            await _hub.Groups.AddToGroupAsync(player.TransportId, $"map_{map.Id}");
        }

        public async Task RemovePlayerFromMapGroupAsync(IPlayer player, IMap map)
        {
            await _hub.Groups.RemoveFromGroupAsync(player.TransportId, $"map_{map.Id}");
        }

        public void SendMessageToMap(IMap map, string message)
        {
            SendMessageToMap(map.Id, message);
        }


        public void SendMessageToMap(string mapId, string message)
        {
            _hub.Clients.Group($"map_{mapId}").SendAsync("ChatStream", message);
        }

        public void SendMessageToMapExcept(IMap map, IEnumerable<IPlayer> players, string message)
        {
            _hub.Clients.GroupExcept($"map_{map.Id}", players.Select(p => p.TransportId).ToList()).SendAsync("ChatStream", message);
        }

        public void SendMessageToMapExcept(IMap map, IPlayer player, string message)
        {
            SendMessageToMapExcept(map, new List<IPlayer>() { player }, message);
        }

        public void SendMessageToPlayer(IPlayer player, string message)
        {
            SendMessageToPlayer(player.TransportId, message);
        }

        public void SendMessageToPlayer(string networkId, string message)
        {
            _hub.Clients.Client(networkId).SendAsync("ChatStream", message);
        }

        public void SendMessageToPlayers(IEnumerable<IPlayer> players, string message)
        {
            _hub.Clients.Clients(players.Select(p => p.TransportId).ToList()).SendAsync("ChatStream", message);
        }

        public void SendViewCommandsToMap(IMap map, IEnumerable<IViewCommand> commands)
        {
            _hub.Clients.Group($"map_{map.Id}").SendAsync("WorldStream", commands);
        }

        public void SendViewCommandsToMapExcept(IMap map, IEnumerable<IPlayer> players, IEnumerable<IViewCommand> commands)
        {
            _hub.Clients.GroupExcept($"map_{map.Id}", players.Select(p=>p.TransportId).ToList()).SendAsync("WorldStream", commands);
        }

        public void SendViewCommandsToMapExcept(IMap map, IPlayer player, IEnumerable<IViewCommand> commands)
        {
            SendViewCommandsToMapExcept(map, new List<IPlayer>() { player }, commands);
        }

        public void SendViewCommandsToPlayer(IPlayer player, IEnumerable<IViewCommand> commands)
        {
            _hub.Clients.Client(player.TransportId).SendAsync("WorldStream", commands);
        }
    }

}
