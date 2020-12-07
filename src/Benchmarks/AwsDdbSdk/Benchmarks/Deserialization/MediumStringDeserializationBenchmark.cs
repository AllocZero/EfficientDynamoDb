using System;
using System.Text.Json;
using Benchmarks.AwsDdbSdk.Benchmarks.Deserialization.Models;
using Benchmarks.AwsDdbSdk.Entities;
using Benchmarks.Mocks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;

namespace Benchmarks.AwsDdbSdk.Benchmarks.Deserialization
{
    public class MediumStringDeserializationBenchmark : DeserializationBenchmarkBase<MediumStringModel>
    {
        protected override Document CreateEntity(int index) => EntitiesFactory.CreateMediumStringEntity(index).ToDocument();
    }
}