using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface ITransport
    {
        Task SendMessageToPlayerAsync(string transportId, string message);
        Task SendMessageToPlayerAsync(IPlayer player, string message);
        Task SendMessageToPlayersAsync(IEnumerable<IPlayer> players, string message);

        Task SendMessageToMapAsync(IMap map, string message);
        Task SendMessageToMapExceptAsync(IMap map, IEnumerable<IPlayer> players, string message);
        Task SendMessageToMapExceptAsync(IMap map, IPlayer player, string message);

        Task SendViewCommandsToPlayerAsync(IPlayer player, IViewCommand<IViewItem> viewCommand);
        Task SendViewCommandsToMapAsync(IMap map, IViewCommand<IViewItem> viewCommand);
        Task SendViewCommandsToMapExceptAsync(IMap map, IEnumerable<IPlayer> players, IViewCommand<IViewItem> viewCommand);
        Task SendViewCommandsToMapExceptAsync(IMap map, IPlayer player, IViewCommand<IViewItem> viewCommand);


        Task AddPlayerToMapGroupAsync(IPlayer player, IMap map);
        Task RemovePlayerFromMapGroupAsync(IPlayer player, IMap map);


    }
}
