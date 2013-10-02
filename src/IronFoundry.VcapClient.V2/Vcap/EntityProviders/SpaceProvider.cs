using System;
using System.Collections.Generic;
using System.Net.Http;
using IronFoundry.VcapClient.V2.Models;
using PortableRest;

namespace IronFoundry.VcapClient.V2
{
    internal class SpaceProvider : BaseProvider<Space, SpaceManifest>
    {
        public SpaceProvider(VcapCredentialManager credentialManager) :
            base(credentialManager)
        {
        }

        protected override string EntityName
        {
            get { return Constants.Space; }
        }

        internal Resource<Space> Create(string name, Guid organizationId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Name must be entered");
            }

            var spaceManifest = new SpaceManifest
                {
                    Name = name,
                    OrganizationId = organizationId
                };

            return Create(spaceManifest);
        }

        public IEnumerable<Resource<Space>> GetSpacesByOrganization(Organization organization)
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, organization.SpacesUrl);
            return VcapRequest.Execute<ResponseData<Space>>().Resources;
        }
    }
}
