using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Benchmarks.AwsDdbSdk.Benchmarks.Deserialization.Models;
using Benchmarks.AwsDdbSdk.Entities;
using Benchmarks.Mocks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;

namespace Benchmarks.AwsDdbSdk.Benchmarks.Deserialization
{
    public class MixedDeserializationBenchmark : DeserializationBenchmarkBase<MixedModel>
    {
        protected override Document CreateEntity(int index) => EntitiesFactory.CreateMixedEntity(index).ToDocument();
    }
}