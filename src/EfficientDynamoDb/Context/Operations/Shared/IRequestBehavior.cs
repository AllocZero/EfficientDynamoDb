using System;
using System.Collections.Generic;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.Shared
{
    internal interface IIndexName
    {
        public string? IndexName { set; }
    }

    internal interface IKeyExpression
    {
        public FilterBase KeyExpression { set; }
    }

    internal interface IConsistentRead
    {
        public bool ConsistentRead { set; }
    }
    
    internal interface ILimit
    {
        public int? Limit { set; }
    }

    internal interface IProjectionExpression
    {
        public IReadOnlyList<string> ProjectionExpression { set; }
    }

    internal interface IReturnConsumedCapacity
    {
        public ReturnConsumedCapacity ReturnConsumedCapacity { set; }
    }
    
    internal interface ISelect
    {
        public Select? Select { set; }
    }
    
    internal interface IScanIndexForward
    {
        public bool ScanIndexForward { set; }
    }

    internal interface IFilterExpression
    {
        public FilterBase FilterExpression { set; }
    }

    internal interface IItemType
    {
        public Type? ItemType { set; }
    }

    internal interface IItem
    {
        public object? Item { set; }
    }

    internal interface IReturnValues
    {
        public ReturnValues ReturnValues { set; }
    }

    internal interface IReturnItemCollectionMetrics
    {
        public ReturnItemCollectionMetrics ReturnItemCollectionMetrics { set; }
    }

    internal interface IUpdateCondition
    {
        public FilterBase UpdateCondition { set; }
    }

    internal interface IPaginationToken
    {
        public string? PaginationToken { set; }
    }
}