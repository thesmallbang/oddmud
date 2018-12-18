using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.BasicGame;
using OddMud.BasicGame.Commands;
using OddMud.BasicGame.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OddMud.SampleGamePlugins;
using OddMud.View.MudLike;

namespace OddMud.BasicGamePlugins.CommandPlugins
{
    public class LookPlugin : LoggedInCommandPlugin
    {
        public override string Name => nameof(LookPlugin);
        public override IReadOnlyList<string> Handles => _handles;
        private List<string> _handles = new List<string>() { "look" };

        public new GridGame Game => (GridGame)base.Game;


        public override async Task LoggedInProcessAsync(IProcessorData<CommandModel> request, IPlayer player)
        {

            switch (request.Data.FirstPart)
            {
                case "look":
                    await ProcessBasicLook(request, player);
                    break;

            }

            await base.LoggedInProcessAsync(request, player);
        }

        private async Task ProcessBasicLook(IProcessorData<CommandModel> request, IPlayer player)
        {
            request.Handled = true;
            if (!string.IsNullOrEmpty(request.Data.SecondPart))
            {
                await Game.Network.SendMessageToPlayerAsync(player, "Complex look not yet supported. Processing without arguments.");
            }

            var mapView = MudLikeCommandBuilder.Start()
                .AddWorldDate(Game.World.Time.WorldTime)
                .AddMap((GridMap)player.Map, includePlayers: true)
                .Build(ViewCommandType.Set);

            await Game.Network.SendViewCommandsToPlayerAsync(player, mapView);


        }
    }
}
