using System.Net.Http.Headers;

namespace MBA.Marketplace.Web.Helpers
{
    public static class HttpClientExtensions
    {
        public static HttpClient CriarRequest(IHttpClientFactory _httpClientFactory, HttpContext context)
        {
            var client = _httpClientFactory.CreateClient();
            client.AdicionarToken(context);
            return client;
        }

        public static void AdicionarToken(this HttpClient client, HttpContext context)
        {
            var token = context.Request.Cookies["AccessToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
