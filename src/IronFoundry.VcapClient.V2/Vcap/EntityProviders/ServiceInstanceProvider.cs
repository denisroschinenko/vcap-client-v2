using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    internal class ServiceInstanceProvider : BaseProvider<ServiceInstance>
    {
        public ServiceInstanceProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string Constant
        {
            get { return Constants.ServiceInstance; }
        }
    }
}
