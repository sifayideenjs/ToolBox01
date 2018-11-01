using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Backbone.Common.Utilities
{
    public static class ExpressionUtilities
    {
        /// <summary>
        /// Gets the member name from member expression.
        /// </summary>
        public static String GetMemberName(Expression<Func<Object>> expression)
        {
            MemberExpression mxp = null;

            if (expression.Body.NodeType == ExpressionType.Convert)
                mxp = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
                mxp = expression.Body as MemberExpression;
            else
                return String.Empty;

            return mxp.Member.Name;
        }

        /// <summary>
        /// Gets the member name from member expression. (Extension member)
        /// </summary>
        public static String GetMemberName<T>(this T source, Expression<Func<T, Object>> expression)
        {
            MemberExpression mxp = null;

            if (expression.Body.NodeType == ExpressionType.Convert)
                mxp = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
                mxp = expression.Body as MemberExpression;
            else
                return String.Empty;

            return mxp.Member.Name;
        }
    }
}
