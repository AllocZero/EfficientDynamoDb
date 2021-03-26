using System.Collections.Generic;

namespace EfficientDynamoDb.Operations.Shared
{
    public readonly struct PagedResult<TEntity>
    {
        public IReadOnlyList<TEntity> Items { get; }
        
        public string? PaginationToken { get; }

        public PagedResult(IReadOnlyList<TEntity> items, string? paginationToken)
        {
            Items = items;
            PaginationToken = paginationToken;
        }
    }
}