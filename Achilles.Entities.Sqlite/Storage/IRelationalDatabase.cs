using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Storage
{
    public interface IRelationalDatabase
    {
        bool Exists();

        //void EnsureCreated();

        void Create();

        void Delete();
    }
}
