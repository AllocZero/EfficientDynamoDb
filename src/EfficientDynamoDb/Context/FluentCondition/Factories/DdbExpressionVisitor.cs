using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Context.FluentCondition.Factories
{
    internal sealed class DdbExpressionVisitor : ExpressionVisitor
    {
        private readonly DynamoDbContextMetadata _metadata;
        private readonly StringBuilder _builder = new StringBuilder();
        private readonly List<string> _cachedAttributeNames = new List<string>();

        public IReadOnlyList<string> CachedAttributeNames => _cachedAttributeNames;

        public DdbClassInfo ClassInfo { get; private set; } = null!;

        public DdbExpressionVisitor(DynamoDbContextMetadata metadata)
        {
            _metadata = metadata;
        }

        public string GetEncodedExpressionName() => _builder.ToString();

        public void Visit<TEntity>(Expression expression)
        {
            var entityType = typeof(TEntity);
            ClassInfo = _metadata.GetOrAddClassInfo(entityType);

            _builder.Clear();

            Visit(expression);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ConstantExpression constantExpression && node.Member is FieldInfo fieldInfo)
            {
                var value = fieldInfo.GetValue(constantExpression.Value);
                _builder.Append(value);

                return node;
            }

            if (node.Expression.NodeType != ExpressionType.Parameter)
                Visit(node.Expression);

            if (_builder.Length > 0)
                _builder.Append('.');

            _builder.Append("#f");
            _builder.Append(_cachedAttributeNames!.Count);

            if (!ClassInfo.PropertiesMap.TryGetValue(node.Member.Name, out var ddbPropertyInfo))
                throw new DdbException(
                    $"Property {node.Member.Name} does not exist in entity {ClassInfo.Type.Name} or it's not marked by {nameof(DynamoDBPropertyAttribute)} attribute");
            
            _cachedAttributeNames.Add(ddbPropertyInfo.AttributeName);
            ClassInfo = ddbPropertyInfo.RuntimeClassInfo;

            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.ArrayIndex)
            {
                Visit(node.Left);

                _builder.Append('[');
                Visit(node.Right);
                _builder.Append(']');
                
                ClassInfo = ClassInfo.ElementClassInfo!;
            }

            return node;
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            if (node.Object.NodeType != ExpressionType.Parameter)
                Visit(node.Object);
            
            _builder.Append('[');
            Visit(node.Arguments);
            _builder.Append(']');
            
            ClassInfo = ClassInfo.ElementClassInfo!;
            
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _builder.Append(node.Value);
            
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.IsSpecialName && node.Method.Name == "get_Item")
            {
                Visit(node.Object);
                
                _builder.Append('[');
                Visit(node.Arguments);
                _builder.Append(']');

                ClassInfo = ClassInfo.ElementClassInfo!;
            }

            return node;
        }
    }
}