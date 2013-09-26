using System;
using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{

    internal class OrganizationProvider : BaseProvider<Organization, OrganizationManifest>
    {
        public OrganizationProvider(VcapCredentialManager credentialManager)
            : base(credentialManager)
        {
        }

        protected override string EntityName
        {
            get { return Constants.Organization; }
        }

        public Resource<Organization> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Name must be entered");
            }

            var organizationManifest = new OrganizationManifest() { Name = name };

            return Create(organizationManifest);
        }
    }
}
