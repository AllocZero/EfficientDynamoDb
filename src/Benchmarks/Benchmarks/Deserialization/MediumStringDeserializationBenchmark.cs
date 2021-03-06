using Benchmarks.Deserialization.Models;
using Benchmarks.Mocks;
using EfficientDynamoDb.DocumentModel;

namespace Benchmarks.Deserialization
{
    public class MediumStringDeserializationBenchmark : DeserializationBenchmarkBase<MediumStringModel>
    {
        protected override Document CreateEntity(int index) => EntitiesFactory.CreateMediumStringEntity(index).ToDocument();
    }
}