using System;
using System.Linq.Expressions;

namespace SMEAppHouse.Core.CodeKits.Helpers.Expressions;

public static class ExpressionConverter
{
    public static Expression<Func<TEFModel, bool>> Convert<TEFModel, TDtoModel>(Expression<Func<TDtoModel, bool>> modelFilter)
        where TEFModel : class
         where TDtoModel : class
    {
        var parameter = Expression.Parameter(typeof(TEFModel), "bp");

        // Create a visitor to rewrite the expression
        var visitor = new ParameterReplaceVisitor<TEFModel>(modelFilter.Parameters[0], parameter);

        var body = visitor.Visit(modelFilter.Body);

        return Expression.Lambda<Func<TEFModel, bool>>(body, parameter);
    }

    internal class ParameterReplaceVisitor<TEFModel> : ExpressionVisitor where TEFModel : class
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        public ParameterReplaceVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _oldParameter = oldParameter ?? throw new ArgumentNullException(nameof(oldParameter));
            _newParameter = newParameter ?? throw new ArgumentNullException(nameof(newParameter));
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            // Replace old parameter with new parameter
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression == _oldParameter)
            {
                var newMember = typeof(TEFModel).GetMember(node.Member.Name)[0];
                return Expression.MakeMemberAccess(_newParameter, newMember);
            }
            return base.VisitMember(node);
        }
    }
}