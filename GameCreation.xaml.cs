using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var task2 = Shell.Current.GoToAsync("Waiting");
         
            await Task.WhenAll(task1, task2);
        }

        private async void OnJoinGame(Object sender, EventArgs e)
        {
            var gameId = await DisplayPromptAsync("Join Game", "Enter the game id");
            if(await Game.JoinGame(gameId))
            {
                Game.GameID = gameId;
                var task1 = Game.SubscribeToEvents();
                var task2 = Shell.Current.GoToAsync("Waiting");

                await Task.WhenAll(task1, task2);
            }
            else
            {
                await DisplayAlert("Join Game", "Could not join game", "ok");
            }
        }
    }
}
