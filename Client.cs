using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using System.Net.Http.Json;

namespace MPWordleClient
{
    public static class MpClient
    {
        public static HttpClient HttpClient { get; }
        public static readonly string BaseUrl = "https://mpwordle-ase2a7h9d9hjhwcn.southafricanorth-01.azurewebsites.net";
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
            Console.WriteLine("XOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOXOX");
            var body = new { username, password };
            var content = JsonContent.Create(body);
            var response = await HttpClient.PostAsync(BaseUrl + endpoint, content);
            return response;
        }
    }
}
