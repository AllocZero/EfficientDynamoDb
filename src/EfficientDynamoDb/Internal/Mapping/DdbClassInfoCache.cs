using System;
using System.Collections.Concurrent;

namespace EfficientDynamoDb.Internal.Mapping
{
    internal static class DdbClassInfoCache
    {
        private static readonly ConcurrentDictionary<Type, DdbClassInfo> Cache = new ConcurrentDictionary<Type, DdbClassInfo>();

        public static DdbClassInfo GetOrAdd(Type classType) => Cache.GetOrAdd(classType, x => new DdbClassInfo(x));
    }
}