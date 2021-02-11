using System.Collections.Generic;
using Bam.Net.CoreServices.AccessControl;
using Bam.Net.Encryption;

namespace Bam.Net.Github.Actions.Tests
{
    public class TestGithubActionsClient: GithubActionsClient
    {
        public TestGithubActionsClient() : base()
        {
        }

        public TestGithubActionsClient(string headerValue, Vault vault) : base(new VaultAuthorizationHeaderProvider(headerValue, vault){TokenType = TokenTypes.Token})
        {
        }

        public Dictionary<string, string> CallGetHeaders(bool includeHeaders)
        {
            return GetHeaders(includeHeaders);
        }

        public void SetTestValue(string value, Vault vault = null)
        {
            vault = vault ?? Vault.Profile;
            vault[AuthorizationHeaderProvider.ConfigKey] = value;
        }
    }
}