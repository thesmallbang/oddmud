﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IWorld
    {


        event Func<object, IMapChangeEvent, Task> MapChanged;

        IReadOnlyList<IMap> Maps { get; }

        string Name { get; }

        Task MovePlayerAsync(IPlayer player, IMap map);

        IMap GetStarterMap();


    }
}
