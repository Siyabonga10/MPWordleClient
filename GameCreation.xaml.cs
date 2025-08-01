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
        }

        private void OnJoinGame(Object sender, EventArgs e)
        {
        }
    }
}
