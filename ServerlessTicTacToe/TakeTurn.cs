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
    public static class TakeTurn
    {
        [FunctionName("TakeTurn")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post")]HttpRequestMessage req, [Table("Game", Connection = "AzureWebJobsStorage")]CloudTable outTable, [Table("Game", Connection = "AzureWebJobsStorage")]IQueryable<Game> inTable, TraceWriter log)
        {
            dynamic data = await req.Content.ReadAsAsync<object>();
            int? pos = data?.position;
            string player = data?.player;
            string key = data?.partitionKey;

            if (key == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a gameid in the request body");
            }

            if (pos == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a position in the request body");
            }

            if (pos < 0 || pos > 8)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a position from 0 to 8");
            }

            if (player == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a player in the request body");
            }

            var query = from g in inTable where g.PartitionKey == key select g;
            var game = query.FirstOrDefault();

            if (game.Board[(int)pos] != '-')
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pick another position, this one is already filled");
            }

            char XorO = 'X';
            if (game.Player1 == player)
                XorO = 'X';
            if (game.Player2 == player)
                XorO = 'O';

            var array = game.Board.ToCharArray();
            array[(int)pos] = XorO;

            game.Board = string.Join("",array);
            game.PlayerTurn = game.PlayerTurn == 1 ? 2 : 1;

            TableOperation updateOperation = TableOperation.Replace(game);
            TableResult result = outTable.Execute(updateOperation);

            return req.CreateResponse(HttpStatusCode.Created, game);
        }
    }
}
