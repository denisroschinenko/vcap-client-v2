using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    internal class ServiceBindProvider : BaseProvider<ServiceBind>
    {
        public ServiceBindProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string Constant
        {
            get { return Constants.ServiceBinding; }
        }
    }
}
