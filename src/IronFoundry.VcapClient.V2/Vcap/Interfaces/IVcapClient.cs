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

        IEnumerable<Resource<Application>> GetApplications();
        Resource<Application> GetApplication(Guid applicationId);
        Resource<Application> GetApplication(string applicationName);
        void PushApplication(ApplicationManifest application, string projectPath, string subDomain, string domain);
        Resource<Application> CreateApplication(ApplicationManifest application);
        Resource<Application> UpdateApplication(Resource<Application> resource);
        void DeleteApplication(Guid applicationId);
        void StartApplication(Guid applicationId);
        void StopApplication(Guid applicationId);
        void RestartApplication(Guid applicationId);
        void BindServiceToApplication(Guid serviceInstanceId, Guid applicationId);
        void UnbindServiceFromApplication(Guid serviceBindId);
        Stream DownloadApplication(Guid applicationId);
        void BindRouteApplication(Guid applicationId, Guid routeId);
        void UnbindRouteApplication(Guid applicationId, Guid routeId);

        IEnumerable<Resource<Stack>> GetStacks();
        Resource<Stack> GetStack(Guid stackId);

        Resource<Space> CreateSpace(SpaceManifest space);
        IEnumerable<Resource<Space>> GetSpaces();
        Resource<Space> GetSpace(Guid spaceId);
        void DeleteSpace(Guid spaceId);

        IEnumerable<Resource<User>> GetUsers();
        Resource<User> GetUser(Guid userId);

        IEnumerable<Resource<Organization>> GetOrganizations();
        Resource<Organization> GetOrganization(Guid organizationId);
        Resource<Organization> CreateOrganization(OrganizationManifest organization);
        void DeleteOrganization(Guid organizationId);

        IEnumerable<Resource<Service>> GetServices();
        Resource<Service> GetService(Guid serviceId);

        IEnumerable<Resource<ServiceInstance>> GetServiceInstances();
        Resource<ServiceInstance> GetServiceInstance(Guid serviceInstanceId);

        IEnumerable<Resource<ServiceBind>> GetServiceBindings();
        Resource<ServiceBind> GetServiceBinding(Guid serviceBindingId);

        IEnumerable<Resource<ServicePlan>> GetServicePlans();
        Resource<ServicePlan> GetServicePlan(Guid servicePlanId);

        IEnumerable<Resource<Domain>> GetDomainsBySpace(Guid spaceId);
        IEnumerable<Resource<Domain>> GetDomains();
        Resource<Domain> GetDomain(Guid domainId);

        IEnumerable<Resource<Event>> GetEvents();
        Resource<Event> GetEvent(Guid eventId);

        Resource<Route> CreateRoute(RouteManifest route);
        void DeleteRoute(Guid routeId);
        IEnumerable<Resource<Route>> GetRoutes();
        Resource<Route> GetRoute(Guid routeId);

        Info GetInfo();
    }
}
