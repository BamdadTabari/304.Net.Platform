using System;
using System.Linq.Expressions;

public static class ExpressionHelper
{
    public static bool CheckExpressionForName<TEntity>(Expression<Func<TEntity, bool>> expression, string expectedName)
    {
        if (expression.Body is BinaryExpression binary &&
            binary.Left is MemberExpression member &&
            binary.Right is ConstantExpression constant)
        {
            return member.Member.Name == "name" && constant.Value?.ToString() == expectedName;
        }

        return false;
    }

    public static bool CheckExpressionForSlug<TEntity>(Expression<Func<TEntity, bool>> expression, string expectedSlug)
    {
        if (expression.Body is BinaryExpression binary &&
            binary.Left is MemberExpression member &&
            binary.Right is ConstantExpression constant)
        {
            return member.Member.Name == "slug" && constant.Value?.ToString() == expectedSlug;
        }

        return false;
    }

    public static bool TestExpressionMatch<TEntity>(
    Expression<Func<TEntity, bool>> expression,
    string propertyName,
    string expectedValue
)
    {
        if (expression.Body is BinaryExpression binaryExpr &&
            binaryExpr.Left is MemberExpression memberExpr &&
            binaryExpr.Right is ConstantExpression constantExpr)
        {
            return memberExpr.Member.Name == propertyName &&
                   constantExpr.Value?.ToString() == expectedValue;
        }

        return false;
    }

}
