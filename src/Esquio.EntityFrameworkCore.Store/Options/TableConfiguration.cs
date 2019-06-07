namespace Esquio.EntityFrameworkCore.Store.Options
{
    /// <summary>
    /// Provide table configuration for Entity Famework Core Store.
    /// </summary>
    public class TableConfiguration
    {
        /// <summary>
        /// Create a new table configuration.
        /// </summary>
        /// <param name="name">The table name.</param>
        public TableConfiguration(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Create a table configuration.
        /// </summary>
        /// <param name="name">The table name.</param>
        /// <param name="schema">The schema name.</param>
        public TableConfiguration(string name, string schema)
        {
            Name = name;
            Schema = schema;
        }

        /// <summary>
        /// Get or set the Table Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the Schema Name.
        /// </summary>
        public string Schema { get; set; }
    }
}
