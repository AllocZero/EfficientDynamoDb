using System;

namespace EfficientDynamoDb.Configs
{
	public class PrefixTableNameFormatter : ITableNameFormatter
	{
		public string Prefix { get; }

		public PrefixTableNameFormatter(string prefix) {
			Prefix = prefix;
		}

		public string FormatTableName(ref TableNameFormatterContext context) {
			return $"{Prefix}{context.TableName}";
		}

		public int CalculateLength(ref TableNameFormatterContext context) => Prefix.Length + context.TableName.Length;

		public bool TryFormat(Span<char> buffer, ref TableNameFormatterContext context, out int length) {
			length = Prefix.Length + context.TableName.Length;
			if( buffer.Length < length ) {
				return false;
			}
			Prefix.AsSpan().CopyTo(buffer);
			context.TableName.AsSpan().CopyTo(buffer[Prefix.Length..]);
			return true;
		}
	}
}
