using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IWorld
    {


        event Func<object, IMapChangeEvent, Task> PlayerMoved;

        IReadOnlyList<IMap> Maps { get; }

        string Name { get; }

        Task MovePlayerAsync(IPlayer player, IMap map);

        void AddMap(IMap map);

        IMap GetStarterMap();




    }
}
