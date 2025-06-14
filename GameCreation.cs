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

        private async Task OnJoinGame(Object sender, EventArgs e)
        {
            await DisplayPromptAsync("Join Game", "Enter the game code:", "Join", "Cancel", "Game Code", 6, keyboard: Keyboard.Numeric);
        }
    }
}
