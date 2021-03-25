using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context
{
    public static class Joiner
    {
        public static FilterBase And(params FilterBase[] filters) => new FilterAndWrapper(filters);

        public static FilterBase Or(params FilterBase[] filters) => new FilterOrWrapper(filters);
    }
}