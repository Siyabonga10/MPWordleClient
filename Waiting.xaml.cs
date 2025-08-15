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
            MpClient.NewStream += OnNewStream;
        }
        private void OnStartGame(Object sender, EventArgs e)
        {

        }

        public void OnNewStream(Object? sender, string e)
        {
            var tmp = BackgroundColor;
            DataLB.Text += $"\n New Log {e}\n";
        }

    }
}
