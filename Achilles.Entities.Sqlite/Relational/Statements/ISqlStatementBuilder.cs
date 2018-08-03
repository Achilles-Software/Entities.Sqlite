namespace Achilles.Entities.Relational.Statements
{
    internal interface ISqlStatementBuilder<out TStatement>
        where TStatement : ISqlStatement
    {
        TStatement BuildStatement();
    }
}
