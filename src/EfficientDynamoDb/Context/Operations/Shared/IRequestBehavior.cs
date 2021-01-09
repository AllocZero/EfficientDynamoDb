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
}