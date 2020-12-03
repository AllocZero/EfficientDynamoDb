using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using Amazon.Runtime.Internal.Transform;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Benchmarks.AwsDdbSdk.Entities;
using Benchmarks.AwsDdbSdk.Models;
using EfficientDynamoDb;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Operations.Query;
using EfficientDynamoDb.Internal.Reader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AttributeValue = EfficientDynamoDb.DocumentModel.AttributeValues.AttributeValue;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class DeserializationBenchmark : DdbBenchmarkBase
    {
        private const string MediumEntityPk = "medium_des_bench";
        private string _json;
        private MemoryStream _jsonStream;
        private byte[] _jsonBytes;
        private string _queryJson;
        private AttributeValue[] _items;
        private QueryResponseUnmarshaller _unmarshaller;
        
        [GlobalSetup]
        public async Task SetupAsync()
        {
            var entities = await SetupBenchmarkAsync<MediumStringFieldsEntity>(MediumEntityPk).ConfigureAwait(false);
            
            _json = JsonConvert.SerializeObject(entities);
        }

        [GlobalSetup(Target = nameof(UnmarshallerBenchmark) + "," + nameof(NewtonsoftQueryOutputBenchmark) + "," + nameof(TextJsonQueryOutputBenchmark) + "," + nameof(TextReaderQueryOutputBenchmark) + "," + nameof(TextReaderToAttributeValueQueryOutputBenchmark) + "," + nameof(EfficientReaderBenchmark))]
        public async Task SetupUnmarshaller()
        {
            GlobalDynamoDbConfig.InternAttributeNames = true;
            // _queryJson = File.ReadAllText("C:\\Users\\Administrator\\Downloads\\QueryResponse.json");
            _queryJson = File.ReadAllText("C:\\Users\\Administrator\\Downloads\\Mixed2QueryResponse.json");

            _jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(_queryJson));
            _jsonBytes = Encoding.UTF8.GetBytes(_queryJson);
            _unmarshaller = new QueryResponseUnmarshaller();
            // _items = await DdbJsonReader.ReadAsync(new MemoryStream(_jsonBytes)).ConfigureAwait(false);
        }
        
        [GlobalSetup(Target = nameof(EfficientReaderWithoutInternBenchmark))]
        public async Task SetupUnmarshallerWithoutIntern()
        {
            GlobalDynamoDbConfig.InternAttributeNames = false;
            // _queryJson = File.ReadAllText("C:\\Users\\Administrator\\Downloads\\QueryResponse.json");
            _queryJson = File.ReadAllText("C:\\Users\\Administrator\\Downloads\\Mixed2QueryResponse.json");

            _jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(_queryJson));
            _jsonBytes = Encoding.UTF8.GetBytes(_queryJson);
        }
        
        // [Benchmark]
        public int NewtonsoftBenchmark()
        {
            var entities = JsonConvert.DeserializeObject<List<MediumStringFieldsEntity>>(_json);
            return entities.Count;
        }
        
        // [Benchmark]
        public int NewtonsoftQueryOutputBenchmark()
        {
            var entities = JsonConvert.DeserializeObject<QueryModel>(_queryJson);
            return entities.Count;
        }
        
        // [Benchmark]
        public int TextJsonQueryOutputBenchmark()
        {
            var entities = JsonSerializer.Deserialize<QueryModel>(_queryJson);
            return entities.Count;
        }
        
        // [Benchmark]
        public int TextReaderQueryOutputBenchmark()
        {
            var reader = new Utf8JsonReader(_jsonBytes);

            var sum = 0;
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    // TODO: Map directly to Document or final Entity, don't be a bitch
                    case JsonTokenType.PropertyName:
                    case JsonTokenType.String:
                    {
                        var text = reader.GetString();
                        sum += text.Length;
                        break;
                    }
                    case JsonTokenType.Number:
                    {
                        var intValue = reader.GetInt32();
                        sum += intValue;
                        break;
                    }
                }
            }

            return sum;
        }

        [Benchmark]
        public async Task<int> EfficientReaderBenchmark()
        {
            var items = await DdbJsonReader.ReadAsync(new MemoryStream(_jsonBytes), QueryParsingOptions.Instance, false).ConfigureAwait(false);

            return items.Value!.Count;
        }
        
        // [Benchmark]
        public async Task<int> EfficientReaderWithoutInternBenchmark()
        {
            var items = await DdbJsonReader.ReadAsync(new MemoryStream(_jsonBytes), QueryParsingOptions.Instance, false).ConfigureAwait(false);

            return items.Value!.Count;
        }


        // [Benchmark]
        public int TextReaderToAttributeValueQueryOutputBenchmark()
        {
            var items = new List<Dictionary<string, CustomValue>>(1000);
            var reader = new Utf8JsonReader(_jsonBytes);

            var sum = 0;
            var depth = 0;
            string currentKey = null;
            var parsingItems = false;

            Dictionary<string, CustomValue> currentEntity = null;
            string currentFieldKey = null;
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    // TODO: Map directly to Document or final Entity, don't be a bitch
                    case JsonTokenType.PropertyName:
                    {
                        currentKey = reader.GetString();

                        switch (depth)
                        {
                            case 1:
                                parsingItems = currentKey == "Items";
                                break;
                            case 3:
                                currentFieldKey = currentKey;
                                break;
                        }
                        break;
                    }
                    case JsonTokenType.StartObject:
                    case JsonTokenType.StartArray:
                    {
                        depth++;

                        if (parsingItems && depth == 3)
                        {
                            currentEntity = new Dictionary<string, CustomValue>();
                            items.Add(currentEntity);
                        }
                        
                        break;
                    }
                    case JsonTokenType.EndObject:
                    case JsonTokenType.EndArray:
                    {
                        depth--;
                        break;
                    }
                    case JsonTokenType.String when parsingItems && depth == 4:
                    {
                        var value = reader.GetString();
                        sum += value.Length;
                        
                       // currentEntity.Add(currentFieldKey, null);
                        
                        currentEntity.Add(currentFieldKey, new CustomValue(value));
                        
                        // switch (currentKey)
                        // {
                        //     case "N":
                        //         currentEntity.Add(currentFieldKey, new AttributeValue
                        //         {
                        //             N = value
                        //         });
                        //         break;
                        //     case "S":
                        //         currentEntity.Add(currentFieldKey, new AttributeValue
                        //         {
                        //             S = value
                        //         });
                        //         break;
                        // }
                        break;
                    }
                }
            }

            return items.Count + sum;
        }

        public readonly struct CustomValue
        {
            public string S { get; }
            
            public CustomValue(string s)
            {
                S = s;
            }
        }
        
        // [Benchmark(Baseline = true)]
        public int TextJsonBenchmark()
        {
            var entities = JsonSerializer.Deserialize<List<MediumStringFieldsEntity>>(_json);
            
            return entities.Count;
        }
        
        // [Benchmark]
        public int TextJsonDocumentEmptyBenchmark()
        {
            using var document = JsonDocument.Parse(_json);
            
            return document.RootElement.GetArrayLength();
        }
        
        // [Benchmark]
        public int TextJsonDocumentRandomBenchmark()
        {
            using var document = JsonDocument.Parse(_json);

            var length = document.RootElement.GetArrayLength();
            var entities = new MediumStringFieldsEntity[length];

            for (var i = 0; i < length; i++)
            {
                var element = document.RootElement[i];

                entities[i] = new MediumStringFieldsEntity
                {
                    Pk = element.GetProperty("Pk").GetString(),
                    Sk = element.GetProperty("Sk").GetString(),
                    F1 = element.GetProperty("F1").GetString(),
                    F2 = element.GetProperty("F2").GetString(),
                    F3 = element.GetProperty("F3").GetDateTime(),
                    F4 = element.GetProperty("F4").GetDateTime(),
                    F5 = element.GetProperty("F5").GetString(),
                    F6 = element.GetProperty("F6").GetString(),
                    F7 = element.GetProperty("F7").GetDateTime(),
                    F8 = element.GetProperty("F8").GetDateTime(),
                    F9 = element.GetProperty("F9").GetInt32(),
                    F10 = element.GetProperty("F10").GetInt32(),
                    F11 = element.GetProperty("F11").GetInt32(),
                    F12 = element.GetProperty("F12").GetInt32(),
                };
            }

            return entities.Length;
        }
        
        // [Benchmark]
        public int TextJsonDocumentIterationBenchmark()
        {
            using var document = JsonDocument.Parse(_json);

            var sum = 0;

            foreach (var element in document.RootElement.EnumerateArray())
            {
                var entity = new MediumStringFieldsEntity();
                
                foreach (var property in element.EnumerateObject())
                {
                    switch (property.Name)
                    {
                        case nameof(MediumStringFieldsEntity.Pk):
                            entity.Pk = property.Value.GetString();
                            break;
                        case nameof(MediumStringFieldsEntity.Sk):
                            entity.Sk = property.Value.GetString();
                            break;
                        case nameof(MediumStringFieldsEntity.F1):
                            entity.F1 = property.Value.GetString();
                            break;
                        case nameof(MediumStringFieldsEntity.F2):
                            entity.F2 = property.Value.GetString();
                            break;
                        case nameof(MediumStringFieldsEntity.F3):
                            entity.F3 = property.Value.GetDateTime();
                            break;
                        case nameof(MediumStringFieldsEntity.F4):
                            entity.F4 = property.Value.GetDateTime();
                            break;
                        case nameof(MediumStringFieldsEntity.F5):
                            entity.F5 = property.Value.GetString();
                            break;
                        case nameof(MediumStringFieldsEntity.F6):
                            entity.F6 = property.Value.GetString();
                            break;
                        case nameof(MediumStringFieldsEntity.F7):
                            entity.F7 = property.Value.GetDateTime();
                            break;
                        case nameof(MediumStringFieldsEntity.F8):
                            entity.F8 = property.Value.GetDateTime();
                            break;
                        case nameof(MediumStringFieldsEntity.F9):
                            entity.F9 = property.Value.GetInt32();
                            break;
                        case nameof(MediumStringFieldsEntity.F10):
                            entity.F10 = property.Value.GetInt32();
                            break;
                        case nameof(MediumStringFieldsEntity.F11):
                            entity.F11 = property.Value.GetInt32();
                            break;
                        case nameof(MediumStringFieldsEntity.F12):
                            entity.F12 = property.Value.GetInt32();
                            break;
                    }
                }
                
                sum += entity.F9;
            }
            
            return sum;
        }

        private static readonly Dictionary<string, Action<MediumStringFieldsEntity, JsonElement>> PropertiesMap =
            new Dictionary<string, Action<MediumStringFieldsEntity, JsonElement>>
            {
                {nameof(MediumStringFieldsEntity.Pk), (x, p) => x.Pk = p.GetString()},
                {nameof(MediumStringFieldsEntity.Sk), (x, p) => x.Sk = p.GetString()},
                {nameof(MediumStringFieldsEntity.F1), (x, p) => x.F1 = p.GetString()},
                {nameof(MediumStringFieldsEntity.F2), (x, p) => x.F2 = p.GetString()},
                {nameof(MediumStringFieldsEntity.F3), (x, p) => x.F3 = p.GetDateTime()},
                {nameof(MediumStringFieldsEntity.F4), (x, p) => x.F4 = p.GetDateTime()},
                {nameof(MediumStringFieldsEntity.F5), (x, p) => x.F5 = p.GetString()},
                {nameof(MediumStringFieldsEntity.F6), (x, p) => x.F6 = p.GetString()},
                {nameof(MediumStringFieldsEntity.F7), (x, p) => x.F7 = p.GetDateTime()},
                {nameof(MediumStringFieldsEntity.F8), (x, p) => x.F8 = p.GetDateTime()},
                {nameof(MediumStringFieldsEntity.F9), (x, p) => x.F9 = p.GetInt32()},
                {nameof(MediumStringFieldsEntity.F10), (x, p) => x.F10 = p.GetInt32()},
                {nameof(MediumStringFieldsEntity.F11), (x, p) => x.F11 = p.GetInt32()},
                {nameof(MediumStringFieldsEntity.F12), (x, p) => x.F12 = p.GetInt32()},
            };
        
        // [Benchmark]
        public int TextJsonDocumentIterationDictBenchmark()
        {
            using var document = JsonDocument.Parse(_json);

            var sum = 0;

            foreach (var element in document.RootElement.EnumerateArray())
            {
                var entity = new MediumStringFieldsEntity();

                foreach (var property in element.EnumerateObject())
                {
                    PropertiesMap[property.Name](entity, property.Value);
                }

                sum += entity.F9;
            }
            
            return sum;
        }

        // [Benchmark]
        public object UnmarshallerBenchmark()
        {
            return _unmarshaller.Unmarshall(new JsonUnmarshallerContext(new MemoryStream(_jsonBytes), false, null, false));
        }

        protected override async Task<IReadOnlyCollection<object>> QueryAsync<T>(string pk)
        {
            return await DbContext.QueryAsync<T>(pk).GetRemainingAsync().ConfigureAwait(false);
        }
    }
}