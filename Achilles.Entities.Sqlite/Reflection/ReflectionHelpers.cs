#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Remotion.Linq.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Reflection
{
    /// <summary>
    /// Provides helper methods for reflection operations.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Returns the <see cref="MemberInfo"/> instance for the specified lamba expression.
        /// </summary>
        /// <param name="lambda">A lamba expression containing a MemberExpression.</param>
        /// <returns>A MemberInfo object for the member in the specified lambda expression.</returns>
        public static MemberInfo GetMemberInfo( LambdaExpression lambda )
        {
            Expression expr = lambda;

            while ( true )
            {
                switch ( expr.NodeType )
                {
                    case ExpressionType.Lambda:
                        expr = ((LambdaExpression)expr).Body;
                        break;

                    case ExpressionType.Convert:
                        expr = ((UnaryExpression)expr).Operand;
                        break;

                    case ExpressionType.MemberAccess:
                        var memberExpression = (MemberExpression)expr;
                        var baseMember = memberExpression.Member;
                        Type paramType;

                        while ( memberExpression != null )
                        {
                            paramType = memberExpression.Type;
                            if ( paramType.GetMembers().Any( member => member.Name == baseMember.Name ) )
                            {
                                return paramType.GetMember( baseMember.Name )[ 0 ];
                            }

                            memberExpression = memberExpression.Expression as MemberExpression;
                        }

                        // Make sure we get the property from the derived type.
                        paramType = lambda.Parameters[ 0 ].Type;
                        return paramType.GetMember( baseMember.Name )[ 0 ];

                    default:
                        return null;
                }
            }
        }

        public static Type GetMemberReturnType( MemberInfo member )
        {
            if ( member == null )
            {
                throw new ArgumentNullException( nameof( member ) );
            }

            var propertyInfo = member as PropertyInfo;
            if ( propertyInfo != null )
            {
                return propertyInfo.PropertyType;
            }

            var fieldInfo = member as FieldInfo;
            if ( fieldInfo != null )
            {
                return fieldInfo.FieldType;
            }

            var methodInfo = member as MethodInfo;
            if ( methodInfo != null )
            {
                return methodInfo.ReturnType;
            }

            throw new ArgumentException( "Argument must be FieldInfo, PropertyInfo, or MethodInfo.", "member" );
        }

        public static Type GetItemTypeOfClosedGenericIEnumerable( Type enumerableType, string argumentName )
        {
            if ( enumerableType == null )
            {
                throw new ArgumentNullException( nameof( enumerableType ) );
            }

            if ( string.IsNullOrEmpty( argumentName ) )
            {
                throw new ArgumentException( "message", nameof( argumentName ) );
            }

            Type itemType;
            if ( !ItemTypeReflectionUtility.TryGetItemTypeOfClosedGenericIEnumerable( enumerableType, out itemType ) )
            {
                var message = string.Format( "Expected a closed generic type implementing IEnumerable<T>, but found '{0}'.", enumerableType );
                throw new ArgumentException( message, argumentName );
            }

            return itemType;
        }
    }
}

