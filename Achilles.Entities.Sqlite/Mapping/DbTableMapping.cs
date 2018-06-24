using Achilles.Entities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Achilles.Entities.Mapping
{
    public class DbTableMapping
    {
        public Type MappedType { get; private set; }

        public string TableName { get; private set; }

        public bool WithoutRowId { get; private set; }

        public Column[] Columns { get; private set; }

        public Column PK { get; private set; }

        public string GetByPrimaryKeySql { get; private set; }

        public CreateFlags CreateFlags { get; private set; }

        readonly Column _autoPk;
        readonly Column[] _insertColumns;
        readonly Column[] _insertOrReplaceColumns;

        public TableMapping( Type type, CreateFlags createFlags = CreateFlags.None )
        {
            MappedType = type;
            CreateFlags = createFlags;

            var typeInfo = type.GetTypeInfo();
            var tableAttr =
                typeInfo.CustomAttributes
                        .Where( x => x.AttributeType == typeof( TableAttribute ) )
                        .Select( x => (TableAttribute)Orm.InflateAttribute( x ) )
                        .FirstOrDefault();

            TableName = (tableAttr != null && !string.IsNullOrEmpty( tableAttr.Name )) ? tableAttr.Name : MappedType.Name;
            WithoutRowId = tableAttr != null ? tableAttr.WithoutRowId : false;

            var props = new List<PropertyInfo>();
            var baseType = type;
            var propNames = new HashSet<string>();
            while ( baseType != typeof( object ) )
            {
                var ti = baseType.GetTypeInfo();
                var newProps = (
                    from p in ti.DeclaredProperties
                    where
                        !propNames.Contains( p.Name ) &&
                        p.CanRead && p.CanWrite &&
                        (p.GetMethod != null) && (p.SetMethod != null) &&
                        (p.GetMethod.IsPublic && p.SetMethod.IsPublic) &&
                        (!p.GetMethod.IsStatic) && (!p.SetMethod.IsStatic)
                    select p).ToList();
                foreach ( var p in newProps )
                {
                    propNames.Add( p.Name );
                }
                props.AddRange( newProps );
                baseType = ti.BaseType;
            }

            var cols = new List<Column>();
            foreach ( var p in props )
            {
                var ignore = p.IsDefined( typeof( IgnoreAttribute ), true );
                if ( !ignore )
                {
                    cols.Add( new Column( p, createFlags ) );
                }
            }
            Columns = cols.ToArray();
            foreach ( var c in Columns )
            {
                if ( c.IsAutoInc && c.IsPK )
                {
                    _autoPk = c;
                }
                if ( c.IsPK )
                {
                    PK = c;
                }
            }

            HasAutoIncPK = _autoPk != null;

            if ( PK != null )
            {
                GetByPrimaryKeySql = string.Format( "select * from \"{0}\" where \"{1}\" = ?", TableName, PK.Name );
            }
            else
            {
                // People should not be calling Get/Find without a PK
                GetByPrimaryKeySql = string.Format( "select * from \"{0}\" limit 1", TableName );
            }

            _insertColumns = Columns.Where( c => !c.IsAutoInc ).ToArray();
            _insertOrReplaceColumns = Columns.ToArray();
        }

        public bool HasAutoIncPK { get; private set; }

        public void SetAutoIncPK( object obj, long id )
        {
            if ( _autoPk != null )
            {
                _autoPk.SetValue( obj, Convert.ChangeType( id, _autoPk.ColumnType, null ) );
            }
        }

        public Column[] InsertColumns
        {
            get
            {
                return _insertColumns;
            }
        }

        public Column[] InsertOrReplaceColumns
        {
            get
            {
                return _insertOrReplaceColumns;
            }
        }

        public Column FindColumnWithPropertyName( string propertyName )
        {
            var exact = Columns.FirstOrDefault( c => c.PropertyName == propertyName );
            return exact;
        }

        public Column FindColumn( string columnName )
        {
            var exact = Columns.FirstOrDefault( c => c.Name.ToLower() == columnName.ToLower() );
            return exact;
        }

        public class Column
        {
            PropertyInfo _prop;

            public string Name { get; private set; }

            public PropertyInfo PropertyInfo => _prop;

            public string PropertyName { get { return _prop.Name; } }

            public Type ColumnType { get; private set; }

            public string Collation { get; private set; }

            public bool IsAutoInc { get; private set; }
            public bool IsAutoGuid { get; private set; }

            public bool IsPK { get; private set; }

            public IEnumerable<IndexedAttribute> Indices { get; set; }

            public bool IsNullable { get; private set; }

            public int? MaxStringLength { get; private set; }

            public bool StoreAsText { get; private set; }

            public Column( PropertyInfo prop, CreateFlags createFlags = CreateFlags.None )
            {
                var colAttr = prop.CustomAttributes.FirstOrDefault( x => x.AttributeType == typeof( ColumnAttribute ) );

                _prop = prop;
                Name = (colAttr != null && colAttr.ConstructorArguments.Count > 0) ?
                        colAttr.ConstructorArguments[ 0 ].Value?.ToString() :
                        prop.Name;
                //If this type is Nullable<T> then Nullable.GetUnderlyingType returns the T, otherwise it returns null, so get the actual type instead
                ColumnType = Nullable.GetUnderlyingType( prop.PropertyType ) ?? prop.PropertyType;
                Collation = Orm.Collation( prop );

                IsPK = Orm.IsPK( prop ) ||
                    (((createFlags & CreateFlags.ImplicitPK) == CreateFlags.ImplicitPK) &&
                         string.Compare( prop.Name, Orm.ImplicitPkName, StringComparison.OrdinalIgnoreCase ) == 0);

                var isAuto = Orm.IsAutoInc( prop ) || (IsPK && ((createFlags & CreateFlags.AutoIncPK) == CreateFlags.AutoIncPK));
                IsAutoGuid = isAuto && ColumnType == typeof( Guid );
                IsAutoInc = isAuto && !IsAutoGuid;

                Indices = Orm.GetIndices( prop );
                if ( !Indices.Any()
                    && !IsPK
                    && ((createFlags & CreateFlags.ImplicitIndex) == CreateFlags.ImplicitIndex)
                    && Name.EndsWith( Orm.ImplicitIndexSuffix, StringComparison.OrdinalIgnoreCase )
                    )
                {
                    Indices = new IndexedAttribute[] { new IndexedAttribute() };
                }
                IsNullable = !(IsPK || Orm.IsMarkedNotNull( prop ));
                MaxStringLength = Orm.MaxStringLength( prop );

                StoreAsText = prop.PropertyType.GetTypeInfo().CustomAttributes.Any( x => x.AttributeType == typeof( StoreAsTextAttribute ) );
            }

            public void SetValue( object obj, object val )
            {
                if ( val != null && ColumnType.GetTypeInfo().IsEnum )
                {
                    _prop.SetValue( obj, Enum.ToObject( ColumnType, val ) );
                }
                else
                {
                    _prop.SetValue( obj, val, null );
                }
            }

            public object GetValue( object obj )
            {
                return _prop.GetValue( obj, null );
            }
        }
    }
}
