
using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{

    internal class OrganizationProvider : BaseProvider<Organization>
    {
        public OrganizationProvider(VcapCredentialManager credentialManager) : base(credentialManager)
        {
        }

        protected override string Constant
        {
            get { return Constants.Organization; }
        }
    }
}
