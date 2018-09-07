#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Extensions;
using Achilles.Entities.Relational;
using Remotion.Linq.Clauses.Expressions;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Achilles.Entities.Linq.ExpressionVisitors
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectExpressionVisitor : SqlExpressionVisitor
    {
        #region Fields

        private bool _inProjection = false;
        private MemberAssignment _projectionBinding;

        #endregion

        #region Constructor(s)

        public SelectExpressionVisitor( DataContext dbContext, SqlParameterCollection parameters )
            : base( dbContext, parameters )
        {
        }

        #endregion

        public static string GetStatement( DataContext dbContext, SqlParameterCollection parameters, Expression expression )
        {
            var selectExpressionVisitor = new SelectExpressionVisitor( dbContext, parameters );

            selectExpressionVisitor.Visit( expression );

            return selectExpressionVisitor.GetStatement();
        }

        protected override Expression VisitQuerySourceReference( QuerySourceReferenceExpression expression )
        {
            var EntityMapping = _dbContext.Model.GetEntityMapping( expression.ReferencedQuerySource.ItemType );

            if ( _inProjection )
            {
                Statement.AppendEnumerable(
                    EntityMapping.ColumnMappings.Select( p => p.ColumnName ),
                    string.Format( "{0}.", expression.ReferencedQuerySource.ItemName ),
                    ", ",
                    _projectionBinding.Member.Name );

            }
            else
            {
                Statement.AppendEnumerable(
                EntityMapping.ColumnMappings.Select( p => p.ColumnName ),
                string.Format( "{0}.", expression.ReferencedQuerySource.ItemName ),
                ", " );
            }

            return expression;
        }

        /// <summary>
        /// The MemberInit expression creates a new object and initializes the object properties through the expression bindings.
        /// </summary>
        /// <example>
        /// select new EntityType { }
        /// </example>
        /// <param name="expression">The MemberInit expression.</param>
        /// <returns>The MemberInit expression parameter.</returns>
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

                if ( binding.Expression.NodeType == ExpressionType.Extension )
                {
                    _inProjection = true;
                    _projectionBinding = binding;
                }
                else
                {
                    _inProjection = false;
                }

                Visit( binding.Expression );

                if ( binding.Expression.NodeType == ExpressionType.MemberAccess )
                {
                    Statement.AppendFormat( " AS {0}", binding.Member.Name );
                }
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
