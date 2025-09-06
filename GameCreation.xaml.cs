using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MPWordleClient
{
    partial class GameCreation: ContentPage
    {
        public GameCreation() 
        {
            InitializeComponent();
            JoinGameBtn.WidthRequest = CreateGameBtn.Width;
        }

        private async void OnCreateGame(Object sender, EventArgs e)
        {
            await Game.InitGame();
            var task1 = Game.SubscribeToEvents();
            var task2 = Navigation.PushModalAsync(new Waiting());
            await Task.WhenAll(task1, task2);
        }

        private async void OnJoinGame(Object sender, EventArgs e)
        {
            var gameId = await DisplayPromptAsync("Join Game", "Enter the game id");
            if (await Game.JoinGame(gameId))
            {
                await Task.WhenAll(Game.SubscribeToEvents(), Navigation.PushModalAsync(new Waiting()));
            }
            else
                await DisplayAlert("Join Game", "Could not join game", "ok");
        }
    }
}
