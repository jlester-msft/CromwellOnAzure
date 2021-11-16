using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace TesApi.Web
{
    public class AzureKeyVaultClient
    {
        public async Task<string> GetSecretAsync(string keyVaultUrl, string secretName, string uamiClientId = null)
        {
            var client = new SecretClient(
                vaultUri: new Uri(keyVaultUrl),
                credential: new DefaultAzureCredential(new DefaultAzureCredentialOptions {  ManagedIdentityClientId = uamiClientId }));

            var secret = await client.GetSecretAsync(secretName);

            return secret.Value.Value;
        }
    }
}
