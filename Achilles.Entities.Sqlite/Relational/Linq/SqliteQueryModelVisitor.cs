#region Namespaces

using Achilles.Entities.Extensions;
using Achilles.Entities.Relational.Linq.ExpressionVisitors;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Achilles.Entities.Relational.Linq
{
    public class SqliteQueryModelVisitor : QueryModelVisitorBase
    {
        #region Fields

        private readonly DataContext _context;

        private SqlParameterCollection _parameters;

        private string _fromPart;
        private List<string> _whereParts;
        private string _selectPart;
        private string _limitPart;
        private List<string> _orderByParts;
        private List<string> _joinParts;

        private string _fromClauseItemName;

        #endregion

        public SqliteQueryModelVisitor( DataContext context )
        {
            _context = context;

            _parameters = new SqlParameterCollection();

            _whereParts = new List<string>();
            _orderByParts = new List<string>();
            _joinParts = new List<string>();
        }

        public string GetSql()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat( "SELECT {0}", _selectPart );
            stringBuilder.AppendFormat( " FROM {0}", _fromPart );
            stringBuilder.AppendEnumerable( _joinParts );
            stringBuilder.AppendEnumerable( _whereParts, " WHERE ", " AND " );
            stringBuilder.AppendEnumerable( _orderByParts, " ORDER BY ", ", " );

            if ( !string.IsNullOrEmpty( _limitPart ) )
            {
                stringBuilder.AppendFormat( " LIMIT {0}", _limitPart );
            }

            return stringBuilder.ToString();
        }

        public SqlParameterCollection Parameters
        {
            get { return _parameters; }
        }

        public override void VisitMainFromClause( MainFromClause fromClause, QueryModel queryModel )
        {
            string fromTableName = fromClause.ItemType.Name;

            var EntityMapping = _context.Model.EntityMappings.GetOrAddMapping( fromClause.ItemType );

            if ( !string.IsNullOrEmpty( EntityMapping.TableName ) )
            {
                fromTableName = EntityMapping.TableName;
            }

            if ( fromClause.ItemName.StartsWith( "<generated>_", StringComparison.Ordinal ) )
            {
                // TODO: Use a shortened unique identifier.

                fromClause.ItemName= fromClause.ItemName.Replace( "<generated>_", fromTableName + "_" );
            }

            _fromPart = string.Format( "{0} as {1}", fromTableName , fromClause.ItemName );

            base.VisitMainFromClause( fromClause, queryModel );
        }

        public override void VisitResultOperator( ResultOperatorBase resultOperator, QueryModel queryModel, int index )
        {
            if ( resultOperator is SumResultOperator )
            {
                _selectPart = string.Format( "SUM({0})", _selectPart );
            }
            else if ( resultOperator is CountResultOperator )
            {
                // FIXME
                _selectPart = "COUNT(*)";
            }
            else if ( resultOperator is AnyResultOperator )
            {
                _selectPart = string.Format( "CASE COUNT({0}) WHEN 0 THEN 0 ELSE 1 END", _selectPart );
            }
            else if ( resultOperator is AverageResultOperator )
            {
                _selectPart = string.Format( "AVG({0})", _selectPart );
            }
            else if ( resultOperator is MinResultOperator )
            {
                _selectPart = string.Format( "MIN({0})", _selectPart );
            }
            else if ( resultOperator is MaxResultOperator )
            {
                _selectPart = string.Format( "MAX({0})", _selectPart );
            }
            else if ( resultOperator is FirstResultOperator )
            {
                _limitPart = "1";
            }
            else if ( resultOperator is SingleResultOperator )
            {
                // if we get more then one we throw exception
                _selectPart = string.Format( "TOP(2) {0}", _selectPart );
            }
            else if ( resultOperator is LastResultOperator )
            {
                throw new NotSupportedException( "Last is not supported, reverse the order and use First" );
            }
            else
            {
                throw new NotSupportedException( resultOperator.GetType().Name + " is not supported" );
            }

            base.VisitResultOperator( resultOperator, queryModel, index );
        }

        public override void VisitSelectClause( SelectClause selectClause, QueryModel queryModel )
        {
            _selectPart = SelectExpressionVisitor.GetStatement( _context, _parameters, selectClause.Selector );

            base.VisitSelectClause( selectClause, queryModel );
        }

        public override void VisitWhereClause( WhereClause whereClause, QueryModel queryModel, int index )
        {
            _whereParts.Add( WhereExpressionVisitor.GetStatement( _context, _parameters, whereClause.Predicate ) );
        }

        public override void VisitOrderByClause( OrderByClause orderByClause, QueryModel queryModel, int index )
        {
            foreach ( var ordering in orderByClause.Orderings )
            {
                _orderByParts.Add( OrderByExpressionVisitor.GetStatement( _context, _parameters, ordering.Expression, ordering.OrderingDirection ) );
            }
        }

        public override void VisitJoinClause( JoinClause joinClause, QueryModel queryModel, int index )
        {
            SqlExpressionVisitor sqlExpressionVisitor = new SqlExpressionVisitor( _context, _parameters );
            sqlExpressionVisitor.Visit( joinClause.InnerKeySelector );
            string innerKey = sqlExpressionVisitor.GetStatement();

            sqlExpressionVisitor = new SqlExpressionVisitor( _context, _parameters );
            sqlExpressionVisitor.Visit( joinClause.OuterKeySelector );
            string outerKey = sqlExpressionVisitor.GetStatement();

            _joinParts.Add( string.Format( " JOIN {0} AS {1} ON {2} = {3}", joinClause.ItemType.Name, joinClause.ItemName, outerKey,
                innerKey ) );

            base.VisitJoinClause( joinClause, queryModel, index );
        }
    }
}
