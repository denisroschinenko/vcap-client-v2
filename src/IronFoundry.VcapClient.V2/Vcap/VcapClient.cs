﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    public class VcapClient : IVcapClient
    {
        #region Fields

        private VcapCredentialManager _credentialManager;
        private IStableDataStorage _stableDataStorage;

        #endregion

        #region Constructors

        private VcapClient(IStableDataStorage stableDataStorage)
        {
            _stableDataStorage = stableDataStorage;
        }

        public VcapClient(Uri targetUri, IStableDataStorage stableDataStorage)
            : this(stableDataStorage)
        {
            InitializationCredentialManager(targetUri);
        }

        public VcapClient(string targetUrl, IStableDataStorage stableDataStorage)
            : this(stableDataStorage)
        {
            Target(targetUrl);
        }

        #endregion

        #region Properties

        public AccessToken CurrentToken
        {
            get { return _credentialManager.CurrentToken; }
        }

        public string CurrentTarget
        {
            get { return _credentialManager.CurrentTarget.AbsoluteUri; }
        }

        #endregion

        #region Implementation IVcapClient

        public void Target(string targetUrl)
        {
            if (string.IsNullOrWhiteSpace(targetUrl))
            {
                throw new ArgumentException("targetUrl");
            }

            Uri validatedUri;
            if (!Uri.TryCreate(targetUrl, UriKind.Absolute, out validatedUri))
            {
                validatedUri = new Uri("http://" + targetUrl);
            }

            InitializationCredentialManager(validatedUri);
        }

        public void Login()
        {
            Login(string.Empty, string.Empty);
        }
        public void Login(string email, string password)
        {
            var provider = new UserProvider(_credentialManager, true, false);
            provider.Login(email, password);
        }
        public Resource<User> CreateUser(string username, IEnumerable<string> arrayEmails, string passwd, string firstName, string lastName,
            IEnumerable<Guid> organizationGuids, IEnumerable<Guid> managedOrganizationGuids, IEnumerable<Guid> auditedOrganizationGuids)
        {
            var provider = new UserProvider(_credentialManager, true);
            var userId = provider.Create(username, arrayEmails, passwd, firstName, lastName);
            return CreateUser(organizationGuids, managedOrganizationGuids, auditedOrganizationGuids, userId);
        }
        public void ChangePassword(Guid userId, string newPassword, string oldPassword)
        {
            var provider = new UserProvider(_credentialManager, true);
            provider.ChangePassword(userId, newPassword, oldPassword);
        }
        public IEnumerable<Resource<User>> GetUsers()
        {
            var provider = new UserProvider(_credentialManager);
            return provider.GetAll();
        }
        public Resource<User> GetUser(Guid userId)
        {
            var provider = new UserProvider(_credentialManager);
            return provider.GetById(userId);
        }

        public IEnumerable<Resource<Application>> GetApplicationsBySpace(Space space)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.GetApplicationsBySpace(space);
        }
        public IEnumerable<Resource<Application>> GetApplications()
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.GetAll();
        }
        public Resource<Application> GetApplication(Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.GetById(applicationId);
        }
        public Resource<Application> GetApplication(string applicationName)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.GetByParam(Constants.ParamName, applicationName);
        }
        public void PushApplication(string name, Guid stackId, Guid spaceId, long memory, int numerInstance, string projectPath, string subDomain, string domain)
        {
            Resource<Route> resourceRoute = null;
            if (!string.IsNullOrWhiteSpace(domain) && !string.IsNullOrWhiteSpace(subDomain))
            {
                var bindDomain = GetDomainByName(spaceId, domain);
                resourceRoute = GetRouteByHostName(subDomain) ?? CreateRoute(subDomain, bindDomain.Metadata.ObjectId, spaceId);
            }

            var provider = new ApplicationProvider(_credentialManager, _stableDataStorage);
            var resource = provider.PushApplication(name, stackId, spaceId, memory, numerInstance, projectPath);
            if (resourceRoute != null)
            {
                provider.BindRouteApplication(resource.Metadata.ObjectId, resourceRoute.Metadata.ObjectId);
            }
        }
        public Resource<Application> CreateApplication(string name, Guid stackId, Guid spaceId, long memory, int numerInstance)
        {
            var provider = new ApplicationProvider(_credentialManager, _stableDataStorage);
            return provider.Create(name, stackId, spaceId, memory, numerInstance);
        }
        public Resource<Application> UpdateApplication(Resource<Application> resource)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.Update(resource);
        }
        public void UpdateApplicationBits(string projectPath, Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            provider.UploadApplication(projectPath, applicationId);
            provider.RestartApplication(applicationId);
        }
        public void DeleteApplication(Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            provider.Delete(applicationId);
        }
        public void StartApplication(Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            provider.StartApplication(applicationId);
        }
        public void StopApplication(Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            provider.StopApplication(applicationId);

        }
        public void RestartApplication(Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            provider.RestartApplication(applicationId);
        }
        public Stream DownloadApplication(Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.Download(applicationId);
        }
        public IEnumerable<StatInfo> GetStats(Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.GetStats(applicationId);
        }
        public IEnumerable<InstanceDetail> GetInstances(Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.GetInstances(applicationId);
        }
        public IEnumerable<Resource<ApplicationEvent>> GetApplicationEvents(Application application)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.GetEvents(application);
        }

        public string GetApplicationLogs(Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            provider.GetInstances(applicationId);

            //TODO: Need to send request ~apps/guid/instances/guid/files/logs (strange mistake)

            return string.Empty;
        }

        public IEnumerable<Crashlog> GetApplicationCrashlogs(Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.GetCrashlogs(applicationId);

            //TODO: Need to send request ~apps/guid/instances/guid/files/logs (strange mistake)
        }
        public string GetApplicationEnvironmentVariables(Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.GetApplicationEnvironmentVariables(applicationId);
        }
        public Resource<Application> SetApplicationEnvironmentVariables(Guid applicationId, KeyValuePair<string, string> variable)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.SetApplicationEnvironmentVariables(applicationId, variable);
        }
        public Resource<Application> UnsetApplicationEnvironmentVariables(Guid applicationId, string keyVariable)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.UnsetApplicationEnvironmentVariables(applicationId, keyVariable);
        }

        public IEnumerable<Resource<Stack>> GetStacks()
        {
            var provider = new StackProvider(_credentialManager);
            return provider.GetAll();
        }
        public Resource<Stack> GetStack(Guid stackId)
        {
            var provider = new StackProvider(_credentialManager);
            return provider.GetById(stackId);
        }


        public Resource<Space> CreateSpace(string name, Guid organizationId)
        {
            var provider = new SpaceProvider(_credentialManager);
            return provider.Create(name, organizationId);
        }
        public Resource<Space> UpdateSpace(Resource<Space> resource)
        {
            var provider = new SpaceProvider(_credentialManager);
            return provider.Update(resource);
        }
        public IEnumerable<Resource<Space>> GetSpaces()
        {
            var provider = new SpaceProvider(_credentialManager);
            return provider.GetAll();
        }
        public IEnumerable<Resource<Space>> GetSpacesByOrganization(Organization organization)
        {
            var provider = new SpaceProvider(_credentialManager);
            return provider.GetSpacesByOrganization(organization);
        }
        public Resource<Space> GetSpace(Guid spaceId)
        {
            var provider = new SpaceProvider(_credentialManager);
            return provider.GetById(spaceId);
        }
        public void DeleteSpace(Guid spaceId)
        {
            var provider = new SpaceProvider(_credentialManager);
            provider.Delete(spaceId);
        }

        public IEnumerable<Resource<Organization>> GetOrganizations()
        {
            var provider = new OrganizationProvider(_credentialManager);
            return provider.GetAll();
        }
        public Resource<Organization> GetOrganization(Guid organizationId)
        {
            var provider = new OrganizationProvider(_credentialManager);
            return provider.GetById(organizationId);
        }
        public Resource<Organization> CreateOrganization(string name)
        {
            var provider = new OrganizationProvider(_credentialManager);
            return provider.Create(name);
        }
        public Resource<Organization> UpdateOrganization(Resource<Organization> resource)
        {
            var provider = new OrganizationProvider(_credentialManager);
            return provider.Update(resource);
        }
        public void DeleteOrganization(Guid organizationId)
        {
            var provider = new OrganizationProvider(_credentialManager);
            provider.Delete(organizationId);
        }


        public IEnumerable<Resource<Service>> GetServices()
        {
            var provider = new ServiceProvider(_credentialManager);
            return provider.GetAll();
        }
        public Resource<Service> GetService(Guid serviceId)
        {
            var provider = new ServiceProvider(_credentialManager);
            return provider.GetById(serviceId);
        }


        public IEnumerable<Resource<ServiceInstance>> GetServiceInstances()
        {
            var provider = new ServiceInstanceProvider(_credentialManager);
            return provider.GetAll();
        }
        public Resource<ServiceInstance> GetServiceInstance(Guid serviceInstanceId)
        {
            var provider = new ServiceInstanceProvider(_credentialManager);
            return provider.GetById(serviceInstanceId);
        }
        public Resource<ServiceInstance> CreateServiceInstance(string name, Guid servicePlanId, Guid spaceId)
        {
            var provider = new ServiceInstanceProvider(_credentialManager);
            return provider.Create(name, servicePlanId, spaceId);
        }
        public Resource<ServiceInstance> UpdateServiceInstance(Resource<ServiceInstance> resource)
        {
            var provider = new ServiceInstanceProvider(_credentialManager);
            return provider.Update(resource);
        }
        public void DeleteServiceInstance(Guid serviceInstanceId)
        {
            var provider = new ServiceInstanceProvider(_credentialManager);
            provider.Delete(serviceInstanceId);
        }


        public IEnumerable<Resource<ServiceBind>> GetServiceBindings()
        {
            var provider = new ServiceBindProvider(_credentialManager);
            return provider.GetAll();
        }
        public Resource<ServiceBind> GetServiceBinding(Guid serviceBindingId)
        {
            var provider = new ServiceBindProvider(_credentialManager);
            return provider.GetById(serviceBindingId);
        }
        public void BindServiceToApplication(Guid serviceInstanceId, Guid applicationId)
        {
            var provider = new ServiceBindProvider(_credentialManager);
            provider.BindService(serviceInstanceId, applicationId);
        }
        public void UnbindServiceFromApplication(Guid serviceBindId)
        {
            var provider = new ServiceBindProvider(_credentialManager);
            provider.Delete(serviceBindId);
        }

        public IEnumerable<Resource<ServicePlan>> GetServicePlans()
        {
            var provider = new ServicePlanProvider(_credentialManager);
            return provider.GetAll();
        }
        public Resource<ServicePlan> GetServicePlan(Guid servicePlanId)
        {
            var provider = new ServicePlanProvider(_credentialManager);
            return provider.GetById(servicePlanId);
        }


        public IEnumerable<Resource<Domain>> GetDomainsBySpace(Guid spaceId)
        {
            var provider = new DomainProvider(_credentialManager);
            return provider.GetDomainsBySpace(spaceId);
        }
        public IEnumerable<Resource<Domain>> GetDomains()
        {
            var provider = new DomainProvider(_credentialManager);
            return provider.GetAll();
        }
        public Resource<Domain> GetDomain(Guid domainId)
        {
            var provider = new DomainProvider(_credentialManager);
            return provider.GetById(domainId);
        }

        public Resource<Domain> GetDomainByName(Guid spaceId, string name)
        {
            return GetDomainsBySpace(spaceId).SingleOrDefault(x => x.Entity.Name.Equals(name));
        }

        public Resource<Domain> MapDomain(string name, Guid? organizationId, bool isWilcardExist = true)
        {
            var provider = new DomainProvider(_credentialManager);
            return provider.Create(name, organizationId, isWilcardExist);
        }
        public void UnmapDomain(Guid domainId)
        {
            var provider = new DomainProvider(_credentialManager);
            provider.Delete(domainId);
        }


        public IEnumerable<Resource<Event>> GetEvents()
        {
            var provider = new EventProvider(_credentialManager);
            return provider.GetAll();
        }
        public Resource<Event> GetEvent(Guid eventId)
        {
            var provider = new EventProvider(_credentialManager);
            return provider.GetById(eventId);
        }


        public Resource<Route> CreateRoute(string host, Guid domainId, Guid spaceId)
        {
            var provider = new RouteProvider(_credentialManager);
            return provider.Create(host, domainId, spaceId);
        }
        public void DeleteRoute(Guid routeId)
        {
            var provider = new RouteProvider(_credentialManager);
            provider.Delete(routeId);
        }
        public IEnumerable<Resource<Route>> GetRoutes()
        {
            var provider = new RouteProvider(_credentialManager);
            return provider.GetAll();
        }
        public Resource<Route> GetRoute(Guid routeId)
        {
            var provider = new RouteProvider(_credentialManager);
            return provider.GetById(routeId);
        }

        public Resource<Route> GetRouteByHostName(string name)
        {
            return GetRoutes().FirstOrDefault(x => x.Entity.Host.Equals(name));
        }
        public void BindRouteApplication(Guid applicationId, Guid routeId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            provider.BindRouteApplication(applicationId, routeId);
        }
        public void UnbindRouteApplication(Guid applicationId, Guid routeId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            provider.UnbindRouteApplication(applicationId, routeId);
        }

        public Info GetInfo()
        {
            var provider = new InfoProvider(_credentialManager);
            return provider.GetInfo();
        }


        #endregion

        #region Private methods

        private void InitializationCredentialManager(Uri targetUri)
        {
            _credentialManager = new VcapCredentialManager(targetUri, _stableDataStorage);

            var info = GetInfo();

            _credentialManager.SetLoginUri(info.AuthorizationUrl);

        }

        private Resource<User> CreateUser(IEnumerable<Guid> organizationGuids, IEnumerable<Guid> managedOrganizationGuids,
                                IEnumerable<Guid> auditedOrganizationGuids, Guid userId)
        {
            var provider = new UserProvider(_credentialManager);
            provider.Create(userId);
            return provider.Update(organizationGuids, managedOrganizationGuids, auditedOrganizationGuids, userId);
        }
        #endregion
    }
}

