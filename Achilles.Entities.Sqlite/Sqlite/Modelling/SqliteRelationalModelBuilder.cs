#region Namespaces

using Achilles.Entities.Relational.Modelling;

#endregion

namespace Achilles.Entities.Sqlite.Modelling
{
    public class SqliteRelationalModelBuilder : RelationalModelBuilder
    {
        public override IRelationalModel Build( DbContext context )
        {
            var modelBuilder = new MappingConfiguration();

            context.OnModelMapping( modelBuilder );

            return new RelationalModel( modelBuilder.EntityMappings );
        }
    }
}
