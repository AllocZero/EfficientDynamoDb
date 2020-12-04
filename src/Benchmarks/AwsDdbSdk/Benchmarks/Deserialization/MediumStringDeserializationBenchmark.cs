using System;
using System.Text.Json;
using Benchmarks.AwsDdbSdk.Benchmarks.Deserialization.Models;
using Benchmarks.AwsDdbSdk.Entities;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;

namespace Benchmarks.AwsDdbSdk.Benchmarks.Deserialization
{
    public class MediumStringDeserializationBenchmark : DeserializationBenchmarkBase<MediumStringModel>
    {
        protected override void WriteEntity(Utf8JsonWriter writer, int index)
        {
            var entity =  new MediumStringFieldsEntity
            {
                Pk = $"pk_{index:0000}",
                Sk = $"sk_{index:0000}",
                F1 = $"test_f1_{index:0000}",
                F2 = $"test_f2_{index:0000}",
                F3 = new DateTime(2020, 12, 01, 8, 15, 0).AddSeconds(index),
                F4 = new DateTime(2020, 12, 20, 20, 20, 0).AddSeconds(index),
                F5 = $"test_f1_{index:0000}",
                F6 = $"test_f1_{index:0000}",
                F7 = new DateTime(2020, 12, 5, 19, 15, 0).AddSeconds(index),
                F8 = new DateTime(2020, 12, 3, 15, 15, 0).AddSeconds(index),
                F9 = index,
                F10 = 1000 - index,
                F11 = 1000000,
                F12 = 1
            };

            writer.WriteAttributesDictionary(new Document
            {
                {"pk", entity.Pk},
                {"sk", entity.Sk},
                {"f1", entity.F1},
                {"f2", entity.F2},
                {"f3", entity.F3.ToString("O")},
                {"f4", entity.F4.ToString("O")},
                {"f5", entity.F5},
                {"f6", entity.F6},
                {"f7", entity.F7.ToString("O")},
                {"f8", entity.F8.ToString("O")},
                {"f9", entity.F9},
                {"f10", entity.F10},
                {"f11", entity.F11},
                {"f12", entity.F12}
            });
        }
    }
}