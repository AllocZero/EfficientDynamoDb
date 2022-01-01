using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EfficientDynamoDb;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Extensions;
using EfficientDynamoDb.Internal.Crc;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Signing;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var client = DynamoDbPartiqlClient.CreateLocal("http://localhost:8000");

await client.ExecuteStatementAsync("SELECT * FROM \"RPG1\"");



Console.WriteLine();


[DynamoDbTable("RPG1")]
public class temp
{
    [DynamoDbProperty("PK", DynamoDbAttributeType.PartitionKey)]

    public string PK { get; set; }

    [DynamoDbProperty("SK", DynamoDbAttributeType.SortKey)]
    public string SK { get; set; }
}