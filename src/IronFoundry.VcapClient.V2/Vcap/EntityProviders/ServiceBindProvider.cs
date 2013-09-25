using System;
using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    internal class ServiceBindProvider : BaseProvider<ServiceBind, ServiceBindManifest>
    {
        public ServiceBindProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string Constant
        {
            get { return Constants.ServiceBinding; }
        }

        public Resource<ServiceBind> BindService(Guid serviceInstanceId, Guid applicationId)
        {
            var serviceBindManifest = new ServiceBindManifest
                {
                    ApplicationId = applicationId,
                    ServiceInstanceId = serviceInstanceId
                };

            return Create(serviceBindManifest);
        }
    }
}
