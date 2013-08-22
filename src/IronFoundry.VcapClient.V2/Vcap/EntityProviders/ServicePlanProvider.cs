using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    internal class ServicePlanProvider : BaseProvider<ServicePlan>
    {
        public ServicePlanProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string Constant
        {
            get { return Constants.ServicePlan; }
        }
    }
}
