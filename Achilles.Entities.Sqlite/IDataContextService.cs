namespace Achilles.Entities
{
    public interface IDataContextService
    {
        DataContext Instance { get; }

        IDataContextService Initialize( DataContext context );
    }
}
