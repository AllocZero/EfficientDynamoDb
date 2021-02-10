using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Context.Operations.Query
{
    internal enum BuilderNodeType : byte
    {
        Primitive,
        KeyExpression,
        FilterExpression,
        Item,
        UpdateCondition,
        AddUpdate,
        SetUpdate,
        RemoveUpdate,
        DeleteUpdate,
        PrimaryKey,
        Projection
    }

    internal static class NodeBits
    {
        public const int IndexName = 1 << 0;
        public const int ConsistentRead = 1 << 1;
        public const int Limit = 1 << 2;
        public const int ProjectedAttributes = 1 << 3;
        public const int ReturnConsumedCapacity = 1 << 4;
        public const int Select = 1 << 5;
        public const int BackwardSearch = 1 << 6;
        public const int ReturnValues = 1 << 7;
        public const int ReturnItemCollectionMetrics = 1 << 8;
        public const int PaginationToken = 1 << 9;
        public const int PrimaryKey = 1 << 10;
        public const int Item = 1 << 11;
        public const int UpdateCondition = 1 << 12;
    }
    
    internal abstract class BuilderNode
    {
        public BuilderNode? Next { get; }

        public virtual BuilderNodeType Type => BuilderNodeType.Primitive;

        protected BuilderNode(BuilderNode? next) => Next = next;

        public abstract void WriteValue(in DdbWriter writer, ref int state);
    }

    internal abstract class BuilderNode<TValue> : BuilderNode
    {
        public TValue Value { get; }

        protected BuilderNode(TValue value, BuilderNode? next) : base(next) => Value = value;
    }

    internal sealed class IndexNameNode : BuilderNode<string>
    {
        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            if (state.IsBitSet(NodeBits.IndexName))
                return;
            
            writer.JsonWriter.WriteString("IndexName", Value);

            state = state.SetBit(NodeBits.IndexName);
        }

        public IndexNameNode(string value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class KeyExpressionNode : BuilderNode<FilterBase>
    {
        public override BuilderNodeType Type => BuilderNodeType.KeyExpression;

        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            throw new System.NotImplementedException();
        }

        public KeyExpressionNode(FilterBase value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class ConsistentReadNode : BuilderNode<bool>
    {
        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            if (state.IsBitSet(NodeBits.ConsistentRead))
                return;
            
            writer.JsonWriter.WriteBoolean("ConsistentRead", Value);

            state = state.SetBit(NodeBits.ConsistentRead);
        }

        public ConsistentReadNode(bool value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class LimitNode : BuilderNode<int> 
    {
        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            if (state.IsBitSet(NodeBits.Limit))
                return;
            
            writer.JsonWriter.WriteNumber("Limit", Value);
            
            state = state.SetBit(NodeBits.Limit);
        }

        public LimitNode(int value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class ProjectedAttributesNode : BuilderNode<DdbClassInfo>
    {
        public override BuilderNodeType Type => BuilderNodeType.Projection;

        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            throw new NotImplementedException();
        }

        public ProjectedAttributesNode(DdbClassInfo projectionClassInfo, BuilderNode? next) : base(projectionClassInfo, next)
        {
        }
    }

    internal sealed class ReturnConsumedCapacityNode : BuilderNode<ReturnConsumedCapacity>
    {
        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            if (state.IsBitSet(NodeBits.ReturnConsumedCapacity))
                return;
            
            if (Value != ReturnConsumedCapacity.None)
                writer.JsonWriter.WriteReturnConsumedCapacity(Value);
            
            state = state.SetBit(NodeBits.ReturnConsumedCapacity);
        }

        public ReturnConsumedCapacityNode(ReturnConsumedCapacity value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class SelectNode : BuilderNode<Select> 
    {
        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            if (state.IsBitSet(NodeBits.Select))
                return;
            
            var selectValue = Value switch
            {
                Select.AllAttributes => "ALL_ATTRIBUTES",
                Select.AllProjectedAttributes => "ALL_PROJECTED_ATTRIBUTES",
                Select.Count => "COUNT",
                Select.SpecificAttributes => "SPECIFIC_ATTRIBUTES",
                _ => "ALL_ATTRIBUTES"
            };
            
            writer.JsonWriter.WriteString("Select", selectValue);
            
            state = state.SetBit(NodeBits.Select);
        }

        public SelectNode(Select value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class BackwardSearchNode : BuilderNode<bool> 
    {
        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            if (state.IsBitSet(NodeBits.BackwardSearch))
                return;
            
            if (Value)
                writer.JsonWriter.WriteBoolean("ScanIndexForward", false);
            
            state = state.SetBit(NodeBits.BackwardSearch);
        }

        public BackwardSearchNode(bool value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class FilterExpressionNode : BuilderNode<FilterBase>
    {
        public override BuilderNodeType Type => BuilderNodeType.FilterExpression;
        
        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            throw new System.NotImplementedException();
        }

        public FilterExpressionNode(FilterBase value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal abstract class EntityNodeBase : BuilderNode
    {
        public DdbClassInfo EntityClassInfo { get; }

        protected EntityNodeBase(DdbClassInfo entityClassInfo, BuilderNode? next) : base(next)
        {
            EntityClassInfo = entityClassInfo;
        }
    }
    
    internal sealed class ItemNode : EntityNodeBase
    {
        public override BuilderNodeType Type => BuilderNodeType.Item;

        public object Value { get; }
        
        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            throw new System.NotImplementedException();
        }

        public ItemNode(object value, DdbClassInfo entityClassInfo, BuilderNode? next) : base(entityClassInfo, next)
        {
            Value = value;
        }
    }
    
    internal sealed class ReturnValuesNode : BuilderNode<ReturnValues>
    {
        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            if (state.IsBitSet(NodeBits.ReturnValues))
                return;
            
            if (Value != ReturnValues.None)
                writer.JsonWriter.WriteReturnValues(Value);

            state = state.SetBit(NodeBits.ReturnValues);
        }

        public ReturnValuesNode(ReturnValues value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class ReturnItemCollectionMetricsNode : BuilderNode<ReturnItemCollectionMetrics> 
    {
        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            if (state.IsBitSet(NodeBits.ReturnItemCollectionMetrics))
                return;
            
            if (Value != ReturnItemCollectionMetrics.None)
                writer.JsonWriter.WriteReturnItemCollectionMetrics(Value);
            
            state = state.SetBit(NodeBits.ReturnItemCollectionMetrics);
        }

        public ReturnItemCollectionMetricsNode(ReturnItemCollectionMetrics value, BuilderNode? next) : base(value, next)
        {
        }
    }
    
    internal sealed class UpdateConditionNode : BuilderNode<FilterBase>
    {
        public override BuilderNodeType Type => BuilderNodeType.UpdateCondition;
        
        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            throw new System.NotImplementedException();
        }

        public UpdateConditionNode(FilterBase value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class PaginationTokenNode : BuilderNode<string?>
    {
        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            if (state.IsBitSet(NodeBits.PaginationToken))
                return;
            
            if(Value != null)
                writer.WritePaginationToken(Value);
            
            state = state.SetBit(NodeBits.PaginationToken);
        }

        public PaginationTokenNode(string? value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class UpdateAttributeNode : BuilderNode<UpdateBase>
    {
        public override BuilderNodeType Type { get; }

        public UpdateAttributeNode(UpdateBase value, BuilderNodeType type, BuilderNode? next) : base(value, next)
        {
            Type = type;
        }

        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            throw new NotImplementedException();
        }
    }
    
    internal abstract class PrimaryKeyNodeBase : BuilderNode
    {
        public abstract void Write(in DdbWriter writer, DdbClassInfo classInfo, ref int state);
        
        public override BuilderNodeType Type => BuilderNodeType.PrimaryKey;

        protected PrimaryKeyNodeBase(BuilderNode? next) : base(next)
        {
        }
    }

    internal sealed class PartitionAndSortKeyNode<TPk, TSk> : PrimaryKeyNodeBase
    {
        private TPk _pk;
        private TSk _sk;

        public PartitionAndSortKeyNode(TPk pk, TSk sk, BuilderNode? next) : base(next)
        {
            _pk = pk;
            _sk = sk;
        }

        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            throw new NotImplementedException();
        }

        public override void Write(in DdbWriter writer, DdbClassInfo classInfo, ref int state)
        {
            if (state.IsBitSet(NodeBits.PrimaryKey))
                return;
            
            writer.JsonWriter.WritePropertyName("Key");
            writer.JsonWriter.WriteStartObject();

            var pkAttribute = (DdbPropertyInfo<TPk>) classInfo.PartitionKey!;
            pkAttribute.Converter.Write(in writer, pkAttribute.AttributeName, ref _pk);
            
            var skAttribute = (DdbPropertyInfo<TSk>)classInfo.SortKey!;
            skAttribute.Converter.Write(in writer, skAttribute.AttributeName, ref _sk);
            
            writer.JsonWriter.WriteEndObject();

            state = state.SetBit(NodeBits.PrimaryKey);
        }
    }

    internal sealed class PartitionKeyNode<TPk> : PrimaryKeyNodeBase
    {
        private TPk _pk;

        public PartitionKeyNode(TPk pk, BuilderNode? next) : base(next)
        {
            _pk = pk;
        }

        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            throw new NotImplementedException();
        }

        public override void Write(in DdbWriter writer, DdbClassInfo classInfo, ref int state)
        {
            if (state.IsBitSet(NodeBits.PrimaryKey))
                return;
            
            writer.JsonWriter.WritePropertyName("Key");
            writer.JsonWriter.WriteStartObject();

            var pkAttribute = (DdbPropertyInfo<TPk>) classInfo.PartitionKey!;
            pkAttribute.Converter.Write(in writer, pkAttribute.AttributeName, ref _pk);
            
            writer.JsonWriter.WriteEndObject();
            
            state = state.SetBit(NodeBits.PrimaryKey);
        }
    }
    
    internal abstract class DeletePrimaryKeyNodeBase : EntityNodeBase
    {
        public override BuilderNodeType Type => BuilderNodeType.PrimaryKey;

        protected DeletePrimaryKeyNodeBase(DdbClassInfo entityClassInfo, BuilderNode? next) : base(entityClassInfo, next)
        {
        }
    }
    
    internal sealed class DeletePartitionAndSortKeyNode<TPk, TSk> : DeletePrimaryKeyNodeBase
    {
        private TPk _pk;
        private TSk _sk;

        public DeletePartitionAndSortKeyNode(DdbClassInfo entityClassInfo, TPk pk, TSk sk, BuilderNode? next) : base(entityClassInfo, next)
        {
            _pk = pk;
            _sk = sk;
        }

        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            writer.JsonWriter.WritePropertyName("Key");
            writer.JsonWriter.WriteStartObject();

            var pkAttribute = (DdbPropertyInfo<TPk>) EntityClassInfo.PartitionKey!;
            pkAttribute.Converter.Write(in writer, pkAttribute.AttributeName, ref _pk);
            
            var skAttribute = (DdbPropertyInfo<TSk>)EntityClassInfo.SortKey!;
            skAttribute.Converter.Write(in writer, skAttribute.AttributeName, ref _sk);
            
            writer.JsonWriter.WriteEndObject();
        }
    }

    internal sealed class DeletePartitionKeyNode<TPk> : DeletePrimaryKeyNodeBase
    {
        private TPk _pk;
        
        public DeletePartitionKeyNode(DdbClassInfo entityClassInfo, TPk pk, BuilderNode? next) : base(entityClassInfo, next)
        {
            _pk = pk;
        }

        public override void WriteValue(in DdbWriter writer, ref int state)
        {
            writer.JsonWriter.WritePropertyName("Key");
            writer.JsonWriter.WriteStartObject();

            var pkAttribute = (DdbPropertyInfo<TPk>) EntityClassInfo.PartitionKey!;
            pkAttribute.Converter.Write(in writer, pkAttribute.AttributeName, ref _pk);
            
            writer.JsonWriter.WriteEndObject();
        }
    }
}