#region Namespaces

using Achilles.Entities.Relational;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Parsing;
using System;
using System.Linq.Expressions;
using System.Text;

#endregion

namespace Achilles.Entities.Linq.ExpressionVisitors
{
    public class SqlExpressionVisitor : ThrowingExpressionVisitor
    {
        #region Private Fields

        protected readonly DataContext _dbContext;
        protected readonly SqlParameterCollection _parameters;

        #endregion

        #region Constructor(s)

        public SqlExpressionVisitor( DataContext context, SqlParameterCollection parameters )
        {
            _dbContext = context;
            _parameters = parameters;

            Statement = new StringBuilder();
        }

        #endregion

        #region Public Properties

        public StringBuilder Statement { get; private set; }

        public string GetStatement()
        {
            return Statement.ToString();
        }

        #endregion

        #region Relinq Overrides

        public override Expression Visit( Expression expression )
        {
            return base.Visit( expression );
        }

        protected override Expression VisitBinary( BinaryExpression expression )
        {
            string op = "";
            bool arthemetic;

            switch ( expression.NodeType )
            {
                case ExpressionType.Equal:
                    op = " = ";
                    arthemetic = true;
                    break;
                case ExpressionType.NotEqual:
                    op = " <> ";
                    arthemetic = true;
                    break;
                case ExpressionType.GreaterThan:
                    op = " > ";
                    arthemetic = true;
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    op = " >= ";
                    arthemetic = true;
                    break;
                case ExpressionType.LessThan:
                    op = " < ";
                    arthemetic = true;
                    break;
                case ExpressionType.LessThanOrEqual:
                    op = " <= ";
                    arthemetic = true;
                    break;
                case ExpressionType.Add:
                    op = " + ";
                    arthemetic = true;
                    break;
                case ExpressionType.Subtract:
                    op = " - ";
                    arthemetic = true;
                    break;
                case ExpressionType.Multiply:
                    op = " * ";
                    arthemetic = true;
                    break;
                case ExpressionType.Divide:
                    op = " / ";
                    arthemetic = true;
                    break;
                case ExpressionType.AndAlso:
                    op = " AND ";
                    arthemetic = false;
                    break;
                case ExpressionType.OrElse:
                    op = " OR ";
                    arthemetic = false;
                    break;
                default:
                    base.VisitBinary( expression );
                    arthemetic = false;
                    break;
            }

            Statement.Append( "(" );

            if ( arthemetic )
            {
                if ( expression.Left.NodeType == ExpressionType.MemberAccess )
                {
                    VisitArithmeticMember( expression.Left as MemberExpression );
                }
                else
                {
                    Visit( expression.Left );
                }

                Statement.Append( op );

                if ( expression.Right.NodeType == ExpressionType.MemberAccess )
                {
                    VisitArithmeticMember( expression.Right as MemberExpression );
                }
                else
                {
                    Visit( expression.Right );
                }
            }
            else
            {
                Visit( expression.Left );
                Statement.Append( op );
                Visit( expression.Right );
            }

            Statement.Append( ")" );

            return expression;
        }

        protected override Expression VisitUnary( UnaryExpression expression )
        {
            if ( expression.NodeType == ExpressionType.Not )
            {
                Statement.Append( "NOT (" );
                Visit( expression.Operand );
                Statement.Append( ")" );
            }
            else if ( expression.NodeType == ExpressionType.Convert )
            {
                Visit( expression.Operand );
            }
            else
            {
                base.VisitUnary( expression );
            }

            return expression;
        }

        protected override Expression VisitQuerySourceReference( QuerySourceReferenceExpression expression )
        {
            Statement.Append( expression.ReferencedQuerySource.ItemName );

            return expression;
        }

        //protected override Expression VisitSubQuery( SubQueryExpression expression )
        //{
        //    return base.VisitSubQuery( expression );
        //}

        protected override Expression VisitMember( MemberExpression expression )
        {
            Visit( expression.Expression );
            Statement.AppendFormat( ".{0}", expression.Member.Name );

            if ( expression.Type == typeof( bool ) )
            {
                Statement.Append( " = 1" );
            }

            return expression;
        }

        protected virtual Expression VisitArithmeticMember( MemberExpression expression )
        {
            Visit( expression.Expression );
            Statement.AppendFormat( ".{0}", expression.Member.Name );

            return expression;
        }

        protected override Expression VisitConstant( ConstantExpression expression )
        {
            var namedParameter = _parameters.Add( expression.Value );
            Statement.AppendFormat( "@{0}", namedParameter.Name );

            return expression;
        }

        protected override Exception CreateUnhandledItemException<T>( T unhandledItem, string visitMethod )
        {
            string itemText = FormatUnhandledItem( unhandledItem );
            var message = string.Format( "The expression '{0}' (type: {1}) is not supported by this LINQ provider.", itemText, typeof( T ) );
            return new NotSupportedException( message );
        }

        #endregion

        #region Private Methods

        private string FormatUnhandledItem<T>( T unhandledItem )
        {
            return unhandledItem is Expression itemAsExpression ? itemAsExpression.ToString() : unhandledItem.ToString();
        }

        #endregion
    }
}
