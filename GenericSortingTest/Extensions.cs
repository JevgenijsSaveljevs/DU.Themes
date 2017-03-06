using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GenericSortingTest
{
    public static class Extensions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string filter)
            where T : class
        {

            var type = typeof(T);
            var method = "OrderBy";
            var parameter = Expression.Parameter(type, "p");

            var keys = filter.Split('.');

            var propertyType = typeof(T);
            MemberExpression me = null;

            foreach (var propertyKey in keys)
            {
                if (me == null)
                {
                    me = Expression.Property(parameter, propertyKey);
                }
                else
                {
                    me = Expression.Property(me, propertyKey);
                }

                propertyType = me.Type;
            }

            var orderByExp = Expression.Lambda(me, parameter);

            // OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression <Func<TSource, TKey>> keySelector)

            MethodCallExpression resultExp = Expression.Call(
                    type: typeof(Queryable),
                    methodName: method,
                    typeArguments: new Type[] { type, propertyType }, // OrderBy<TSource, TKey>
                    arguments: new[]
                    {
                        source.Expression,               // param1: this IQueryable <TSource> source
                        Expression.Quote(orderByExp)     // param2: Expression <Func<TSource, TKey>> keySelector
                    });

            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
