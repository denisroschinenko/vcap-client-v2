using System;
using System.Collections.Generic;
using System.Net.Http;
using IronFoundry.VcapClient.V2.Models;
using PortableRest;

namespace IronFoundry.VcapClient.V2
{
    internal class UserProvider : BaseProvider<User, User>
    {
        public UserProvider(VcapCredentialManager credentialManager, bool isLogin = false, bool isAuthentication = true)
            : base(credentialManager, isLogin, isAuthentication) { }

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

        public Guid Create(string username, IEnumerable<string> arrayEmails, string passwd, string firstName, string lastName)
        {
            VcapRequest.BuildRequest(HttpMethod.Post, ContentTypes.Json, Constants.UserUaa);

            var listEmails = BuildingEmails(arrayEmails);
            var user = new
                {
                    userName = username,
                    emails = listEmails,
                    password = passwd,
                    name = new { givenName = firstName, familyName = lastName }
                };
            VcapRequest.AddBodyParameter(EntityName, user);

            var tempUser = new { id = Guid.Empty };

            return VcapRequest.ExecuteAnonymousType(tempUser).id;
        }

        public Resource<User> Create(Guid userId)
        {
            VcapRequest.BuildRequest(HttpMethod.Post, ContentTypes.Json, GetEntityNameV2());
            var user = new { guid = userId };

            VcapRequest.AddBodyParameter(EntityName, user);

            return VcapRequest.Execute<Resource<User>>();
        }

        public Resource<User> Update(IEnumerable<Guid> organizationGuids, IEnumerable<Guid> managedOrganizationGuids,
                                             IEnumerable<Guid> auditedOrganizationGuids, Guid userId)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.Json, GetEntityNameV2(), userId);
            var organization =
                new
                    {
                        organization_guids = organizationGuids,
                        managed_organization_guids = managedOrganizationGuids,
                        audited_organization_guids = auditedOrganizationGuids
                    };
            VcapRequest.AddBodyParameter(string.Empty, organization);

            return VcapRequest.Execute<Resource<User>>();
        }


        public void ChangePassword(Guid userId, string newPassword, string oldPassword)
        {
            VcapRequest.BuildRequest(HttpMethod.Post, ContentTypes.FormUrlEncoded, Constants.Password, Constants.Score);
            VcapRequest.AddBodyParameter("password", newPassword);

            var score = new { score = 0, requiredScore = 0 };
            var responceInfo = VcapRequest.ExecuteAnonymousType(score);
            //TODO: need additional info how to handle previous responce

            ScorePassword(userId, newPassword, oldPassword);
        }

        private void ScorePassword(Guid userId, string newPass, string oldPass)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.Json, Constants.UserUaa, userId, Constants.Password);

            var password = new { password = newPass, oldPassword = oldPass };

            VcapRequest.AddBodyParameter("password", password);
            VcapRequest.Execute();
        }

        private List<object> BuildingEmails(IEnumerable<string> arrayEmails)
        {
            var emails = new List<object>();
            foreach (var email in arrayEmails)
            {
                emails.Add(new { value = email });
            }
            return emails;
        }

    }
}
