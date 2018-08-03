#region Namespaces

using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Mapping
{
    public class PropertyMapping : IPropertyMapping
    {
        #region  Constructor(s)

        public PropertyMapping( PropertyInfo propertyInfo )
        {
            PropertyInfo = propertyInfo;
            ColumnName = PropertyInfo.Name;

            // TODO: TypeMapping Must be a Sqlite specific DI service

            var columnTypeMapping = SqliteTypeMapping.FindMapping( PropertyType );

            if ( columnTypeMapping != null )
            {
                ColumnType = columnTypeMapping.MappingTypeName;
            }
            else
            {
                throw new NotSupportedException( "The property cannot be mapped. Name: " + PropertyName + ", Type: " + PropertyType.ToString() );
            }
        }

        #endregion

        #region Public Properties

        //public Action<TEntity> Setter => CreateSetter<TEntity>(); 

        public PropertyInfo PropertyInfo { get; }

        public Type PropertyType => PropertyInfo.PropertyType;

        public string PropertyName
        {
            get { return PropertyInfo.Name; }
        }

        public string ColumnType { get; set; }

        public string ColumnName { get; set; }

        public bool IsKey { get; set; } = false;

        public bool IsRequired { get; set; } = false;

        public bool Ignore { get; set; } = false;

        public int? MaxLength { get; set; }

        public string DefaultValue { get; set; } = string.Empty;

        public bool IsUnique { get; set; } = false;

        #endregion

        private Action<TEntity> CreateSetter<TEntity>()
        {
            ParameterExpression instance = Expression.Parameter( typeof( TEntity ), "instance" );
            //ParameterExpression parameter = Expression.Parameter( typeof( TProperty ), "param" );

            var body = Expression.Call( instance, PropertyInfo.GetSetMethod() );
            // parameters = new ParameterExpression[] { instance, parameter };

            return Expression.Lambda<Action<TEntity>>( body ).Compile();
        }
    }
}
