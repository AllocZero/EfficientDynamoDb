namespace EfficientDynamoDb.Context.FluentCondition.Core
{
    public class FilterAndWrapper : IFilter
    {
        private readonly IFilter[] _filters;

        public FilterAndWrapper(params IFilter[] filters) => _filters = filters;
    }
}