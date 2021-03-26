using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Benchmarks.AwsDdbSdk.Entities;
using Benchmarks.Mocks;
using EfficientDynamoDb;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;

namespace Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class ToObjectBenchmark
    {
        [Params(10, 100, 1000)]
        public int EntitiesCount;

        private readonly DynamoDbContextMetadata _metadata = new DynamoDbContextMetadata(Array.Empty<DdbConverter>());
        private Document[] _documents;

        [GlobalSetup]
        public void Setup()
        {
            _documents = Enumerable.Range(0, EntitiesCount).Select(x => EntitiesFactory.CreateMixedEntity(x).ToDocument()).ToArray();
        }

        [Benchmark]
        public int MixedEntityBenchmark()
        {
            var entities = new MixedEntity[_documents.Length];

            for (var i = 0; i < _documents.Length; i++)
                entities[i] = _documents[i].ToObject<MixedEntity>(_metadata);

            return entities.Length;
        }
    }
}