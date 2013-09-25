using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    internal class RouteProvider : BaseProvider<Route, RouteManifest>
    {
        public RouteProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string Constant
        {
            get { return Constants.Route; }
        }
    }
}
