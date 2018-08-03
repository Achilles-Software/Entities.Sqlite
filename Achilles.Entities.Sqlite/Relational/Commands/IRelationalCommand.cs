namespace Achilles.Entities.Relational.Statements
{
    public interface IRelationalCommand
    {
        string Sql { get; }

        SqlParameterCollection Parameters { get; }
    }
}
