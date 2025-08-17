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
        private readonly ILogger<Waiting> _logger;
        public Waiting(ILogger<Waiting> logger)
        {
            InitializeComponent();
            _logger = logger;
            MpClient.PlayerJoinedEvent += OnPlayerJoined;
        }
        private void OnStartGame(Object sender, EventArgs e)
        {

        }

        public void OnPlayerJoined(Object? sender, string playerUsername)
        {
            Label NewPlayerLb = new()
            {
                Text = $"{playerUsername}",
                VerticalOptions = LayoutOptions.Start,
                FontSize = 30,
                Margin = new(50, 0, 0, 0),
                TextColor = Colors.Black,
                FontAttributes = FontAttributes.Bold,
            };
            PlayersList.Add(NewPlayerLb);
        }
    }
}
