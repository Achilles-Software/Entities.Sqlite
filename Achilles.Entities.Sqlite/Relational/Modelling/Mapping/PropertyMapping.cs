#region Namespaces

using Achilles.Entities.Mapping;
using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Relational.Modelling.Mapping
{
    /// <summary>
    /// Implements the <see cref="IColumnMapping"/> interface.
    /// </summary>
    public class ColumnMapping : IColumnMapping
    {
        #region  Constructor(s)

        /// <summary>
        /// Constructs a new instance of <see cref="ColumnMapping "/> from the provided <see cref="MemberInfo"/> parameter.
        /// </summary>
        /// <param name="memberInfo">A <see cref="System.Reflection.MemberInfo"/> instance.</param>
        /// <param name="isProperty">True indicates a <see cref="PropertyInfo"/> backed column; False indicates a <see cref="FieldInfo"/> backed column.</param>
        public ColumnMapping( MemberInfo memberInfo, bool isProperty = true )
        {
            MemberInfo = memberInfo;
            IsPropertyMember = isProperty;

            // TODO: TypeMapping Must be a Sqlite specific DI service

            var typeMapping = SqliteTypeMapping.FindMapping( MemberType );

            if ( typeMapping != null )
            {
                // The property is a simple scalar type. It is mapped to a database column.
                IsColumn = true;
                ColumnName = MemberInfo.Name;
                ColumnType = typeMapping.MappingTypeName;
            }
            else
            {
                // The property is either a foreign key or an entity reference 
                // single entity reference for a 1 to 1 relationship; 
                // collection entity reference for a 1 to many relationhip;

                // TJT: FIXME
            }
        }

        #endregion

        #region Public Properties

        //public Action<TEntity> Setter => CreateSetter<TEntity>(); 

        /// <inheritdoc />
        public MemberInfo MemberInfo { get; }

        /// <inheritdoc />
        public bool IsPropertyMember { get; } = true;

        /// <inheritdoc />
        public Type MemberType => IsPropertyMember ? (MemberInfo as PropertyInfo).PropertyType : (MemberInfo as FieldInfo).FieldType;
        
        /// <inheritdoc />
        public string MemberName
        {
            get { return MemberInfo.Name; }
        }

        /// <inheritdoc />
        public bool IsColumn { get; } = false;

        /// <inheritdoc />
        public string ColumnName { get; set; }

        /// <inheritdoc />
        public string ColumnType { get; set; }

        /// <inheritdoc />
        public bool IsKey { get; set; } = false;

        /// <inheritdoc />
        public bool IsRequired { get; set; } = false;

        /// <inheritdoc />
        public bool Ignore { get; set; } = false;

        /// <inheritdoc />
        public int? MaxLength { get; set; }

        /// <inheritdoc />
        public string DefaultValue { get; set; } = string.Empty;

        /// <inheritdoc />
        public bool IsUnique { get; set; } = false;

        #endregion

        //private Action<TEntity> CreateSetter<TEntity>()
        //{
        //    ParameterExpression instance = Expression.Parameter( typeof( TEntity ), "instance" );
        //    //ParameterExpression parameter = Expression.Parameter( typeof( TProperty ), "param" );

        //    var body = Expression.Call( instance, MethodInfo.GetSetMethod() );
        //    // parameters = new ParameterExpression[] { instance, parameter };

        //    return Expression.Lambda<Action<TEntity>>( body ).Compile();
        //}
    }
}
