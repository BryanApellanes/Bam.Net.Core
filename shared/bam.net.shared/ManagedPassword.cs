using Bam.Net.Data.Repositories;
using Bam.Net.Encryption;

namespace Bam.Net
{
    public class ManagedPassword
    {
        public ManagedPassword()
        {
            JulianDate = Net.JulianDate.UtcNow;
            NameInVault = 6.RandomLetters();
        }

        public ManagedPassword(string value)
        {
            _value = value;
        }

        public static ManagedPassword Load(string nameInVault)
        {
            return new ManagedPassword(Vault.Profile[nameInVault])
            {
                NameInVault = nameInVault
            };
        }
        
        private string _nameInVault;

        /// <summary>
        /// The name of this value in the vault.
        /// </summary>
        public string NameInVault
        {
            get => _nameInVault;
            set
            {
                Vault.Profile.Remove(_nameInVault);
                _nameInVault = value;
                Save();
            }
        }
        
        public double? JulianDate { get; set; }
        
        private string _value;
        public string Value
        {
            get => "******";
            set
            {
                _value = value;
                Save();
            }
        }

        public void Set(string password)
        {
            Value = password;
        }
        
        public string Show()
        {
            return _value;
        }

        public override string ToString()
        {
            return Value;
        }

        public void Save()
        {
            Vault.Profile[NameInVault] = _value;
        }
    }
}