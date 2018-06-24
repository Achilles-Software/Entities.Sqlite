using System;

namespace Achilles.Entities.Mapping
{
    [Flags]
    public enum CreateFlags
    {
        /// <summary>
        /// Use the default creation options
        /// </summary>
        None = 0x000,
        /// <summary>
        /// Create a primary key index for a property called 'Id' (case-insensitive).
        /// This avoids the need for the [PrimaryKey] attribute.
        /// </summary>
        ImplicitPK = 0x001,
        /// <summary>
        /// Create indices for properties ending in 'Id' (case-insensitive).
        /// </summary>
        ImplicitIndex = 0x002,
        /// <summary>
        /// Create a primary key for a property called 'Id' and
        /// create an indices for properties ending in 'Id' (case-insensitive).
        /// </summary>
        AllImplicit = 0x003,
        /// <summary>
        /// Force the primary key property to be auto incrementing.
        /// This avoids the need for the [AutoIncrement] attribute.
        /// The primary key property on the class should have type int or long.
        /// </summary>
        AutoIncPK = 0x004,
        /// <summary>
        /// Create virtual table using FTS3
        /// </summary>
        FullTextSearch3 = 0x100,
        /// <summary>
        /// Create virtual table using FTS4
        /// </summary>
        FullTextSearch4 = 0x200
    }
}
