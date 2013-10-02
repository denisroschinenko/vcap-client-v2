using System;
using System.Collections.Generic;
using System.IO;
using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    public interface IVcapClient
    {
        AccessToken CurrentToken { get; }
        string CurrentTarget { get; }

        void Target(string uri);
        void Login();
        void Login(string email, string password);
        Resource<User> CreateUser(string username, IEnumerable<string> arrayEmails, string passwd, string firstName, string lastName,
            IEnumerable<Guid> organizationGuids, IEnumerable<Guid> managedOrganizationGuids, IEnumerable<Guid> auditedOrganizationGuids);
        void ChangePassword(Guid userId, string newPassword, string oldPassword);
        IEnumerable<Resource<User>> GetUsers();
        Resource<User> GetUser(Guid userId);

        IEnumerable<Resource<Application>> GetApplications();
        Resource<Application> GetApplication(Guid applicationId);
        Resource<Application> GetApplication(string applicationName);
        void PushApplication(string name, Guid stackId, Guid spaceId, long memory, int numerInstance, string projectPath, string subDomain, string domain);
        Resource<Application> CreateApplication(string name, Guid stackId, Guid spaceId, long memory, int numerInstance);
        Resource<Application> UpdateApplication(Resource<Application> resource);
        void UpdateApplicationBits(string projectPath, Guid applicationId);
        void DeleteApplication(Guid applicationId);
        void StartApplication(Guid applicationId);
        void StopApplication(Guid applicationId);
        void RestartApplication(Guid applicationId);
        Stream DownloadApplication(Guid applicationId);
        IEnumerable<StatInfo> GetStats(Guid applicationId);
        IEnumerable<InstanceDetail> GetInstances(Guid applicationId);
        IEnumerable<Resource<ApplicationEvent>> GetApplicationEvents(Application application);
        string GetApplicationLogs(Guid applicationId);
        IEnumerable<Crashlog> GetApplicationCrashlogs(Guid applicationId);

        IEnumerable<Resource<Stack>> GetStacks();
        Resource<Stack> GetStack(Guid stackId);

        Resource<Space> CreateSpace(string name, Guid organizationId);
        Resource<Space> UpdateSpace(Resource<Space> resource);
        IEnumerable<Resource<Space>> GetSpaces();
        Resource<Space> GetSpace(Guid spaceId);
        void DeleteSpace(Guid spaceId);

        IEnumerable<Resource<Organization>> GetOrganizations();
        Resource<Organization> GetOrganization(Guid organizationId);
        Resource<Organization> CreateOrganization(string name);
        Resource<Organization> UpdateOrganization(Resource<Organization> resource);
        void DeleteOrganization(Guid organizationId);

        IEnumerable<Resource<Service>> GetServices();
        Resource<Service> GetService(Guid serviceId);

        IEnumerable<Resource<ServiceInstance>> GetServiceInstances();
        Resource<ServiceInstance> GetServiceInstance(Guid serviceInstanceId);
        Resource<ServiceInstance> CreateServiceInstance(string name, Guid servicePlanId, Guid spaceId);
        Resource<ServiceInstance> UpdateServiceInstance(Resource<ServiceInstance> resource);
        void DeleteServiceInstance(Guid serviceInstanceId);

        IEnumerable<Resource<ServiceBind>> GetServiceBindings();
        Resource<ServiceBind> GetServiceBinding(Guid serviceBindingId);
        void BindServiceToApplication(Guid serviceInstanceId, Guid applicationId);
        void UnbindServiceFromApplication(Guid serviceBindId);

        IEnumerable<Resource<ServicePlan>> GetServicePlans();
        Resource<ServicePlan> GetServicePlan(Guid servicePlanId);

        IEnumerable<Resource<Domain>> GetDomainsBySpace(Guid spaceId);
        IEnumerable<Resource<Domain>> GetDomains();
        Resource<Domain> GetDomain(Guid domainId);
        Resource<Domain> GetDomainByName(Guid spaceId, string name);
        Resource<Domain> MapDomain(string name, Guid? organizationId, bool isWilcardExist = true);
        void UnmapDomain(Guid domainId);

        IEnumerable<Resource<Event>> GetEvents();
        Resource<Event> GetEvent(Guid eventId);

        Resource<Route> CreateRoute(string host, Guid domainId, Guid spaceId);
        void DeleteRoute(Guid routeId);
        IEnumerable<Resource<Route>> GetRoutes();
        Resource<Route> GetRoute(Guid routeId);
        Resource<Route> GetRouteByHostName(string name);
        void BindRouteApplication(Guid applicationId, Guid routeId);
        void UnbindRouteApplication(Guid applicationId, Guid routeId);

        Info GetInfo();
    }
}
