using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Benchmarks.AwsDdbSdk.Benchmarks.Deserialization.Models;
using Benchmarks.AwsDdbSdk.Entities;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Extensions;

namespace Benchmarks.AwsDdbSdk.Benchmarks.Deserialization
{
    public class MixedDeserializationBenchmark : DeserializationBenchmarkBase<MixedModel>
    {
        protected override void WriteEntity(Utf8JsonWriter writer, int index)
        {
            var entity = new MixedEntity
            {
                Pk = $"pk_{index:0000}",
                Sk = $"sk_{index:0000}",
                B = index % 2 == 0,
                N = index,
                S = $"test_{index:0000}",
                Ns = new HashSet<int> {index},
                Ss = new HashSet<string> {$"test_set_{index:0000}"},
                M = new MapObject {P1 = $"test_p0_{index:0000}"},
                L1 = new List<MapObject> {new MapObject {P1 = $"test_p1_{index:0000}"}},
                L2 = new List<MapObject> {new MapObject {P1 = $"test_p2_{index:0000}"}},
                L3 = new List<MapObject> {new MapObject {P1 = $"test_p3_{index:0000}"}}
            };

            writer.WriteAttributesDictionary(new Document
            {
                {"pk", entity.Pk},
                {"sk", entity.Sk},
                {"b", entity.B},
                {"n", entity.N},
                {"s", entity.S},
                {"ns", new NumberSetAttributeValue(entity.Ns.Select(x => x.ToString()).ToArray())},
                {"ss", new StringSetAttributeValue(entity.Ss)},
                {
                    "m", new AttributeValue(new MapAttributeValue(new Document
                    {
                        {"p1", entity.M.P1}
                    }))
                },
                {
                    "l1", new ListAttributeValue(entity.L1.Select(x => new AttributeValue(new MapAttributeValue(new Document
                    {
                        {"p1", x.P1}
                    }))).ToArray())
                },
                {
                    "l2", new ListAttributeValue(entity.L2.Select(x => new AttributeValue(new MapAttributeValue(new Document
                    {
                        {"p1", x.P1}
                    }))).ToArray())
                },
                {
                    "l3", new ListAttributeValue(entity.L3.Select(x => new AttributeValue(new MapAttributeValue(new Document
                    {
                        {"p1", x.P1}
                    }))).ToArray())
                },
            });
        }
    }
}