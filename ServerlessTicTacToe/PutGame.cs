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
    public static class PutGame
    {
        [FunctionName("PutGame")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "put")]Game game, [Table("Game", Connection = "")]CloudTable outTable, TraceWriter log)
        {
            if (string.IsNullOrEmpty(person.Name))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("A non-empty Name must be specified.")
                };
            };

            log.Info($"PersonName={person.Name}");

            TableOperation updateOperation = TableOperation.InsertOrReplace(person);
            TableResult result = outTable.Execute(updateOperation);
            return new HttpResponseMessage((HttpStatusCode)result.HttpStatusCode);
        }

        public class Person : TableEntity
        {
            public string Name { get; set; }
        }
    }
}
