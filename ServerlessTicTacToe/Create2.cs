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
    public static class Create2
    {
        [FunctionName("Create2")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post")]HttpRequestMessage req, [Table("Game", Connection = "AzureWebJobsStorage")]ICollector<Game> outTable, TraceWriter log)
        {
            dynamic data = await req.Content.ReadAsAsync<object>();
            string name = data?.name;

            if (name == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name in the request body");
            }

            outTable.Add(new Game()
            {
                PartitionKey = "Functions",
                RowKey = Guid.NewGuid().ToString(),
                Player1 = name,
                PlayerTurn = 2,
                Row1 = "---",
                Row2 = "---",
                Row3 = "---"
            });
            return req.CreateResponse(HttpStatusCode.Created);
        }
    }
}
