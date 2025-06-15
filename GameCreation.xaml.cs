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

        private void OnCreateGame(Object sender, EventArgs e)
        {

        }

        private async void OnJoinGame(Object sender, EventArgs e)
        {
            try
            {
                await DisplayPromptAsync("Join Game", "Enter the game code:", "Join", "Cancel", "Game Code", 6, keyboard: Keyboard.Numeric);
                // This is where I'd be querrying the server to join the game but for now we just go straigh to the waiting room
                await Shell.Current.GoToAsync("Waiting");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error joining game: {ex.Message}");
                await DisplayAlert("Error", "Failed to join the game. Please try again.", "OK");
                return;
            }
        }
    }
}
