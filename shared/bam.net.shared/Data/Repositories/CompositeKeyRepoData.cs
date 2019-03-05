using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data.Repositories
{

    /// <summary>
    /// Extend this class to define a type that uses multiple properties to determine
    /// persistence instance uniqueness.  Addorn key properties with the CompositeKey
    /// attribute.  Adds the Key property to KeyHashAuditRepoData.
    /// </summary>
    [Serializable]
    public abstract class CompositeKeyRepoData : KeyHashAuditRepoData
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
