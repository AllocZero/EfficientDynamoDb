using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.UpdateItem
{
    internal sealed class UpdateItemSaveHttpContent<TEntity> : DynamoDbHttpContent where TEntity : class
    {
        private static readonly Dictionary<Type, IVersionWriter> VersionWriters = new Dictionary<Type, IVersionWriter>
        {
            {typeof(byte?), new ByteVersionWriter()},
            {typeof(short?), new ShortVersionWriter()},
            {typeof(int?), new IntVersionWriter()},
            {typeof(long?), new LongVersionWriter()},
        };

        private readonly DynamoDbContextConfig _config;
        private readonly TEntity _entity;
        private readonly DdbClassInfo _classInfo;

        public bool HasVersion => _classInfo.Version != null;

        public UpdateItemSaveHttpContent(DynamoDbContextConfig config, TEntity entity)
            : base("DynamoDB_20120810.UpdateItem")
        {
            _config = config;
            _entity = entity;
            _classInfo = _config.Metadata.GetOrAddClassInfo<TEntity>();
        }

        public void SetIncrementedVersion() => VersionWriters[_classInfo.Version!.PropertyInfo.PropertyType].SetIncrementedValue(_entity, _classInfo.Version);

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            ddbWriter.JsonWriter.WriteStartObject();
            
            ddbWriter.JsonWriter.WriteTableName(_config.TableNamePrefix, _classInfo.TableName!);
            
            ddbWriter.JsonWriter.WritePropertyName("Key");
            ddbWriter.JsonWriter.WriteStartObject();
            _classInfo.PartitionKey!.Write(_entity, in ddbWriter);
            _classInfo.SortKey?.Write(_entity, in ddbWriter);
            ddbWriter.JsonWriter.WriteEndObject();

            WriteUpdateItem(in ddbWriter, _classInfo);

            ddbWriter.JsonWriter.WriteEndObject();

            return new ValueTask();
        }

        private void WriteUpdateItem(in DdbWriter ddbWriter, DdbClassInfo classInfo)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            try
            {
                if (classInfo.Version != null)
                    WriteConditionExpression(ddbWriter, classInfo, builder);

                var hasRemove = false;
                var firstSet = true;

                // Write SET expression
                for (var i = 0; i < classInfo.Properties.Length; i++)
                {
                    var property = classInfo.Properties[i];
                    if (property.AttributeType != DynamoDbAttributeType.Regular)
                        continue;
                    
                    if (!property.ShouldWrite(_entity) && property != classInfo.Version)
                    {
                        hasRemove = true;
                        continue;
                    }

                    if (firstSet)
                        builder.Append("SET ");
                    else
                        builder.Append(',');

                    builder.Append("#f");
                    builder.Append(i);
                    builder.Append(" = ");
                    builder.Append(":v");
                    builder.Append(i);

                    firstSet = false;
                }
                
                // Write Remove expression
                if (hasRemove)
                {
                    if(!firstSet) 
                        builder.Append(' ');
                        
                    builder.Append("REMOVE ");

                    var firstRemove = true;
                    for (var i = 0; i < classInfo.Properties.Length; i++)
                    {
                        var property = classInfo.Properties[i];
                        if (property.AttributeType != DynamoDbAttributeType.Regular || property == classInfo.Version)
                            continue;
                        
                        if (property.ShouldWrite(_entity))
                            continue;
                        
                        if(!firstRemove)
                            builder.Append(',');
                        
                        builder.Append("#f");
                        builder.Append(i);

                        firstRemove = false;
                    }
                }
                
                ddbWriter.JsonWriter.WriteString("UpdateExpression", builder.GetBuffer());
                builder.Clear();

                WriteExpressionAttributeNames(in ddbWriter, ref builder, classInfo);
                WriteExpressionAttributeValues(in ddbWriter, ref builder, classInfo);
            }
            finally
            {
                builder.Dispose();
            }
        }

        private void WriteConditionExpression(DdbWriter ddbWriter, DdbClassInfo classInfo, NoAllocStringBuilder builder)
        {
            if (classInfo.Version!.IsNull(_entity))
            {
                builder.Append("attribute_not_exists(");
                builder.Append("#f");
                builder.Append(classInfo.Properties.Length);
                builder.Append(')');
            }
            else
            {
                builder.Append("#f");
                builder.Append(classInfo.Properties.Length);
                builder.Append(" = ");
                builder.Append(":v");
                builder.Append(classInfo.Properties.Length);
            }

            ddbWriter.JsonWriter.WriteString("ConditionExpression", builder.GetBuffer());
            builder.Clear();
        }

        private static void WriteExpressionAttributeNames(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, DdbClassInfo classInfo)
        {
            ddbWriter.JsonWriter.WritePropertyName("ExpressionAttributeNames");

            ddbWriter.JsonWriter.WriteStartObject();
            for (var i = 0; i < classInfo.Properties.Length; i++)
            {
                var property = classInfo.Properties[i];
                if (property.AttributeType != DynamoDbAttributeType.Regular)
                    continue;
                
                builder.Append("#f");
                builder.Append(i);

                ddbWriter.JsonWriter.WriteString(builder.GetBuffer(), property.AttributeName);

                builder.Clear();
            }

            if (classInfo.Version != null)
            {
                builder.Append("#f");
                builder.Append(classInfo.Properties.Length);
                
                ddbWriter.JsonWriter.WriteString(builder.GetBuffer(), classInfo.Version.AttributeName);
                builder.Clear();
            }
            
            ddbWriter.JsonWriter.WriteEndObject();
        }

        private void WriteExpressionAttributeValues(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, DdbClassInfo classInfo)
        {
            ddbWriter.JsonWriter.WritePropertyName("ExpressionAttributeValues");

            ddbWriter.JsonWriter.WriteStartObject();
            for (var i = 0; i < classInfo.Properties.Length; i++)
            {
                var property = classInfo.Properties[i];
                if (property.AttributeType != DynamoDbAttributeType.Regular)
                    continue;

                if (property == classInfo.Version)
                {
                    WriteIncrementedVersionValue(in ddbWriter, ref builder, classInfo.Version, i);
                    continue;
                }
                
                if (!property.ShouldWrite(_entity))
                    continue;
                
                builder.Append(":v");
                builder.Append(i);

                ddbWriter.JsonWriter.WritePropertyName(builder.GetBuffer());
                property.WriteValue(_entity, in ddbWriter);
                builder.Clear();
            }
            
            if (classInfo.Version != null && !classInfo.Version.IsNull(_entity))
            {
                // Write expected value
                builder.Append(":v");
                builder.Append(classInfo.Properties.Length);
                
                ddbWriter.JsonWriter.WritePropertyName(builder.GetBuffer());
                classInfo.Version.WriteValue(_entity, in ddbWriter);
                builder.Clear();
            }
            
            ddbWriter.JsonWriter.WriteEndObject();
        }

        private void WriteIncrementedVersionValue(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, DdbPropertyInfo version, int index)
        {
            builder.Append(":v");
            builder.Append(index);

            ddbWriter.JsonWriter.WritePropertyName(builder.GetBuffer());
            if (!VersionWriters.TryGetValue(version.PropertyInfo.PropertyType, out var writer))
                throw new DdbException($"Unsupported version property type '{version.PropertyInfo.PropertyType.Name}'.");

            writer.WriteIncrementedValue(in ddbWriter, _entity, version);
            builder.Clear();
        }

        private interface IVersionWriter
        {
            void WriteIncrementedValue(in DdbWriter ddbWriter, TEntity entity, DdbPropertyInfo version);

            void SetIncrementedValue(TEntity entity, DdbPropertyInfo versionProperty);
        }

        private class ByteVersionWriter : IVersionWriter
        {
            public void WriteIncrementedValue(in DdbWriter ddbWriter, TEntity entity, DdbPropertyInfo version)
            {
                var property = (DdbPropertyInfo<byte?>) version;
                var currentValue = property.Get(entity);

                if (currentValue is null)
                {
                    byte? newValue = 0;
                    property.Converter.Write(in ddbWriter, ref newValue);
                }
                else
                {
                    byte? newValue = (byte) (currentValue.Value + 1);
                    property.Converter.Write(in ddbWriter, ref newValue);
                }
            }

            public void SetIncrementedValue(TEntity entity, DdbPropertyInfo versionProperty)
            {
                var property = (DdbPropertyInfo<byte?>) versionProperty;
                var currentValue = property.Get(entity);

                byte? newValue = currentValue is null ? (byte) 0 : (byte) (currentValue.Value + 1);
                property.Set!.Invoke(entity, newValue);
            }
        }
        
        private class ShortVersionWriter : IVersionWriter
        {
            public void WriteIncrementedValue(in DdbWriter ddbWriter, TEntity entity, DdbPropertyInfo version)
            {
                var property = (DdbPropertyInfo<short?>) version;
                var currentValue = property.Get(entity);

                if (currentValue is null)
                {
                    short? newValue = 0;
                    property.Converter.Write(in ddbWriter, ref newValue);
                }
                else
                {
                    short? newValue = (short) (currentValue.Value + 1);
                    property.Converter.Write(in ddbWriter, ref newValue);
                }
            }
            
            public void SetIncrementedValue(TEntity entity, DdbPropertyInfo versionProperty)
            {
                var property = (DdbPropertyInfo<short?>) versionProperty;
                var currentValue = property.Get(entity);

                short? newValue = currentValue is null ? (short) 0 : (short) (currentValue.Value + 1);
                property.Set!.Invoke(entity, newValue);
            }
        }
        
        private class IntVersionWriter : IVersionWriter
        {
            public void WriteIncrementedValue(in DdbWriter ddbWriter, TEntity entity, DdbPropertyInfo version)
            {
                var property = (DdbPropertyInfo<int?>) version;
                var currentValue = property.Get(entity);

                if (currentValue is null)
                {
                    int? newValue = 0;
                    property.Converter.Write(in ddbWriter, ref newValue);
                }
                else
                {
                    int? newValue = currentValue.Value + 1;
                    property.Converter.Write(in ddbWriter, ref newValue);
                }
            }
            
            public void SetIncrementedValue(TEntity entity, DdbPropertyInfo versionProperty)
            {
                var property = (DdbPropertyInfo<int?>) versionProperty;
                var currentValue = property.Get(entity);

                int? newValue = currentValue + 1 ?? 0;
                property.Set!.Invoke(entity, newValue);
            }
        }
        
        private class LongVersionWriter : IVersionWriter
        {
            public void WriteIncrementedValue(in DdbWriter ddbWriter, TEntity entity, DdbPropertyInfo version)
            {
                var property = (DdbPropertyInfo<long?>) version;
                var currentValue = property.Get(entity);

                if (currentValue is null)
                {
                    long? newValue = 0;
                    property.Converter.Write(in ddbWriter, ref newValue);
                }
                else
                {
                    long? newValue = currentValue.Value + 1;
                    property.Converter.Write(in ddbWriter, ref newValue);
                }
            }
            
            public void SetIncrementedValue(TEntity entity, DdbPropertyInfo versionProperty)
            {
                var property = (DdbPropertyInfo<long?>) versionProperty;
                var currentValue = property.Get(entity);

                long? newValue = currentValue + 1 ?? 0;
                property.Set!.Invoke(entity, newValue);
            }
        }
    }
}