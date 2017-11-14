using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Net.Http.Headers;
using System.IO;

namespace ServerlessTicTacToe
{
    public static class PlayGame
    {
        [FunctionName("PlayGame")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "PlayGame")]HttpRequestMessage req, TraceWriter log)
        {
            string path = @"TicTacToe.html";
            string html = File.ReadAllText(path);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(html);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
