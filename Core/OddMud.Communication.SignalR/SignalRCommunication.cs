﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Communication.SignalR
{
    public class SignalRCommunication : ICommunication
    {
        private readonly ILogger<SignalRCommunication> _logger;
        private readonly IHubContext<GameHub> _hub;

        public SignalRCommunication(ILogger<SignalRCommunication> logger, IHubContext<GameHub> hub)
        {
            _logger = logger;
            _hub = hub;
            _logger.LogDebug($"ICommunication Injection: {nameof(SignalRCommunication)}");
        }

        public async Task AddPlayerToMapGroupAsync(IPlayer player, IMap map)
        {
            await _hub.Groups.AddToGroupAsync(player.NetworkId, $"map_{map.Id}");
        }

        public async Task RemovePlayerFromMapGroupAsync(IPlayer player, IMap map)
        {
            await _hub.Groups.RemoveFromGroupAsync(player.NetworkId, $"map_{map.Id}");
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
            _hub.Clients.GroupExcept($"map_{map.Id}", players.Select(p => p.NetworkId).ToList()).SendAsync("ChatStream", message);
        }

        public void SendMessageToMapExcept(IMap map, IPlayer player, string message)
        {
            SendMessageToMapExcept(map, new List<IPlayer>() { player }, message);
        }

        public void SendMessageToPlayer(IPlayer player, string message)
        {
            SendMessageToPlayer(player.NetworkId, message);
        }

        public void SendMessageToPlayer(string networkId, string message)
        {
            _hub.Clients.Client(networkId).SendAsync("ChatStream", message);
        }




    }

}
