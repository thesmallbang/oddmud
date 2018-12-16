﻿using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class AfterPlayerLogoutPlugin : IEventPlugin
    {

        public string Name => nameof(AfterPlayerLogoutPlugin);
        public BasicGame.Game Game;

        public void Configure(IGame game)
        {
            Game = (BasicGame.Game)game;
            Game.PlayerRemoved += AfterPlayerLogout;
        }

        private async Task AfterPlayerLogout(Object sender, IPlayer player)
        {

            var playersLeftBehind = player.Map.Players.Where(p => p.Name != player.Name).Select(o => o.Name);

            var leftBehindNotification = new MudLikeCommandBuilder()
           .AddText("players: ")
           .AddText(string.Join(",", playersLeftBehind), TextColor.Gray)
           .AddTextLine($" -{player.Name}", TextColor.Red)
            .Build(ViewCommandType.Replace);


            await Game.Network.SendViewCommandsToMapAsync(player.Map, leftBehindNotification);
            await player.Map.RemovePlayerAsync(player);
        }

    }
}
