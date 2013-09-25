using System;
using System.Collections.Generic;
using System.Net.Http;
using IronFoundry.VcapClient.V2.Models;
using PortableRest;

namespace IronFoundry.VcapClient.V2
{
    internal class DomainProvider : BaseProvider<Domain, Domain>
    {
        public DomainProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string Constant
        {
            get { return Constants.Domain; }
        }

        public IEnumerable<Resource<Domain>> GetDomainsBySpace(Guid spaceId)
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, V2, Constants.Space, spaceId, Constants.Domain);
            return VcapRequest.Execute<ResponseData<Domain>>().Resources;
        }
    }
}
