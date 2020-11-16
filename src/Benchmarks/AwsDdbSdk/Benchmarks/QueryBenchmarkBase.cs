using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Benchmarks.AwsDdbSdk.Entities;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    public abstract class QueryBenchmarkBase : DdbBenchmarkBase
    {
        private const string KeysOnlyEntityPk = "keys_only_bench";
        private const string MediumEntityPk = "medium_bench_v4";
        private const string MediumComplexEntityPk = "medium_complex_bench_v4";
        private const string MediumConvertersEntityPk = "medium_converter_bench_v1";
        private const string MediumComplexCollectionEntityPk = "medium_complex_col_bench";
        private const string LargeEntityPk = "large_bench";
        private const string MixedEntityPk = "mixed_bench_v2";

        [GlobalSetup(Target = nameof(KeysOnlyBenchmarkAsync))]
        public Task SetupKeysOnlyBenchmarkAsync() => SetupBenchmarkAsync<KeysOnlyEntity>(KeysOnlyEntityPk);

        [Benchmark(Baseline = true)]
        public Task<int> KeysOnlyBenchmarkAsync() => RunBenchmarkAsync<KeysOnlyEntity>(KeysOnlyEntityPk);
        
        [GlobalSetup(Target = nameof(MediumBenchmarkAsync))]
        public Task SetupMediumBenchmarkAsync() => SetupBenchmarkAsync<MediumStringFieldsEntity>(MediumEntityPk);

        [Benchmark]
        public Task<int> MediumBenchmarkAsync() => RunBenchmarkAsync<MediumStringFieldsEntity>(MediumEntityPk);
        
        [GlobalSetup(Target = nameof(MediumComplexBenchmarkAsync))]
        public Task SetupComplexBenchmarkAsync() => SetupBenchmarkAsync<MediumComplexFieldsEntity>(MediumComplexEntityPk);

        [Benchmark]
        public Task<int> MediumComplexBenchmarkAsync() => RunBenchmarkAsync<MediumComplexFieldsEntity>(MediumComplexEntityPk);
        
        [GlobalSetup(Target = nameof(MediumConverterBenchmarkAsync))]
        public Task SetupMediumConverterBenchmarkAsync() => SetupBenchmarkAsync<MediumConverterFieldsEntity>(MediumConvertersEntityPk);

        [Benchmark]
        public Task<int> MediumConverterBenchmarkAsync() => RunBenchmarkAsync<MediumConverterFieldsEntity>(MediumConvertersEntityPk);
        
        [GlobalSetup(Target = nameof(MediumComplexCollectionBenchmarkAsync))]
        public Task SetupComplexCollectionBenchmarkAsync() => SetupBenchmarkAsync<MediumComplexCollectionFieldsEntity>(MediumComplexCollectionEntityPk);

        [Benchmark]
        public Task<int> MediumComplexCollectionBenchmarkAsync() => RunBenchmarkAsync<MediumComplexCollectionFieldsEntity>(MediumComplexCollectionEntityPk);
        
        [GlobalSetup(Target = nameof(LargeBenchmarkAsync))]
        public Task SetupLargeBenchmarkAsync() => SetupBenchmarkAsync<LargeStringFieldsEntity>(LargeEntityPk);

        [Benchmark]
        public Task<int> LargeBenchmarkAsync() => RunBenchmarkAsync<LargeStringFieldsEntity>(LargeEntityPk);
        
        [GlobalSetup(Target = nameof(MixedBenchmarkAsync))]
        public Task SetupMixedBenchmarkAsync() => SetupBenchmarkAsync<MixedEntity>(MixedEntityPk);

        [Benchmark]
        public Task<int> MixedBenchmarkAsync() => RunBenchmarkAsync<MixedEntity>(MixedEntityPk);
    }
}