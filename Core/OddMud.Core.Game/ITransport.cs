using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface ITransport
    {
        void SendMessageToPlayer(string networkId, string message);
        void SendMessageToPlayer(IPlayer player, string message);
        void SendMessageToPlayers(IEnumerable<IPlayer> players, string message);

        void SendMessageToMap(IMap map, string message);
        void SendMessageToMapExcept(IMap map, IEnumerable<IPlayer> players, string message);
        void SendMessageToMapExcept(IMap map, IPlayer player, string message);

        void SendViewCommandsToPlayer(IPlayer player, IEnumerable<IViewCommand> commands);
        void SendViewCommandsToMap(IMap map, IEnumerable<IViewCommand> commands);
        void SendViewCommandsToMapExcept(IMap map, IEnumerable<IPlayer> players, IEnumerable<IViewCommand> commands);
        void SendViewCommandsToMapExcept(IMap map, IPlayer player, IEnumerable<IViewCommand> commands);


        Task AddPlayerToMapGroupAsync(IPlayer player, IMap map);
        Task RemovePlayerFromMapGroupAsync(IPlayer player, IMap map);


    }
}
