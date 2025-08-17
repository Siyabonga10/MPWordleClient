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
            AppLogger.Logger?.LogInformation($"UI THREAD {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            await Game.InitGame();
            var task1 = Game.SubscribeToEvents();
            var task2 = Shell.Current.GoToAsync("Waiting");
            AppLogger.Logger?.LogInformation($"UI THREAD x {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            await Task.WhenAll(task1, task2);
        }

        private async void OnJoinGame(Object sender, EventArgs e)
        {
            AppLogger.Logger?.LogInformation($"UI THREAD {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            var gameId = await DisplayPromptAsync("Join Game", "Enter the game id");
            if (await Game.JoinGame(gameId))
            {
                AppLogger.Logger?.LogInformation($"UI THREAD x {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                await Task.WhenAll(Game.SubscribeToEvents(), Shell.Current.GoToAsync("Waiting"));
            }
            else
                await DisplayAlert("Join Game", "Could not join game", "ok");
        }
    }
}
