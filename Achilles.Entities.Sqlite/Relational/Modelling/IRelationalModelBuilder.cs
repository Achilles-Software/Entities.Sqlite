namespace Achilles.Entities.Relational.Modelling
{
    public interface IRelationalModelBuilder
    {
        IRelationalModel Build( DbContext context );
    }
}
