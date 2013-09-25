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
    }
}
