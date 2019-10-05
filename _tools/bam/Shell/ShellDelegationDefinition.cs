using System;
using System.Configuration.Provider;
using Bam.Shell.Data;

namespace Bam.Shell
{
    public class ShellDelegationDefinition 
    {
        public Type Provider { get; set; }
        public Type Delegator { get; set; }
        public Type Implementation { get; set; }

        public ShellDelegationDescriptor ToDescriptor()
        {
            return new ShellDelegationDescriptor()
            {
                ProviderType = Provider,
                DelegatorType = Delegator,
                ImplementationType = Implementation
            };
        }

        public static ShellDelegationDefinition FromDescriptor(ShellDelegationDescriptor descriptor)
        {
            return new ShellDelegationDefinition()
            {
                Provider = descriptor.ProviderType,
                Delegator = descriptor.DelegatorType,
                Implementation = descriptor.ImplementationType
            };
        }
    }
}