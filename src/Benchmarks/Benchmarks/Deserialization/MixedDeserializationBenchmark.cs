using Benchmarks.Deserialization.Models;
using Benchmarks.Mocks;
using EfficientDynamoDb.DocumentModel;

namespace Benchmarks.Deserialization
{
    public class MixedDeserializationBenchmark : DeserializationBenchmarkBase<MixedModel>
    {
        protected override Document CreateEntity(int index) => EntitiesFactory.CreateMixedEntity(index).ToDocument();
    }
}