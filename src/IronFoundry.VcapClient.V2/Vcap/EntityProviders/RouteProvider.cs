using System;
using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    internal class RouteProvider : BaseProvider<Route, RouteManifest>
    {
        public RouteProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string EntityName
        {
            get { return Constants.Route; }
        }

        public Resource<Route> Create(string host, Guid domainId, Guid spaceId)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentNullException("Host must be entered");
            }

            if (EntityExists(host))
            {
                throw new VcapException();
            }

            var routeManifest = new RouteManifest
            {
                DomainId = domainId,
                Host = host,
                SpaceId = spaceId,
            };

            return Create(routeManifest);
        }
    }
}
