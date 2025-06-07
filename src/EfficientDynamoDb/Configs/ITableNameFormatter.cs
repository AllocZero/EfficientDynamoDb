using System;

namespace EfficientDynamoDb.Configs
{
	public readonly struct TableNameFormatterContext
	{
		public TableNameFormatterContext(string tableName) {
			TableName = tableName;
		}

		public string TableName { get; }
	}

	public interface ITableNameFormatter
	{
		int CalculateLength(ref TableNameFormatterContext context);
		bool TryFormat(Span<char> buffer, ref TableNameFormatterContext context, out int length);
	}
}
