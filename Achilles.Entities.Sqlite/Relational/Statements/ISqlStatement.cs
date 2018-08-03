namespace Achilles.Entities.Relational.Statements
{
    public interface ISqlStatement
    {
        /// <summary>
        /// Gets the statement SQL text.
        /// </summary>
        /// <returns>The statement SQL text</returns>
        string GetText();
    }
}
