using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data.Repositories
{

    /// <summary>
    /// Extend this class to define a type that uses multiple properties to determine
    /// persistence instance uniqueness.  Adorn key properties with the CompositeKey
    /// attribute.  Adds the Key property to CompositeKeyAuditRepoData.
    /// </summary>
    [Serializable]
    public abstract class KeyedAuditRepoData : CompositeKeyAuditRepoData
    {
        ulong key = Convert.ToUInt64(0);
        public ulong Key
        {
            get
            {
                if (key == 0)
                {
                    key = GetULongKeyHash();
                }
                return key;
            }
            set
            {
                key = value;
            }
        }
    }
}
