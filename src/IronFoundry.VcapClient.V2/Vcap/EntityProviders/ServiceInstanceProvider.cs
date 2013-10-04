using System;
using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    internal class ServiceInstanceProvider : BaseProvider<ServiceInstance, ServiceInstanceManifest>
    {
        public ServiceInstanceProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string EntityName
        {
            get { return Constants.ServiceInstance; }
        }

        public Resource<ServiceInstance> Create(string name, Guid servicePlanId, Guid spaceId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Name must be entered");
            }

            if (EntityExists(name))
            {
                throw new VcapException();
            }
            var serviceInstanceManifest = new ServiceInstanceManifest()
                {
                    Name = name,
                    ServicePlanId = servicePlanId,
                    SpaceId = spaceId
                };

            return Create(serviceInstanceManifest);
        }
    }
}
