using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Benchmarks.AwsDdbSdk.Entities;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    public abstract class QueryBenchmarkBase : DdbBenchmarkBase
    {
        private const string KeysOnlyEntityPk = "keys_only_bench";
        private const string MediumEntityPk = "medium_bench_v2";
        private const string MediumComplexEntityPk = "medium_complex_bench_v3";
        private const string MediumComplexCollectionEntityPk = "medium_complex_col_bench";
        private const string LargeEntityPk = "large_bench";

        [GlobalSetup(Target = nameof(KeysOnlyBenchmarkAsync))]
        public Task SetupKeysOnlyBenchmarkAsync() => SetupBenchmarkAsync<KeysOnlyEntity>(KeysOnlyEntityPk);

        [Benchmark(Baseline = true)]
        public Task<int> KeysOnlyBenchmarkAsync() => QueryAsync<KeysOnlyEntity>(KeysOnlyEntityPk);
        
        [GlobalSetup(Target = nameof(MediumBenchmarkAsync))]
        public Task SetupMediumBenchmarkAsync() => SetupBenchmarkAsync<MediumStringFieldsEntity>(MediumEntityPk);

        [Benchmark]
        public Task<int> MediumBenchmarkAsync() => QueryAsync<MediumStringFieldsEntity>(MediumEntityPk);
        
        [GlobalSetup(Target = nameof(MediumComplexBenchmarkAsync))]
        public Task SetupComplexBenchmarkAsync() => SetupBenchmarkAsync<MediumComplexFieldsEntity>(MediumComplexEntityPk);

        [Benchmark]
        public Task<int> MediumComplexBenchmarkAsync() => QueryAsync<MediumComplexFieldsEntity>(MediumComplexEntityPk);
        
        [GlobalSetup(Target = nameof(MediumComplexCollectionBenchmarkAsync))]
        public Task SetupComplexCollectionBenchmarkAsync() => SetupBenchmarkAsync<MediumComplexCollectionFieldsEntity>(MediumComplexCollectionEntityPk);

        [Benchmark]
        public Task<int> MediumComplexCollectionBenchmarkAsync() => QueryAsync<MediumComplexCollectionFieldsEntity>(MediumComplexCollectionEntityPk);
        
        [GlobalSetup(Target = nameof(LargeBenchmarkAsync))]
        public Task SetupLargeBenchmarkAsync() => SetupBenchmarkAsync<LargeStringFieldsEntity>(LargeEntityPk);

        [Benchmark]
        public Task<int> LargeBenchmarkAsync() => QueryAsync<LargeStringFieldsEntity>(LargeEntityPk);
    }
}