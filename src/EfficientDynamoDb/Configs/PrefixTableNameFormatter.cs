using System;
using System.Runtime.CompilerServices;

namespace EfficientDynamoDb.Configs
{
	public class PrefixTableNameFormatter : ITableNameFormatter
	{
		public string Prefix { get; }

		public PrefixTableNameFormatter(string prefix) {
			Prefix = prefix;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CalculateLength(ref TableNameFormatterContext context) => Prefix.Length + context.TableName.Length;

		public bool TryFormat(Span<char> buffer, ref TableNameFormatterContext context, out int length) {
			length = CalculateLength(ref context);
			if( buffer.Length < length ) {
				return false;
			}
			Prefix.AsSpan().CopyTo(buffer);
			context.TableName.AsSpan().CopyTo(buffer[Prefix.Length..]);
			return true;
		}
	}
}
