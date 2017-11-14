using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using System.Linq;

namespace ServerlessTicTacToe
{
    public static class JoinGame
    {
        [FunctionName("JoinGame")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post")]HttpRequestMessage req, [Table("Game", Connection = "AzureWebJobsStorage")]CloudTable outTable, [Table("Game", Connection = "AzureWebJobsStorage")]IQueryable<Game> inTable, TraceWriter log)
        {
            dynamic data = await req.Content.ReadAsAsync<object>();
            string name = data?.name;
            string key = data?.partitionKey;

            if (key == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name in the request body");
            }

            if (name == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a partitionKey in the request body");
            }

            var query = from g in inTable where g.PartitionKey == key select g;
            var game = query.FirstOrDefault();

            if(!string.IsNullOrEmpty(game.Player2))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please join anopther game, this one is already full");
            }

            game.Player2 = name;

            TableOperation updateOperation = TableOperation.Replace(game);
            TableResult result = outTable.Execute(updateOperation);

            return req.CreateResponse(HttpStatusCode.Created, game);
        }
    }
}
