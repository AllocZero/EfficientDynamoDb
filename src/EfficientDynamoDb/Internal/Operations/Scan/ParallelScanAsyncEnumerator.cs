using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.Scan
{
    internal sealed class ParallelScanAsyncEnumerable<TEntity> : IAsyncEnumerable<IReadOnlyList<TEntity>> where TEntity : class
    {
        internal readonly DynamoDbContext Context;
        internal readonly string TableName;
        internal readonly TotalSegmentsNode TotalSegmentsNode;

        public ParallelScanAsyncEnumerable(DynamoDbContext context, string tableName, BuilderNode? node, int totalSegments)
        {
            Context = context;
            TableName = tableName;
            TotalSegmentsNode = new TotalSegmentsNode(totalSegments, node);
        }

        public IAsyncEnumerator<IReadOnlyList<TEntity>> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new ParallelScanAsyncEnumerator<TEntity>(this, cancellationToken);
    }
    internal sealed class ParallelScanAsyncEnumerator<TEntity> : IAsyncEnumerator<IReadOnlyList<TEntity>> where TEntity : class
    {
        private readonly ParallelScanAsyncEnumerable<TEntity> _asyncEnumerable;
        private readonly List<Task<(int Segment, PagedResult<TEntity> Page)>> _tasks;
        private readonly CancellationTokenSource _cts;
        
        public IReadOnlyList<TEntity> Current { get; private set; }

        public ParallelScanAsyncEnumerator(ParallelScanAsyncEnumerable<TEntity> asyncEnumerable, CancellationToken cancellationToken)
        {
            _asyncEnumerable = asyncEnumerable;
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _tasks = new List<Task<(int Segment, PagedResult<TEntity> Page)>>(_asyncEnumerable.TotalSegmentsNode.Value);
            for (var segment = 0; segment < _asyncEnumerable.TotalSegmentsNode.Value; segment++)
                _tasks.Add(ScanSegmentAsync(_asyncEnumerable.TableName, _asyncEnumerable.TotalSegmentsNode, segment, _cts.Token));
            
            Current = null!;
        }

        public async ValueTask DisposeAsync()
        {
            _cts.Cancel();
            _cts.Dispose();

            try
            {
                await Task.WhenAll(_tasks).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
            }
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (_tasks.Count == 0)
                return false;
            
            var finishedSegmentTask = await Task.WhenAny(_tasks).ConfigureAwait(false);
            var finishedSegment = await finishedSegmentTask.ConfigureAwait(false);
            if (finishedSegment.Page.PaginationToken == null)
            {
                _tasks.Remove(finishedSegmentTask);
            }
            else
            {
                var finishedIndex = _tasks.IndexOf(finishedSegmentTask);
                _tasks[finishedIndex] = ScanSegmentAsync(_asyncEnumerable.TableName, new PaginationTokenNode(finishedSegment.Page.PaginationToken, _asyncEnumerable.TotalSegmentsNode),
                    finishedSegment.Segment,
                    _cts.Token);
            }

            Current = finishedSegment.Page.Items;
            return true;
        }

        private async Task<(int Segment, PagedResult<TEntity> Page)> ScanSegmentAsync(string tableName, BuilderNode node, int segment,
            CancellationToken cancellationToken = default)
        {
            var result = await _asyncEnumerable.Context.ScanPageAsync<TEntity>(tableName, new SegmentNode(segment, node), cancellationToken).ConfigureAwait(false);
            return (segment, result);
        }
    }
}