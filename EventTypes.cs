using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWordleClient
{
    public static class EventTypes
    {
        public const string PlayersInGame = "PlayersInGame";
        public const string PlayerJoined = "PlayerJoined";
        public const string StartGame = "StartGame";
        public const string WinnerUpdate = "WinnerUpdate";
    }
}
