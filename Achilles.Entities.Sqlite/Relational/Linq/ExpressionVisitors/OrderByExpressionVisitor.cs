#region Namespaces

using Remotion.Linq.Clauses;
using System.Linq.Expressions;

#endregion

namespace Achilles.Entities.Relational.Linq.ExpressionVisitors
{
    class OrderByExpressionVisitor : SqlExpressionVisitor
    {
        #region Constructor(s)

        public OrderByExpressionVisitor(DataContext dbContext, SqlParameterCollection parameters )
            : base( dbContext,parameters )
        {
        }

        #endregion

        public static string GetStatement( DataContext dbContext, SqlParameterCollection parameters, Expression expression, OrderingDirection orderingDirection )
        {
            var expressionVisitor = new OrderByExpressionVisitor( dbContext, parameters );
            expressionVisitor.Visit( expression );

            if ( orderingDirection == OrderingDirection.Desc )
            {
                expressionVisitor.Statement.Append( " desc" );
            }

            return expressionVisitor.GetStatement();
        }
    }

}
