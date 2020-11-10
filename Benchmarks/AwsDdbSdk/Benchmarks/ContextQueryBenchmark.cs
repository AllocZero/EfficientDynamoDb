using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using AWSSDK.Core.NetStandard.Amazon.Runtime.Pipeline.HttpHandler;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Benchmarks.AwsDdbSdk.Entities;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class ContextQueryBenchmark : DdbBenchmarkBase
    {
        private const string KeysOnlyEntityPk = "keys_only_bench";
        private const string MediumEntityPk = "medium_bench_v2";
        private const string MediumComplexEntityPk = "medium_complex_bench_v2";
        private const string LargeEntityPk = "large_bench";

        [GlobalSetup(Target = nameof(KeysOnlyBenchmarkAsync))]
        public Task SetupKeysOnlyBenchmarkAsync() => SetupBenchmarkAsync<KeysOnlyEntity>(KeysOnlyEntityPk);

        [Benchmark(Baseline = true)]
        public Task<int> KeysOnlyBenchmarkAsync() => PerformBenchmarkAsync<KeysOnlyEntity>(KeysOnlyEntityPk);
        
        [GlobalSetup(Target = nameof(MediumBenchmarkAsync))]
        public Task SetupMediumBenchmarkAsync() => SetupBenchmarkAsync<MediumStringFieldsEntity>(MediumEntityPk);

        [Benchmark]
        public Task<int> MediumBenchmarkAsync() => PerformBenchmarkAsync<MediumStringFieldsEntity>(MediumEntityPk);
        
        [GlobalSetup(Target = nameof(MediumComplexBenchmarkAsync))]
        public Task SetupComplexBenchmarkAsync() => SetupBenchmarkAsync<MediumComplexFieldsEntity>(MediumComplexEntityPk);

        [Benchmark]
        public Task<int> MediumComplexBenchmarkAsync() => PerformBenchmarkAsync<MediumComplexFieldsEntity>(MediumComplexEntityPk);
        
        [GlobalSetup(Target = nameof(LargeBenchmarkAsync))]
        public Task SetupLargeBenchmarkAsync() => SetupBenchmarkAsync<LargeStringFieldsEntity>(LargeEntityPk);

        [Benchmark]
        public Task<int> LargeBenchmarkAsync() => PerformBenchmarkAsync<LargeStringFieldsEntity>(LargeEntityPk);

        private async Task<int> PerformBenchmarkAsync<T>(string pk) where T: KeysOnlyEntity
        {
            var entities = await DbContext.QueryAsync<T>(pk).GetRemainingAsync().ConfigureAwait(false);

            return entities.Count;
        }
    }
}