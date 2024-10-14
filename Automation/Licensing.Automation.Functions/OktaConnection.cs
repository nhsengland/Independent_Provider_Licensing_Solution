using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okta.Sdk.Api;
using Okta.Sdk.Client;

namespace Licensing.Automation.Functions
{
    public class OktaConnection
    {
        public readonly UserApi User;
        public readonly GroupApi Group;

        public OktaConnection(AzureFunctionSettings settings)
        {
            var configuration = new Configuration
            {
                OktaDomain = settings.OktaDomain,
                AuthorizationMode = AuthorizationMode.PrivateKey,
                ClientId = settings.OktaClientId,
                Scopes = ["okta.groups.manage", "okta.users.manage"],
                PrivateKey = new JsonWebKeyConfiguration(settings.OktaPrivateKey)
            };

            User = new UserApi(configuration);
            Group = new GroupApi(configuration);
        }

    }
}
