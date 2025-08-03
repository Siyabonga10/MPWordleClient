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
        public static async Task InitGame()
        {
            GameID = await MpClient.CreateGame();
        }
    }
}
