using DataLayer.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace _304.Net.Platform.Test.Base;
public static class Injector
{
    public static void InjectSpecialRepositories(Mock<IUnitOfWork> unitOfWorkMock)
    {
        var properties = typeof(IUnitOfWork).GetProperties();

        foreach (var prop in properties)
        {
            // فقط ریپازیتوری‌هایی که generic interface دارند
            if (prop.Name.EndsWith("Repository") && prop.PropertyType.IsInterface)
            {
                // Mock از نوع صحیح بساز
                var mockType = typeof(Mock<>).MakeGenericType(prop.PropertyType);
                var mockInstance = Activator.CreateInstance(mockType);

                if (mockInstance is not null)
                {
                    var objectProp = mockType.GetProperty("Object");
                    var mockObject = objectProp?.GetValue(mockInstance);

                    // اگر متد ExistsAsync داره، شبیه‌سازیش کن
                    var existsMethod = prop.PropertyType.GetMethod("ExistsAsync");
                    if (existsMethod != null)
                    {
                        // ست کردن مقدار false پیش‌فرض برای ExistsAsync
                        var setupMethod = mockType.GetMethod("Setup");
                        var lambda = CreateLambdaForExistsAsync(prop.PropertyType);
                        var setupCall = setupMethod?.Invoke(mockInstance, new object[] { lambda });
                        var returnsAsync = setupCall?.GetType().GetMethod("ReturnsAsync", new[] { typeof(bool) });
                        returnsAsync?.Invoke(setupCall, new object[] { false });
                    }

                    // مقدار دهی به unitOfWorkMock
                    prop.SetValue(unitOfWorkMock.Object, mockObject);
                    unitOfWorkMock.Setup(x => prop.GetValue(x)).Returns(mockObject);
                }
            }
        }
    }

    private static object CreateLambdaForExistsAsync(Type repositoryInterfaceType)
    {
        var method = repositoryInterfaceType.GetMethod("ExistsAsync");
        if (method == null) return null!;

        var expressionType = method.GetParameters()[0].ParameterType;
        var parameterType = expressionType.GenericTypeArguments[0];

        var paramExpr = Expression.Parameter(expressionType, "x");
        var lambdaType = typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(parameterType, typeof(bool)));
        return Activator.CreateInstance(lambdaType, Expression.Lambda(Expression.Constant(true), paramExpr))!;
    }

}
