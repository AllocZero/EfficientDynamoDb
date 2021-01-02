using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Benchmarks.AwsDdbSdk.Entities;
using Benchmarks.Mocks;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    public abstract class QueryBenchmarkBase : DdbBenchmarkBase
    {
        private const string KeysOnlyEntityPk = "keys_only_bench";
        private const string MediumEntityPk = "medium_bench_v4";
        private const string MediumComplexEntityPk = "medium_complex_bench_v4";
        private const string LargeEntityPk = "large_bench";
        private const string MixedEntityPk = "mixed_bench_v2";

        [GlobalSetup(Target = nameof(KeysOnlyBenchmarkAsync))]
        public void SetupKeysOnlyBenchmark() => SetupBenchmark<KeysOnlyEntity>(x => EntitiesFactory.CreateKeysOnlyEntity(x).ToDocument());

        // [Benchmark(Baseline = true)]
        public Task<int> KeysOnlyBenchmarkAsync() => RunBenchmarkAsync<KeysOnlyEntity>(KeysOnlyEntityPk);

        [GlobalSetup(Target = nameof(MediumBenchmarkAsync))]
        public void SetupMediumBenchmark() => SetupBenchmark<MediumStringFieldsEntity>(x => EntitiesFactory.CreateMediumStringEntity(x).ToDocument());

        [Benchmark]
        public Task<int> MediumBenchmarkAsync() => RunBenchmarkAsync<MediumStringFieldsEntity>(MediumEntityPk);
        
        [GlobalSetup(Target = nameof(MediumComplexBenchmarkAsync))]
        public void SetupComplexBenchmark() => SetupBenchmark<MediumComplexFieldsEntity>(x => EntitiesFactory.CreateMediumComplexEntity(x).ToDocument());
        
        // [Benchmark]
        public Task<int> MediumComplexBenchmarkAsync() => RunBenchmarkAsync<MediumComplexFieldsEntity>(MediumComplexEntityPk);
        
        [GlobalSetup(Target = nameof(LargeBenchmarkAsync))]
        public void SetupLargeBenchmark() => SetupBenchmark<LargeStringFieldsEntity>(x => EntitiesFactory.CreateLargeStringEntity(x).ToDocument());
        
        // [Benchmark]
        public Task<int> LargeBenchmarkAsync() => RunBenchmarkAsync<LargeStringFieldsEntity>(LargeEntityPk);
        
        [GlobalSetup(Target = nameof(MixedBenchmarkAsync))]
        public void SetupMixedBenchmark() => SetupBenchmark<MixedEntity>(x => EntitiesFactory.CreateMixedEntity(x).ToDocument());
        
        [Benchmark]
        public Task<int> MixedBenchmarkAsync() => RunBenchmarkAsync<MixedEntity>(MixedEntityPk);
    }
}