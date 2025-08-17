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
    enum EventParsingStage
    {
        WAITING,
        PARSING_BODY,
        THROW // Using this for events that arent defined
    }
    delegate void EventProcessor(string line);
    public static class MpClient
    {
        public static HttpClient HttpClient { get; }
        public static readonly string BaseUrl = "https://mpwordle-ase2a7h9d9hjhwcn.southafricanorth-01.azurewebsites.net";
        public static event EventHandler<string> PlayerJoinedEvent;
        private static string CurrentEventType = string.Empty;
        private static EventParsingStage CurrentStage = EventParsingStage.WAITING;
        private static readonly Dictionary<string, EventProcessor> EventHandlers = [];
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

            EventHandlers.Add(EventTypes.PlayerJoined, OnPlayerJoined);
            EventHandlers.Add(EventTypes.PlayersInGame, OnPlayerJoined);
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
            AppLogger.Logger?.LogInformation($"Coming from thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");
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
                AppLogger.Logger?.LogInformation($"Coming from thread xx {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                while ((line = await reader.ReadLineAsync(_cancellationTokenSource.Token)) != null &&
                       !_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    AppLogger.Logger?.LogInformation($"APP LOG: {line}");
                    ProcessEventLine(line);
                }
            }
            catch (Exception ex) {
                AppLogger.Logger?.LogError(ex, "Operation failed");
            }
        }

        public static async Task<bool> JoinGame(string gameId)
        {
            var response = await HttpClient.PutAsync(BaseUrl + $"/game/{gameId}", null);
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        private static void ProcessEventLine(string line)
        {
            if (CurrentStage == EventParsingStage.WAITING)
            {
                if (line.Contains(EventTypes.Event))
                {
                    var eventType = line.Split(":")[1];
                    eventType = eventType?.Trim();
                    if (eventType != null && !EventTypes.AllEvents.Contains(eventType))
                    {
                        CurrentStage = EventParsingStage.WAITING;
                    }
                    else
                    {
                        CurrentEventType = eventType!;
                        CurrentStage = EventParsingStage.PARSING_BODY;
                    }
                }
            }
                
            else if (CurrentStage == EventParsingStage.PARSING_BODY)
            {
                if (EventHandlers.ContainsKey(CurrentEventType))
                {
                    var handler = EventHandlers[CurrentEventType];
                    handler(line.Trim());
                }
                else
                    if (line == string.Empty)
                    CurrentStage = EventParsingStage.WAITING;
            }
        }

        private static void OnPlayerJoined(string line)
        {
            
            if (line == string.Empty)
            {
                CurrentStage = EventParsingStage.WAITING;
                return;
            }
            else if (line.Contains("data"))
            {
                line = line.Replace("data: ", "");
                PlayerJoinedEvent.Invoke(null, line.Trim());
            }
        }
    }
}