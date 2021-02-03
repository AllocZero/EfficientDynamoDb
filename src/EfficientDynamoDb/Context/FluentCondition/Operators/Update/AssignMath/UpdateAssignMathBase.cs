using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignMath
{
    internal abstract class UpdateAssignMathBase : UpdateBase
    {
        private readonly AssignMathOperator _mathOperator;

        protected UpdateAssignMathBase(Expression expression, AssignMathOperator mathOperator) : base(expression)
        {
            _mathOperator = mathOperator;
        }

        protected void AppendMathOperatorExpression(ref NoAllocStringBuilder builder)
        {
            switch (_mathOperator)
            {
                case AssignMathOperator.Plus:
                    builder.Append(" + ");
                    break;
                case AssignMathOperator.Minus:
                    builder.Append(" - ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_mathOperator), "Specified math operator is not supported");
            }
        }
    }
}