using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace IndexedDB.Blazor.Extensions
{
    public static class IndexedSetExtensions
    {
        public static IndexedSet<TEntity> Include<TEntity, TProperty>(this IndexedSet<TEntity> source, Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : new()
        {
            var lambda = navigationPropertyPath as LambdaExpression;
            var member = lambda.Body as MemberExpression;

            var memberName = member.Member.Name;
            Debug.WriteLine(memberName);

            return source;
        }
    }
}
