using System;
using Bam.Net.Data.Repositories;

namespace Bam.Shell.Data
{
    public class ShellDelegationDescriptor : CompositeKeyAuditRepoData
    {
        Type _providerType;
        string _providerTypeName;
        public string Provider
        {
            get
            {
                if (string.IsNullOrEmpty(_providerTypeName))
                {
                    _providerTypeName = _providerType?.FullName;
                }

                return _providerTypeName;
            }
            set
            {
                _providerTypeName = value;
                _providerType = Type.GetType(_providerTypeName);
            }
        }

        protected internal Type ProviderType
        {
            get => _providerType;
            set => _providerType = value;
        }
        
        Type _delegatorType;
        string _delegatorTypeName;
        public string Delegator
        {
            get
            {
                if (string.IsNullOrEmpty(_delegatorTypeName))
                {
                    _delegatorTypeName = _delegatorType?.FullName;
                }

                return _delegatorTypeName;
            }
            set
            {
                _delegatorTypeName = value;
                _delegatorType = Type.GetType(_delegatorTypeName);
            }
        }

        protected internal Type DelegatorType
        {
            get => _delegatorType;
            set => _delegatorType = value;
        }
        
        Type _implementationType;
        string _implementationTypeName;
        public string Implementation
        {
            get
            {
                if (string.IsNullOrEmpty(_implementationTypeName))
                {
                    _implementationTypeName = _implementationType?.FullName;
                }

                return _implementationTypeName;
            }
            set
            {
                _implementationTypeName = value;
                _implementationType = Type.GetType(_implementationTypeName);
            }
        }

        protected internal Type ImplementationType
        {
            get => _implementationType;
            set => _implementationType = value;
        }
    }
}