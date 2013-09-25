using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    internal class ServicePlanProvider : BaseProvider<ServicePlan, ServicePlan>
    {
        public ServicePlanProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string EntityName
        {
            get { return Constants.ServicePlan; }
        }
    }
}
