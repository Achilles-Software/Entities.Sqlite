﻿using Achilles.Entities.Sqlite.Statements;
using Achilles.Entities.Sqlite.Statements.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Mapping
{
    //internal class SqliteAssociationType
    //{
    //    public SqliteAssociationType( EntityMap associationType, EntityMapping container )
    //    {
    //        FromRoleEntitySetName = associationType.Constraint.FromRole.Name;
    //        ToRoleEntitySetName = associationType.Constraint.ToRole.Name;

    //        string fromTable = container.GetEntitySetByName( FromRoleEntitySetName, true ).Table;
    //        string toTable;

    //        if ( IsSelfReferencing( associationType ) )
    //        {
    //            toTable = fromTable;
    //            ToRoleEntitySetName = FromRoleEntitySetName;
    //        }
    //        else
    //        {
    //            toTable = container.GetEntitySetByName( ToRoleEntitySetName, true ).Table;
    //        }

    //        FromTableName = NameCreator.EscapeName( fromTable );
    //        ToTableName = NameCreator.EscapeName( toTable );
    //        ForeignKey = associationType.Constraint.ToProperties.Select( x => x.Name );
    //        ForeignPrimaryKey = associationType.Constraint.FromProperties.Select( x => x.Name );
    //        CascadeDelete = associationType.Constraint.FromRole.DeleteBehavior == OperationAction.Cascade;
    //    }

    //    private static bool IsSelfReferencing( AssociationType associationType )
    //    {
    //        var toRoleRefType = (RefType)associationType.Constraint.ToRole.TypeUsage.EdmType;
    //        var fromRoleRefType = (RefType)associationType.Constraint.FromRole.TypeUsage.EdmType;

    //        bool isSelfReferencing = toRoleRefType.ElementType.Name == fromRoleRefType.ElementType.Name;

    //        return isSelfReferencing;
    //    }

    //    public string ToRoleEntitySetName { get; set; }
    //    public string FromRoleEntitySetName { get; set; }
    //    public IEnumerable<string> ForeignKey { get; }
    //    public string FromTableName { get; }
    //    public string ToTableName { get; }
    //    public IEnumerable<string> ForeignPrimaryKey { get; }
    //    public bool CascadeDelete { get; }
    //}
}
