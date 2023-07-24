using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using EfficientDynamoDb.Lambda;

namespace LambdaProject
{
    public class Handler
    {
        public async Task RunAsync(Stream stream)
        {
            LambdaLogger.Log("RunAsync Started\n");
            
            var request = await LambdaSerializer.DeserializeAsync(stream);

            foreach (var record in request.Records)
            {
                LambdaLogger.Log($"{record.EventId}\n");
            }
            
        }
    }
}