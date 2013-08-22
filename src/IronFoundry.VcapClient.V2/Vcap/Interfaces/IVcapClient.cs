using System;
using System.Collections.Generic;
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
        void PushApplication(Application application, string projectPath);
        void UpdateApplication(Application application);
        void DeleteApplication(Guid applicationId);
        void StartApplication(Guid applicationId);
        void StopApplication(Guid applicationId);
        void RestartApplication(Guid applicationId);

        IEnumerable<Resource<Stack>> GetStacks();
        Resource<Stack> GetStack(Guid stackId);

        IEnumerable<Resource<Space>> GetSpaces();
        Resource<Space> GetSpace(Guid spaceId);

        IEnumerable<Resource<User>> GetUsers();
        Resource<User> GetUser(Guid userId);

        IEnumerable<Resource<Organization>> GetOrganizations();
        Resource<Organization> GetOrganization(Guid organizationId);

        IEnumerable<Resource<Service>> GetServices();
        Resource<Service> GetService(Guid serviceId);

        IEnumerable<Resource<ServiceInstance>> GetServiceInstances();
        Resource<ServiceInstance> GetServiceInstance(Guid serviceInstanceId);

        IEnumerable<Resource<ServiceBind>> GetServiceBindings();
        Resource<ServiceBind> GetServiceBinding(Guid serviceBindingId);

        IEnumerable<Resource<ServicePlan>> GetServicePlans();
        Resource<ServicePlan> GetServicePlan(Guid servicePlanId);

        IEnumerable<Resource<Domain>> GetDomains();
        Resource<Domain> GetDomain(Guid domainId);

        IEnumerable<Resource<Event>> GetEvents();
        Resource<Event> GetEvent(Guid eventId);

        IEnumerable<Resource<Route>> GetRoutes();
        Resource<Route> GetRoute(Guid routeId);

        Info GetInfo();
    }
}
