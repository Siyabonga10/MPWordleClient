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
            WaitingLb.Text = "Game ID:" + Game.GameID.ToString();

        }
        private void OnStartGame(Object sender, EventArgs e)
        {

        }

    }
}
