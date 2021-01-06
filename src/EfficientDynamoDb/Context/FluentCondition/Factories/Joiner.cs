using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Factories
{
    public static class Joiner
    {
        public static IFilter And(params IFilter[] filters) => new FilterAndWrapper(filters);

        public static IFilter Or(params IFilter[] filters) => new FilterOrWrapper(filters);
    }
}