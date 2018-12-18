using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{

    public interface ISpawner
    {
        ISpawnable SpawnedEntity { get; set; }

        Task SpawnerTick();

        Task ResetAsync();
    }

   
}
