using System;
using System.Collections.Generic;
using System.Net.Http;
using IronFoundry.VcapClient.V2.Models;
using PortableRest;

namespace IronFoundry.VcapClient.V2
{
    internal class DomainProvider : BaseProvider<Domain, DomainManifest>
    {
        public DomainProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string EntityName
        {
            get { return Constants.Domain; }
        }

        public IEnumerable<Resource<Domain>> GetDomainsBySpace(Guid spaceId)
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, GetEntityNameV2(Constants.Space), spaceId, EntityName);
            return VcapRequest.Execute<ResponseData<Domain>>().Resources;
        }

        public Resource<Domain> Create(string name, Guid? organizationId, bool isWilcardExist = true)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Name must be entered");
            }

            if (EntityExists(name))
            {
                throw new VcapException();
            }

            var domainManifest = new DomainManifest
                {
                    Name = name,
                    OwningOrganizationId = organizationId,
                    IsWilcardExist = isWilcardExist
                };
            return Create(domainManifest);
        }
    }
}
