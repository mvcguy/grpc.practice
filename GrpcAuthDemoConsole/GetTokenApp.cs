using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrpcAuthDemoConsole
{
    public static class GetTokenApp
    {
        public static async Task<string> GetJwt(string email)
        {
            using var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5001/jwt/token")
            {
                Version = new Version(2, 0)
            };
            request.Headers.Add("email", email);
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }
    }


}
