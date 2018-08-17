﻿#region Namespaces

using Achilles.Entities.Extensions;
using Achilles.Entities.Relational;
using Remotion.Linq.Clauses.Expressions;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Achilles.Entities.Linq.ExpressionVisitors
{
    public class SelectExpressionVisitor : SqlExpressionVisitor
    {
        public SelectExpressionVisitor( DataContext dbContext, SqlParameterCollection parameters )
            : base( dbContext, parameters )
        {
        }

        public static string GetStatement( DataContext dbContext, SqlParameterCollection parameters, Expression expression )
        {
            var expressionVisitor = new SelectExpressionVisitor( dbContext, parameters );

            expressionVisitor.Visit( expression );

            return expressionVisitor.GetStatement();
        }

        protected override Expression VisitQuerySourceReference( QuerySourceReferenceExpression expression )
        {
            var EntityMapping = _dbContext.Model.GetEntityMapping( expression.ReferencedQuerySource.ItemType );
            // Review: Check for null?

            Statement.AppendEnumerable( 
                EntityMapping.ColumnMappings.Select( p => p.ColumnName ), 
                string.Format( "{0}.",
                expression.ReferencedQuerySource.ItemName ), 
                string.Format( ", {0}.", expression.ReferencedQuerySource.ItemName ) );

            return expression;
        }

        protected override Expression VisitMemberInit( MemberInitExpression expression )
        {
            for ( int i = 0; i < expression.Bindings.Count; i++ )
            {
                if ( !(expression.Bindings[ i ] is MemberAssignment binding) )
                {
                    base.VisitMemberInit( expression );

                    return expression;
                }

                if ( i != 0 )
                {
                    Statement.Append( ", " );
                }

                Visit( binding.Expression );

                Statement.AppendFormat( " AS {0}", binding.Member.Name );
            }

            return expression;
        }

        protected override Expression VisitNew( NewExpression expression )
        {
            var parameters = expression.Constructor.GetParameters();

            for ( int i = 0; i < expression.Arguments.Count; i++ )
            {
                if ( i != 0 )
                    Statement.Append( ", " );

                Visit( expression.Arguments[ i ] );

                Statement.AppendFormat( " AS {0}", parameters[ i ].Name );
            }

            return expression;
        }

        protected override Expression VisitArithmeticMember( MemberExpression expression )
        {
            return VisitMember( expression );
        }

        protected override Expression VisitMember( MemberExpression expression )
        {
            if ( expression.Expression is QuerySourceReferenceExpression querySourceReferenceExpression )
            {
                Statement.Append( querySourceReferenceExpression.ReferencedQuerySource.ItemName );
            }
            else
            {
                Visit( expression.Expression );
            }

            Statement.AppendFormat( ".{0}", expression.Member.Name );

            return expression;
        }
    }
}
