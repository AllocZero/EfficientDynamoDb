using System;

namespace EfficientDynamoDb.Internal.Core.Utilities
{
    internal interface IRandom
    {
        /// <inheritdoc cref="Random.Next(int)"/>
        int Next(int maxValue);
    }
}