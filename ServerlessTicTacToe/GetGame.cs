using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace ServerlessTicTacToe
{
    public static class GetGame
    {
        [FunctionName("GetGame")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", Route ="GetGame/{gameId}")]HttpRequestMessage req, string gameId, [Table("Game", Connection = "AzureWebJobsStorage")]IQueryable<Game> inTable, TraceWriter log)
        {
            if (gameId == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a gameid in the request body");
            }

            var query = from g in inTable where g.PartitionKey == gameId select g;
            var game = query.FirstOrDefault();

            HttpStatusCode resultCode = HttpStatusCode.NotFound;
            if (game != null)
                resultCode = HttpStatusCode.OK;

            return req.CreateResponse(resultCode, game);
        }
    }
}
