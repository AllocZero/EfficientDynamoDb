using System;
using System.Text.Json;
using EfficientDynamoDb.Internal.JsonConverters;
using EfficientDynamoDb.Operations.DescribeTable.Models.Enums;
using NUnit.Framework;

namespace EfficientDynamoDb.Tests.Internal.JsonConverters
{
    [TestFixture]
    public class DdbEnumJsonConverter
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
            { Converters = { new DdbEnumJsonConverterFactory() } };

        [TestCase(KeyType.Hash, "HASH")]
        [TestCase(SseType.Kms, "KMS")]
        [TestCase(SseType.Aes256, "AES256")]
        [TestCase(StreamViewType.NewAndOldImages, "NEW_AND_OLD_IMAGES")]
        [TestCase(StreamViewType.KeysOnly, "KEYS_ONLY")]
        public void EnumDeserializationTest<T>(T enumValue, string jsonValue) where T : struct, Enum
        {
            var json = $"\"{jsonValue}\"";

            var result = JsonSerializer.Deserialize<T>(json, _options);

            Assert.That(result, Is.EqualTo(enumValue));
        }
        
        [TestCase(KeyType.Hash, "HASH")]
        [TestCase(SseType.Kms, "KMS")]
        [TestCase(SseType.Aes256, "AES256")]
        [TestCase(StreamViewType.NewAndOldImages, "NEW_AND_OLD_IMAGES")]
        [TestCase(StreamViewType.KeysOnly, "KEYS_ONLY")]
        public void EnumSerializationTest<T>(T enumValue, string expectedJsonValue) where T : struct, Enum
        {
            var result = JsonSerializer.Serialize(enumValue, _options);
            var expectedJson = $"\"{expectedJsonValue}\"";
            
            Assert.That(result, Is.EqualTo(expectedJson));
        }
    }
}