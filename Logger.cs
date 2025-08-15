using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MPWordleClient
{
    public static class AppLogger
    {
        public static ILogger<App>? Logger;
        public static void InitialiseLogger(ILogger<App> logger)
        {
            Logger = logger;
        }
    }
}
