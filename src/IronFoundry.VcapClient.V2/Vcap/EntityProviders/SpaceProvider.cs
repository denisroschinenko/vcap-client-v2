using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    internal class SpaceProvider : BaseProvider<Space, Space>
    {
        public SpaceProvider(VcapCredentialManager credentialManager) :
            base(credentialManager)
        {
        }

        protected override string Constant
        {
            get { return Constants.Space; }
        }
    }
}
