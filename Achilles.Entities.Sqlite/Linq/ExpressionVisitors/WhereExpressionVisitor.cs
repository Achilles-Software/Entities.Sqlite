#region Namespaces

using Achilles.Entities.Relational;
using System.Linq.Expressions;

#endregion

namespace Achilles.Entities.Linq.ExpressionVisitors
{
    class WhereExpressionVisitor : SqlExpressionVisitor
    {
        public WhereExpressionVisitor( DataContext dbContext, SqlParameterCollection parameters )
            : base( dbContext, parameters )
        {
        }

        public static string GetStatement( DataContext dbContext, SqlParameterCollection parameters, Expression expression )
        {
            var expressionVisitor = new WhereExpressionVisitor( dbContext, parameters );
            expressionVisitor.Statement.Append( "(" );
            expressionVisitor.Visit( expression );
            expressionVisitor.Statement.Append( ")" );

            return expressionVisitor.GetStatement();
        }
    }

}
