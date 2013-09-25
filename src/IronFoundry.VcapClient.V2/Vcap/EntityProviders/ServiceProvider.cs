using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    internal class ServiceProvider : BaseProvider<Service, Service>
    {
        public ServiceProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string Constant
        {
            get { return Constants.Service; }
        }
    }
}
