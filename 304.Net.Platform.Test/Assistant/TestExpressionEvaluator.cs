using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace _304.Net.Platform.Test.Assistant;
public static class TestExpressionEvaluator
{
	public static bool MatchPredicate<TEntity>(Expression<Func<TEntity, bool>> actualExpr, Expression<Func<TEntity, bool>> expectedExpr)
	{
		return actualExpr.ToString() == expectedExpr.ToString();
	}
	//public static bool MatchSlugExpression<TEntity>(Expression<Func<TEntity, bool>> expr, string expectedSlug)
	//{
	//	if (expr.Body is BinaryExpression binary &&
	//		binary.Left is MemberExpression member &&
	//		member.Member.Name.Equals("slug", StringComparison.OrdinalIgnoreCase))
	//	{
	//		var rightValue = GetValueFromExpression(binary.Right);
	//		return rightValue?.ToString() == expectedSlug;
	//	}

	//	return false;
	//}

	//private static object? GetValueFromExpression(Expression expression)
	//{
	//	if (expression is ConstantExpression constant)
	//	{
	//		return constant.Value;
	//	}

	//	if (expression is MemberExpression memberExpression)
	//	{
	//		var objectMember = Expression.Convert(memberExpression, typeof(object));
	//		var getterLambda = Expression.Lambda<Func<object>>(objectMember);
	//		var getter = getterLambda.Compile();
	//		return getter();
	//	}

	//	return null;
	//}
}