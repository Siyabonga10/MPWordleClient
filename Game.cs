using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace MPWordleClient
{
    public static class Game
    {
        public static string GameID { get; set; } = "Loading";
        public static string GameIDText { get; set; } = "Loading";
        public static async Task InitGame()
        {

            GameID = await MpClient.CreateGame();
            GameIDText = $"GameID: {GameID}";
        }

        public static async Task SubscribeToEvents()
        {
            await MpClient.SubscribeToGameUpdates(GameID);
        }

        public static async Task<bool> JoinGame(string gameId)
        {
            var joined = await MpClient.JoinGame(gameId);
            if (joined)
                GameID = gameId;
            return joined;
        }
    }
}
