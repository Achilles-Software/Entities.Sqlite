using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Sqlite.Storage
{
    [Flags]
    public enum SqliteConnectionOpenFlags
    {
        ReadOnly = 1,
        ReadWrite = 2,
        Create = 4,

        NoMutex = 0x8000,
        FullMutex = 0x10000,

        SharedCache = 0x20000,
        PrivateCache = 0x40000,

        ProtectionComplete = 0x00100000,
        ProtectionCompleteUnlessOpen = 0x00200000,
        ProtectionCompleteUntilFirstUserAuthentication = 0x00300000,
        ProtectionNone = 0x00400000
    }
}
