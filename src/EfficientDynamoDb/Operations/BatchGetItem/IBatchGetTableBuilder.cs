using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchGetItem
{
    public interface IBatchGetTableBuilder
    {
        internal BuilderNode GetNode();

        internal Type GetTableType();
    }
    
    public interface IBatchGetTableBuilder<TTableEntity> : IBatchGetTableBuilder where TTableEntity : class
    {
        IBatchGetTableBuilder<TTableEntity> WithConsistentRead(bool useConsistentRead);
        
        IBatchGetTableBuilder<TTableEntity> WithProjectedAttributes<TProjection>() where TProjection : class;

        IBatchGetTableBuilder<TTableEntity> WithProjectedAttributes<TProjection>(params Expression<Func<TProjection, object>>[] properties) where TProjection : class;
        
        IBatchGetTableBuilder<TTableEntity> WithProjectedAttributes(params Expression<Func<TTableEntity, object>>[] properties);

        IBatchGetTableBuilder<TTableEntity> WithItems(params IBatchGetItemBuilder[] items);
        
        IBatchGetTableBuilder<TTableEntity> WithItems(IEnumerable<IBatchGetItemBuilder> items);
    }
}