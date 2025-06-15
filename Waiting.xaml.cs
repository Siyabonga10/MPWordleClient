using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWordleClient
{
    partial class Waiting: ContentPage
    {
        public Waiting()
        {
            InitializeComponent();
            try
            {
                GetGameID();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting game ID: {ex.Message}");
            }

        }
        private void OnStartGame(Object sender, EventArgs e)
        { 

        }

        private async Task GetGameID()
        {
            await Task.Delay(1000); // Simulate a delay for getting the game ID
            WaitingLb.Text = "Game ID: 12345"; // Update the label with the game ID
        }

    }
}
