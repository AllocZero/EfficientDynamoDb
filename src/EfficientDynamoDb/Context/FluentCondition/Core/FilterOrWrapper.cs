namespace EfficientDynamoDb.Context.FluentCondition.Core
{
    public class FilterOrWrapper : IFilter
    {
        private readonly IFilter[] _filters;

        public FilterOrWrapper(params IFilter[] filters) => _filters = filters;
    }
}