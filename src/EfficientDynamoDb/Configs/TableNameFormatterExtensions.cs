using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Configs
{
	internal static class TableNameFormatterExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteTableName<TState>(this ITableNameFormatter tableNameFormatter, string tableName, TState state, SpanAction<char,TState> writer) {
			var tableNameContext = new TableNameFormatterContext(tableName);
			var length = tableNameFormatter.CalculateLength(ref tableNameContext);

			char[]? pooledArray = null;
			var arr = length < NoAllocStringBuilder.MaxStackAllocSize
				? stackalloc char[length]
				: pooledArray = ArrayPool<char>.Shared.Rent(length);

			try
			{
				if( !tableNameFormatter.TryFormat(arr, ref tableNameContext, out length) ) {
					throw new DdbException($"Couldn't format table name '{tableName}' using the provided formatter");
				}

				writer(arr[..length], state);
			}
			finally
			{
				if (pooledArray != null)
				{
					pooledArray.AsSpan(0, length).Clear();
					ArrayPool<char>.Shared.Return(pooledArray);
				}
			}
		}
	}
}
