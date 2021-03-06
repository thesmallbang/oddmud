﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Transport.SignalR
{
    public class SignalRHubTransport<THub> : ITransport
        where THub : Hub
    {
        private readonly ILogger<SignalRHubTransport<THub>> _logger;
        private readonly IHubContext<THub> _hub;
        private readonly IViewConverter<string> _viewBuilder;

        public IReadOnlyList<string> Connections => _connections;
        private List<string> _connections = new List<string>();

        public event Func<object, string, Task> Disconnected;
        public event Func<object, string, Task> Connected;

        public SignalRHubTransport(
            ILogger<SignalRHubTransport<THub>> logger,
            IHubContext<THub> hub,
            IViewConverter<string> viewBuilder)
        {
            _logger = logger;
            _hub = hub;
            _logger.LogDebug($"Injection : ICommunication");
            _viewBuilder = viewBuilder;


        }



        public Task AddPlayerToMapGroupAsync(IPlayer player, IMap map)
        {
            return _hub.Groups.AddToGroupAsync(player.TransportId, $"map_{map.Id}");
        }

        public Task RemovePlayerFromMapGroupAsync(IPlayer player, IMap map)
        {
            return _hub.Groups.RemoveFromGroupAsync(player.TransportId, $"map_{map.Id}");
        }

        public Task SendMessageToMapAsync(IMap map, string message)
        {
            return SendMessageToMapAsync(map.Id.ToString(), message);
        }

        public Task SendMessageToAllAsync(string message)
        {
            return _hub.Clients.All.SendAsync(message);
        }

        public Task SendMessageToMapAsync(string mapId, string message)
        {
            return _hub.Clients.Group($"map_{mapId}").SendAsync("ChatStream", message);
        }

        public Task SendMessageToMapExceptAsync(IMap map, IEnumerable<IPlayer> players, string message)
        {
            return _hub.Clients.GroupExcept($"map_{map.Id}", players.Select(p => p.TransportId).ToList()).SendAsync("ChatStream", message);
        }

        public Task SendMessageToMapExceptAsync(IMap map, IPlayer player, string message)
        {
            return SendMessageToMapExceptAsync(map, new List<IPlayer>() { player }, message);
        }

        public Task SendMessageToPlayerAsync(IPlayer player, string message)
        {
            return SendMessageToPlayerAsync(player.TransportId, message);
        }

        public Task SendMessageToPlayerAsync(string transportId, string message)
        {
            return _hub.Clients.Client(transportId).SendAsync("ChatStream", message);
        }

        public Task SendMessageToPlayersAsync(IEnumerable<IPlayer> players, string message)
        {
            return _hub.Clients.Clients(players.Select(p => p.TransportId).ToList()).SendAsync("ChatStream", message);
        }

        public Task SendViewCommandsToMapAsync(IMap map, IViewCommand<IViewItem> viewCommand)
        {
            return _hub.Clients.Group($"map_{map.Id}").SendAsync("WorldStream", ViewToOutput(viewCommand));
        }

        private IEnumerable<ViewOverWire> ViewToOutput(IViewCommand<IViewItem> viewCommand)
        {
            var operations = viewCommand.Operations.ToList();
            var output = new List<ViewOverWire>();


            foreach (var operation in operations)
            {
                var objectToSend = new ViewOverWire()
                {
                    OperationType = operation.OperationType,
                    RelatedId = operation.RelatedId,
                    Output = BuildOperationOutput(operation)
                };
                output.Add(objectToSend);
            }


            return output;
        }
        private class ViewOverWire
        {
            public ViewOperationType OperationType { get; set; }
            public string RelatedId { get; set; }
            public string Output { get; set; }

        }

        public Task SendViewCommandsToMapExceptAsync(IMap map, IEnumerable<IPlayer> players, IViewCommand<IViewItem> viewCommand)
        {
            return _hub.Clients.GroupExcept($"map_{map.Id}", players.Select(p => p.TransportId).ToList()).SendAsync("WorldStream", ViewToOutput(viewCommand));
        }

        public Task SendViewCommandsToMapExceptAsync(IMap map, IPlayer player, IViewCommand<IViewItem> viewCommand)
        {

            return SendViewCommandsToMapExceptAsync(map, new List<IPlayer>() { player }, viewCommand);
        }

        public Task SendViewCommandsToPlayerAsync(IPlayer player, IViewCommand<IViewItem> viewCommand)
        {
            return _hub.Clients.Client(player.TransportId).SendAsync("WorldStream", ViewToOutput(viewCommand));
        }

        private string BuildOperationOutput(IViewOperation<IViewItem> operation)
        {
            return string.Join("", operation.Data.Select((viewItem) => _viewBuilder.Build(viewItem)));
        }

        public async Task AddConnectionAsync(string transportId)
        {
            _connections.Add(transportId);
            if (this.Connected != null)
                await this.Connected(this, transportId);

        }

        public async Task RemoveConnectionAsync(string transportId)
        {
            _connections.RemoveAll(o => o == transportId);
            if (this.Disconnected != null)
                await this.Disconnected(this, transportId);
        }

        public Task SendViewCommandsToAllAsync(IViewCommand<IViewItem> viewCommand)
        {
            return _hub.Clients.All.SendAsync("WorldStream", ViewToOutput(viewCommand));
        }

        public Task SendViewCommandsToAllExceptAsync(IPlayer player, IViewCommand<IViewItem> viewCommand)
        {
            return _hub.Clients.AllExcept(new List<string>() { player.TransportId }).SendAsync("WorldStream", ViewToOutput(viewCommand));
        }

        public Task SendViewCommandsToPlayersAsync(IEnumerable<IPlayer> players, IViewCommand<IViewItem> viewCommand)
        {
            return _hub.Clients.Clients(players.Select(p => p.TransportId).ToList()).SendAsync("WorldStream", ViewToOutput(viewCommand));
        }
    }

}
