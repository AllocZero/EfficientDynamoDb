using System;
using System.Threading;

namespace EfficientDynamoDb.Internal.Core.Utilities
{
    internal sealed class ThreadSafeRandom : IRandom
    {
        private static int _seed = Environment.TickCount;
        private static readonly ThreadLocal<Random> Random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));

        public static readonly ThreadSafeRandom Instance = new ThreadSafeRandom();

        public int Next(int maxValue) => Random.Value.Next(maxValue);
    }
}