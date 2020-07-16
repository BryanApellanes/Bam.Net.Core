using System;
using Bam.Net.Data.SQLite;

namespace Bam.Net.Encryption
{
    public class VaultKeyNotSetException : Exception
    {
        public VaultKeyNotSetException(Vault vault) : base(
            $"VaultKey not set: {((SQLiteDatabase) vault.Database).DatabaseFile.FullName}")
        {
        }
    }
}