using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Benchmarks.AwsDdbSdk.Entities;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Metadata;

namespace Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class EmitBenchmark
    {
        [Params(10, 100, 1000)]
        public int Iterations;

        private DdbPropertyInfo<string>[] _properties;
        private MediumStringFieldsEntity _entity;

        [GlobalSetup]
        public void Setup()
        {
            var metadata = new DynamoDbContextMetadata(Array.Empty<DdbConverter>());
            _properties = new DdbClassInfo(typeof(MediumStringFieldsEntity), metadata, new ObjectDdbConverter<MediumStringFieldsEntity>(metadata)).Properties.OfType<DdbPropertyInfo<string>>().ToArray();
            _entity = new MediumStringFieldsEntity();
        }

        [Benchmark(Baseline = true)]
        public void ReflectionSetBenchmark()
        {
            for (var i = 0; i < Iterations; i++)
            {
                foreach (var property in _properties)
                    property.PropertyInfo.SetValue(_entity, i.ToString());
            }
        }
        
        [Benchmark()]
        public void EmitSetBenchmark()
        {
            for (var i = 0; i < Iterations; i++)
            {
                foreach (var property in _properties)
                    property.Set(_entity, i.ToString());
            }
        }
    }
}