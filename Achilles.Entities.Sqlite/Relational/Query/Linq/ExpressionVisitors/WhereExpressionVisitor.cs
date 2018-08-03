#region Namespaces

using System.Linq.Expressions;

#endregion

namespace Achilles.Entities.Relational.Query.Linq.ExpressionVisitors
{
    class WhereExpressionVisitor : SqlExpressionVisitor
    {
        public WhereExpressionVisitor( DbContext dbContext, SqlParameterCollection parameters )
            : base( dbContext, parameters )
        {
        }

        public static string GetStatement( DbContext dbContext, SqlParameterCollection parameters, Expression expression )
        {
            var expressionVisitor = new WhereExpressionVisitor( dbContext, parameters );
            expressionVisitor.Statement.Append( "(" );
            expressionVisitor.Visit( expression );
            expressionVisitor.Statement.Append( ")" );

            return expressionVisitor.GetStatement();
        }
    }

}
