using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data
{
    /// <summary>
    /// When implemented provides a Dao hydration mechanism.  See Hydrator.
    /// </summary>
    public interface IHydrator
    {
        bool TryHydrate(Dao dao, Database database = null);
        void Hydrate(Dao dao, Database database = null);
    }
}
