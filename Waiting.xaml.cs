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
            var tmp = BackgroundColor;
            DataLB.Text += $"\n {playerUsername}\n";
        }

        public void OnPlayersInGame(object? sender, List<string> usernames)
        {
            foreach(var username in usernames)
                OnPlayerJoined(sender, username);
        }   
    }
}
