using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessTicTacToe
{
    public static class CreateGame
    {
        [FunctionName("CreateGame")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post")]HttpRequestMessage req, [Table("Game", Connection = "AzureWebJobsStorage")]ICollector<Game> outTable, TraceWriter log)
        {
            dynamic data = await req.Content.ReadAsAsync<object>();
            string name = data?.name;

            if (name == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name in the request body");
            }

            Game game =
            new Game()
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = "-",
                Player1 = name,
                PlayerTurn = 2,
                Row1 = "---",
                Row2 = "---",
                Row3 = "---"
            };
            outTable.Add(game);
            return req.CreateResponse(HttpStatusCode.Created, game.PartitionKey);
        }
    }
}
