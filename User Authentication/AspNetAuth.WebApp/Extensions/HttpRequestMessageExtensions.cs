using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AspNetAuth.WebApp.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static async Task PutCurrentUserAccessToken(this HttpRequestMessage httpRequestMessage,
            HttpContext httpContext)
        {
            var userAccessToken = await httpContext.GetTokenAsync("access_token");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);
        }
    }
}