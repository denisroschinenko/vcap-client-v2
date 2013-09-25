using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    internal class ServiceInstanceProvider : BaseProvider<ServiceInstance, ServiceInstance>
    {
        public ServiceInstanceProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string EntityName
        {
            get { return Constants.ServiceInstance; }
        }
    }
}
