using IronFoundry.VcapClient.V2.Models;

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
    }
}
