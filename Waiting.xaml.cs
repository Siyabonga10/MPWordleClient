using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MPWordleClient
{
    partial class Waiting: ContentPage
    {
        public Waiting()
        {
            InitializeComponent();
            MpClient.PlayerJoinedEvent += OnPlayerJoined;
            MpClient.PlayersInGameEvent += OnPlayersInGame;
            MpClient.StartGame += OnServerStartGame;
        }
        public async void OnStartGame(object sender,  EventArgs e)
        {
            await MpClient.StartCurrentGame();
        }
        private async void OnServerStartGame(Object? sender, IEnumerable<string> words)
        {
            WordManager.LoadWords(words);
            await Shell.Current.GoToAsync("Gameplay");
        }

        public void OnPlayerJoined(Object? sender, string playerUsername)
        {
            var style = Application.Current?.Resources["PlayerLabel"] as Style;
            Label NewPlayerLb = new()
            {
                Text = $"{playerUsername}",
                Style = style
            };
            PlayersList.Add(NewPlayerLb);
        }

        public void OnPlayersInGame(Object? sender, List<string> usernames)
        {
            foreach(var username in usernames)
                OnPlayerJoined(sender, username);
        }   
    }
}
