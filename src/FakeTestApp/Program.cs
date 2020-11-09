using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var ddbConfig = new AmazonDynamoDBConfig {RegionEndpoint = RegionEndpoint.USEast1};
            var dbClient =  new AmazonDynamoDBClient(new BasicAWSCredentials(Environment.GetEnvironmentVariable("DEV_AWS_PUBLIC_KEY"), Environment.GetEnvironmentVariable("DEV_AWS_PRIVATE_KEY")), ddbConfig);
           
            var contextConfig = new DynamoDBContextConfig
            {
                TableNamePrefix = "production_",
                Conversion = DynamoDBEntryConversion.V2
            };

            var context = new DynamoDBContext(dbClient, contextConfig);

            // await context.SaveAsync(new Coin
            // {
            //     Pk = "test_pk",
            //     Sk = "test_sk"
            // });
            //
            // for (var i = 0; i < 10000; i++)
            // {
            //     await context.SaveAsync(new Coin
            //     {
            //         Pk = "test_pk",
            //         Sk = "test_sk"
            //     }).ConfigureAwait(false);
            // }
            //
            // var stopwatch = Stopwatch.StartNew();
            //
            // for (var i = 0; i < 10000; i++)
            // {
            //     await context.SaveAsync(new Coin
            //     {
            //         Pk = "test_pk",
            //         Sk = "test_sk"
            //     }).ConfigureAwait(false);
            // }
            //
            // stopwatch.Stop();
            //
            // Console.WriteLine($"Raw Benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
            //
            // stopwatch.Restart();
            //
            // Parallel.ForEach(Enumerable.Range(0, 10000), _ =>
            // {
            //     context.SaveAsync(new Coin
            //     {
            //         Pk = "test_pk",
            //         Sk = "test_sk"
            //     }).Wait();
            // });
            //
            // stopwatch.Stop();
            //
            // Console.WriteLine($"Raw Parallel.ForEach Benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
            //
            // stopwatch.Restart();
            //
            // for (var i = 0; i < 10000; i++)
            // {
            //     await svContext.SaveAsync(new Coin
            //     {
            //         Pk = "test_pk",
            //         Sk = "test_sk"
            //     }, UpdateBuilder.Create<Coin>()).ConfigureAwait(false);
            // }
            //
            // stopwatch.Stop();
            //
            // Console.WriteLine($"SV Raw Benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
            //
            // stopwatch.Restart();
            //
            // for (var i = 0; i < 10000; i++)
            // {
            //     await svContext.SaveAsync(new Coin
            //     {
            //         Pk = "test_pk",
            //         Sk = "test_sk"
            //     }, UpdateBuilder.Create<Coin>()
            //         .Add(x=>x.F1, "test1")
            //         .Add(x=>x.F2, "test2")
            //         .Add(x=>x.F3, "test3")
            //         .Add(x=>x.F4, "test4")
            //         .Add(x=>x.F5, "test5")
            //         .Add(x=>x.F6, "test6")
            //         .Add(x=>x.F7, "test7")
            //         .Add(x=>x.F8, "test8")
            //         .Add(x=>x.F9, "test9")
            //         .Add(x=>x.F10, "test10")
            //         .Add(x=>x.F11, "test11")
            //         .Add(x=>x.F12, "test12")
            //         .Add(x=>x.F1, "test1")
            //         .Add(x=>x.F2, "test2")
            //         .Add(x=>x.F3, "test3")
            //         .Add(x=>x.F4, "test4")
            //         .Add(x=>x.F5, "test5")
            //         .Add(x=>x.F6, "test6")
            //         .Add(x=>x.F7, "test7")
            //         .Add(x=>x.F8, "test8")
            //         .Add(x=>x.F9, "test9")
            //         .Add(x=>x.F10, "test10")
            //         .Add(x=>x.F11, "test11")
            //         .Add(x=>x.F12, "test12")).ConfigureAwait(false);
            // }
            //
            // stopwatch.Stop();
            //
            // Console.WriteLine($"SV Update Benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
            
            
            // Query ///////////////////////////////////////////////////////////////////////////////////////////

            var data = await context.QueryAsync<Coin>("test_pk").GetRemainingAsync();
            
            var stopwatch = Stopwatch.StartNew();
            
            for (var i = 0; i < 10; i++)
            {
                await context.QueryAsync<Coin>("test_pk").GetRemainingAsync();
            }
            
            stopwatch.Stop();
            
            Console.WriteLine($"Raw Query 10 Benchmark: {stopwatch.Elapsed.TotalMilliseconds}");
            int ss = 2;
        }
        
        [DynamoDBTable("coins_system_v2")]
        public class Coin
        {
            [DynamoDBHashKey("pk")]
            public string Pk { get; set; }
            
            [DynamoDBRangeKey("sk")]
            public string Sk { get; set; }

            [Required]
            [DynamoDBProperty("f1")] 
            public string F1 { get; set; } = "test";
            
            [Required]
            [DynamoDBProperty("f2")] 
            public string F2 { get; set; } = "test";
            
            
            [DynamoDBProperty("f3")] 
            public string F3 { get; set; } = "test";
            
            [DynamoDBProperty("f4")] 
            public string F4 { get; set; } = "test";
            
            [DynamoDBProperty("f5")] 
            public string F5 { get; set; } = "test";
            
            [DynamoDBProperty("f6")] 
            public string F6 { get; set; } = "test";
            
            [DynamoDBProperty("f7")] 
            public string F7 { get; set; } = "test";
            
            [DynamoDBProperty("f8")] 
            public string F8 { get; set; } = "test";
            
            [DynamoDBProperty("f9")] 
            public string F9 { get; set; } = "test";
            
            [DynamoDBProperty("f10")] 
            public string F10 { get; set; } = "test";
            
            [DynamoDBProperty("f11")] 
            public string F11 { get; set; } = "test";
            
            [DynamoDBProperty("f12")] 
            public string F12 { get; set; } = "test";
        }
    }
}