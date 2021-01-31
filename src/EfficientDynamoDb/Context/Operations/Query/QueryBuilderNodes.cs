using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.FluentCondition;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Context.Operations.Query
{
    internal enum BuilderNodeType
    {
        Primitive,
        KeyExpression,
        FilterExpression,
        Item,
        UpdateCondition,
        UpdateAttribute
    }
    internal abstract class BuilderNode
    {
        public BuilderNode? Next { get; }

        public virtual BuilderNodeType Type => BuilderNodeType.Primitive;

        protected BuilderNode(BuilderNode? next) => Next = next;

        public abstract void WriteValue(in DdbWriter writer);
    }

    internal abstract class BuilderNode<TValue> : BuilderNode
    {
        public TValue Value { get; }

        protected BuilderNode(TValue value, BuilderNode? next) : base(next) => Value = value;
    }

    internal sealed class IndexNameNode : BuilderNode<string>
    {
        public override void WriteValue(in DdbWriter writer) => writer.JsonWriter.WriteString("IndexName", Value);

        public IndexNameNode(string value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class KeyExpressionNode : BuilderNode<FilterBase>
    {
        public override BuilderNodeType Type => BuilderNodeType.KeyExpression;

        public override void WriteValue(in DdbWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public KeyExpressionNode(FilterBase value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class ConsistentReadNode : BuilderNode<bool>
    {
        public override void WriteValue(in DdbWriter writer) => writer.JsonWriter.WriteBoolean("ConsistentRead", Value);

        public ConsistentReadNode(bool value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class LimitNode : BuilderNode<int> 
    {
        public override void WriteValue(in DdbWriter writer) => writer.JsonWriter.WriteNumber("Limit", Value);

        public LimitNode(int value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class ProjectedAttributesNode : BuilderNode<IReadOnlyList<string>>
    {
        public override void WriteValue(in DdbWriter writer) => writer.JsonWriter.WriteString("ProjectionExpression", string.Join(",", Value));

        public ProjectedAttributesNode(IReadOnlyList<string> value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class ReturnConsumedCapacityNode : BuilderNode<ReturnConsumedCapacity>
    {
        public override void WriteValue(in DdbWriter writer)
        {
            if (Value != ReturnConsumedCapacity.None)
                writer.JsonWriter.WriteReturnConsumedCapacity(Value);
        }

        public ReturnConsumedCapacityNode(ReturnConsumedCapacity value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class SelectNode : BuilderNode<Select> 
    {
        public override void WriteValue(in DdbWriter writer)
        {
            var selectValue = Value switch
            {
                Select.AllAttributes => "ALL_ATTRIBUTES",
                Select.AllProjectedAttributes => "ALL_PROJECTED_ATTRIBUTES",
                Select.Count => "COUNT",
                Select.SpecificAttributes => "SPECIFIC_ATTRIBUTES",
                _ => "ALL_ATTRIBUTES"
            };
            
            writer.JsonWriter.WriteString("Select", selectValue);
        }

        public SelectNode(Select value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class BackwardSearchNode : BuilderNode<bool> 
    {
        public override void WriteValue(in DdbWriter writer)
        {
            if (Value)
                writer.JsonWriter.WriteBoolean("ScanIndexForward", false);
        }

        public BackwardSearchNode(bool value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class FilterExpressionNode : BuilderNode<FilterBase>
    {
        public override BuilderNodeType Type => BuilderNodeType.FilterExpression;
        
        public override void WriteValue(in DdbWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public FilterExpressionNode(FilterBase value, BuilderNode? next) : base(value, next)
        {
        }
    }
    
    internal sealed class ItemNode : BuilderNode<object>
    {
        public override BuilderNodeType Type => BuilderNodeType.Item;
        
        public Type ItemType { get; }
        
        public override void WriteValue(in DdbWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public ItemNode(object value, Type itemType, BuilderNode? next) : base(value, next)
        {
            ItemType = itemType;
        }
    }
    
    internal sealed class ReturnValuesNode : BuilderNode<ReturnValues>
    {
        public override void WriteValue(in DdbWriter writer)
        {
            if (Value != ReturnValues.None)
                writer.JsonWriter.WriteReturnValues(Value);
        }

        public ReturnValuesNode(ReturnValues value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class ReturnItemCollectionMetricsNode : BuilderNode<ReturnItemCollectionMetrics> 
    {
        public override void WriteValue(in DdbWriter writer)
        {
            if (Value != ReturnItemCollectionMetrics.None)
                writer.JsonWriter.WriteReturnItemCollectionMetrics(Value);
        }

        public ReturnItemCollectionMetricsNode(ReturnItemCollectionMetrics value, BuilderNode? next) : base(value, next)
        {
        }
    }
    
    internal sealed class UpdateConditionNode : BuilderNode<FilterBase>
    {
        public override BuilderNodeType Type => BuilderNodeType.UpdateCondition;
        
        public override void WriteValue(in DdbWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public UpdateConditionNode(FilterBase value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class PaginationTokenNode : BuilderNode<string?>
    {
        public override void WriteValue(in DdbWriter writer)
        {
            if(Value != null)
                writer.WritePaginationToken(Value);
        }

        public PaginationTokenNode(string? value, BuilderNode? next) : base(value, next)
        {
        }
    }

    internal sealed class UpdateAttributeNode : BuilderNode<UpdateBase>
    {
        public override BuilderNodeType Type => BuilderNodeType.UpdateAttribute;

        public UpdateAttributeNode(UpdateBase value, BuilderNode? next) : base(value, next)
        {
        }

        public override void WriteValue(in DdbWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}