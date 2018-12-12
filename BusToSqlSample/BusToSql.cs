using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace BusToSqlSample
{
    public static class BusToSql
    {
        [FunctionName("BusToSql")]
        public static void Run(
            [ServiceBusTrigger("ionmessages", AccessRights.Listen, Connection = "sbConnection")]
                string myQueueItem,
            TraceWriter log)
        {
            log.Info($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            var e = JsonConvert.DeserializeObject<MyMessage>(myQueueItem);
            log.Info($"Message: {e.Id}, {e.FirstName}, {e.LastName}");

            ///Write to SQL
            var cnnString = "";

            using (SqlConnection conn = new SqlConnection(cnnString))
            {
                conn.Open();

                // Insert Signup        
                var signupInsert = "INSERT INTO [dbo].[Messages] ([FirstName],[LastName],[Id])" +
                "VALUES ('" + e.FirstName + "','" + e.LastName + "','" + e.Id + "')";

                log.Info($"Insert: {signupInsert}");

                // Execute and load data into database.
                using (SqlCommand cmd = new SqlCommand(signupInsert, conn))
                {
                    var rows = cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
