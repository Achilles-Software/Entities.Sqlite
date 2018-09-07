#region Namespaces

using Achilles.Entities.Relational;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.EagerFetching;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace Achilles.Entities.Linq.ExpressionVisitors
{
    class SubQueryFromClauseModelVisitor : QueryModelVisitorBase
    {
        private static readonly System.Type[] FetchResultOperators =
        {
            typeof (FetchOneRequest),
            typeof (FetchManyRequest)
        };

        public SubQueryFromClauseModelVisitor( DataContext dbContext, SqlParameterCollection parameters )
        {
        }

        public static void Visit( DataContext dbContext, SqlParameterCollection parameters, QueryModel queryModel )
        {
            var subQueryVisitor = new SubQueryFromClauseModelVisitor( dbContext, parameters );

            subQueryVisitor.VisitQueryModel( queryModel );
        }

        public override void VisitMainFromClause( MainFromClause fromClause, QueryModel queryModel )
        {
            if ( fromClause.FromExpression is SubQueryExpression subQueryExpression )
            {
                FlattenSubQuery( subQueryExpression, fromClause, queryModel, 0 );
            }

            base.VisitMainFromClause( fromClause, queryModel );
            
        }

        public override void VisitAdditionalFromClause( AdditionalFromClause fromClause, QueryModel queryModel, int index )
        {
            var subQueryExpression = fromClause.FromExpression as SubQueryExpression;
            if ( subQueryExpression != null )
            {
                FlattenSubQuery( subQueryExpression, fromClause, queryModel, index );
            }

            base.VisitAdditionalFromClause( fromClause, queryModel, index );
        }

        private static void CopyFromClauseData( FromClauseBase source, FromClauseBase destination )
        {
            destination.FromExpression = source.FromExpression;
            destination.ItemName = source.ItemName;
            destination.ItemType = source.ItemType;
        }

        private static void FlattenSubQuery( SubQueryExpression subQueryExpression, FromClauseBase fromClause, QueryModel queryModel, int destinationIndex )
        {
            //if ( !CheckFlattenable( subQueryExpression.QueryModel ) )
            //    return;

            var mainFromClause = subQueryExpression.QueryModel.MainFromClause;
            CopyFromClauseData( mainFromClause, fromClause );

            var innerSelectorMapping = new QuerySourceMapping();
            innerSelectorMapping.AddMapping( fromClause, subQueryExpression.QueryModel.SelectClause.Selector );
            //queryModel.TransformExpressions( ex => ReferenceReplacingExpressionVisitor.ReplaceClauseReferences( ex, innerSelectorMapping, false ) );

            InsertBodyClauses( subQueryExpression.QueryModel.BodyClauses, queryModel, destinationIndex );
            InsertResultOperators( subQueryExpression.QueryModel.ResultOperators, queryModel );

            var innerBodyClauseMapping = new QuerySourceMapping();
            innerBodyClauseMapping.AddMapping( mainFromClause, new QuerySourceReferenceExpression( fromClause ) );
            //queryModel.TransformExpressions( ex => ReferenceReplacingExpressionVisitor.ReplaceClauseReferences( ex, innerBodyClauseMapping, false ) );
        }

        internal static void InsertResultOperators( IEnumerable<ResultOperatorBase> resultOperators, QueryModel queryModel )
        {
            var index = 0;
            foreach ( var bodyClause in resultOperators )
            {
                queryModel.ResultOperators.Insert( index, bodyClause );
                ++index;
            }
        }

        private static void InsertBodyClauses( IEnumerable<IBodyClause> bodyClauses, QueryModel queryModel, int destinationIndex )
        {
            foreach ( var bodyClause in bodyClauses )
            {
                queryModel.BodyClauses.Insert( destinationIndex, bodyClause );
                ++destinationIndex;
            }
        }
    }
}
