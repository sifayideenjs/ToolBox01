using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.Utilities
{
    public static class ReflectionUtility
    {
        private static Dictionary<String, MulticastDelegate> cache;

        static ReflectionUtility()
        {
            cache = new Dictionary<string, MulticastDelegate>();
        }

        // requires explicit specification of object type
        public static string GetPropertyName<T>(Expression<Func<T, object>> exp)
        {
            var expression = GetMemberInfo(exp);
            return expression.Member.Name;
        }

        public static Action<T, TProp> GetSetter<T, TProp>(this T source, Expression<Func<T, TProp>> exp)
        {
            var propertyName = GetMemberInfo(exp).Member.Name;

            var key = String.Format("{0}.{1}", typeof(T).FullName, propertyName);

            if (cache.ContainsKey(key))
                return (Action<T, TProp>)cache[key];

            var targetExp = Expression.Parameter(typeof(T), propertyName);
            var valueExp = Expression.Parameter(typeof(TProp));
            var fieldExp = Expression.Property(targetExp, propertyName);
            var assignExp = Expression.Assign(fieldExp, valueExp);

            var setter = Expression.Lambda<Action<T, TProp>>(assignExp, targetExp, valueExp).Compile();

            // expressions gives a better performance when cached, since compilation takes significant time.
            cache[key] = setter;

            return setter;
        }

        private static MemberExpression GetMemberInfo(Expression method)
        {
            LambdaExpression lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }
    }
}
