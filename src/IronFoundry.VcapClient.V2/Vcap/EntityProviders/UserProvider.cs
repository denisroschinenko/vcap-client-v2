
using System;
using System.Collections.Generic;
using System.Net.Http;
using IronFoundry.VcapClient.V2.Models;
using PortableRest;

namespace IronFoundry.VcapClient.V2
{
    internal class UserProvider : BaseProvider<User>
    {
        public UserProvider(VcapCredentialManager credentialManager, bool isLogin = false)
            : base(credentialManager, isLogin) { }

        protected override string Constant
        {
            get { return Constants.User; }
        }

        public void Login(string email, string password)
        {
            VcapRequest.BuildRequest(HttpMethod.Post, ContentTypes.FormUrlEncoded, Constants.OuthToken);

            VcapRequest.AddBodyParameter("grant_type", "password");
            VcapRequest.AddBodyParameter("username", email);
            VcapRequest.AddBodyParameter("password", password);

            try
            {
                var token = VcapRequest.Execute<AccessToken>();
                CredentialManager.RegisterToken(token, CredentialManager.CurrentTarget);
            }
            catch { }
        }

    }
}
