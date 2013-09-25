﻿using System.Net.Http;
using IronFoundry.VcapClient.V2.Models;
using PortableRest;

namespace IronFoundry.VcapClient.V2
{
    internal class UserProvider : BaseProvider<User, User>
    {
        public UserProvider(VcapCredentialManager credentialManager, bool isLogin = false)
            : base(credentialManager, isLogin) { }

        protected override string EntityName
        {
            get { return Constants.User; }
        }

        public void Login(string email, string password)
        {
            VcapRequest.BuildRequest(HttpMethod.Post, ContentTypes.FormUrlEncoded, Constants.OuthToken);

            VcapRequest.AddBodyParameter("grant_type", "password");
            VcapRequest.AddBodyParameter("username", email);
            VcapRequest.AddBodyParameter("password", password);

            var token = VcapRequest.Execute<AccessToken>();
            CredentialManager.RegisterToken(token, CredentialManager.CurrentTarget);
        }
    }
}
