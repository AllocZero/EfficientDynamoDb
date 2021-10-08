using EfficientDynamoDb.FluentCondition.Core;

namespace EfficientDynamoDb
{
    public static class Joiner
    {
        public static FilterBase And(params FilterBase[] filters) => new FilterAndWrapper(filters);

        public static FilterBase Or(params FilterBase[] filters) => new FilterOrWrapper(filters);

        public static FilterBase Not(FilterBase filter) => new FilterNotWrapper(filter);
    }
}