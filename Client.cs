using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MPWordleClient
{
    public static class MpClient
    {
        public static HttpClient HttpClient { get; }
        public static readonly string BaseUrl = "https://mpwordle-ase2a7h9d9hjhwcn.southafricanorth-01.azurewebsites.net";
        public static event EventHandler<string> NewStream;
        static MpClient()
        {
            var cookieContainer = new CookieContainer();
            var handler  = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true,
                AllowAutoRedirect = true
            };
            HttpClient = new HttpClient(handler);
        }

        public static async Task<(bool LoggedIn, string OutcomeMsg)> LoginPlayerAsync(string username, string password)
        {
            var response = await PlayerAuth(username, password, "/player/login");
            if (response.StatusCode == HttpStatusCode.OK)
                return (true, "Player logged in");
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
                return (false, "Invalid credentials");
            else
                return (false, "Server error");
        }

        public static async Task<(bool LoggedIn, string OutcomeMsg)> CreatePlayerAsync(string username, string password)
        {
            var response = await PlayerAuth(username, password, "/player/create");
            if (response.StatusCode == HttpStatusCode.OK)
                return (true, "Player account created");
            else if (response.StatusCode == HttpStatusCode.Conflict)
                return (false, "Username already exists");
            else
                return (false, "Server error");
        }

        private static async Task<HttpResponseMessage> PlayerAuth(string username, string password, string endpoint)
        {
            AppLogger.Logger?.LogInformation("Handling player auth");
            var body = new { username, password };
            var content = JsonContent.Create(body);
            var response = await HttpClient.PostAsync(BaseUrl + endpoint, content);
            return response;
        }

        public static async Task<string> CreateGame()
        {
            var response = await HttpClient.PostAsync(BaseUrl + "/game", null);
            if(response.StatusCode == HttpStatusCode.Created)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(content);
                var gameId = jsonDoc.RootElement.GetProperty("shortId").GetString() ?? string.Empty;
                return gameId;
            }
            return string.Empty;
        }

        public static async Task SubscribeToGameUpdates(string gameID)
        {
            AppLogger.Logger?.LogInformation("Subbing to events");
            var _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(new("text/event-stream"));

                var response = await HttpClient.GetAsync(BaseUrl + $"/game/{gameID}",
                    HttpCompletionOption.ResponseHeadersRead,
                    _cancellationTokenSource.Token);

                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);               

                string? line;
                var dataBuilder = new StringBuilder();

                while ((line = await reader.ReadLineAsync()) != null &&
                       !_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    AppLogger.Logger?.LogInformation($"{line}");
                    if(line.Contains("\r"))
                        AppLogger.Logger?.LogInformation($"Found a carriadge return feed");
                    if (line == "")
                    {
                        AppLogger.Logger?.LogInformation("Dispatching event");
                        NewStream?.Invoke(null, dataBuilder.ToString());
                        dataBuilder.Clear();
                    }
                    else
                        dataBuilder.Append(line);
                }
            }
            catch (Exception ex) {
                AppLogger.Logger?.LogError($"Got some kind of error {ex.Message}");
                AppLogger.Logger?.LogError($"Exception data {ex.InnerException?.StackTrace}");
            }
        }

        public static async Task<bool> JoinGame(string gameId)
        {
            var response = await HttpClient.PutAsync(BaseUrl + $"/game/{gameId}", null);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
    }
}