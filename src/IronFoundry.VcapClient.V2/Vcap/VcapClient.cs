using System;
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

        #region Methods

        private void InitializationCredentialManager(Uri targetUri)
        {
            _credentialManager = new VcapCredentialManager(targetUri, _stableDataStorage);

            var info = GetInfo();

            _credentialManager.SetLoginUri(info.AuthorizationUrl);

        }

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
            var provider = new UserProvider(_credentialManager, true);
            provider.Login(email, password);
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
        public void PushApplication(ApplicationManifest application, string projectPath, string subDomain, string domain)
        {
            Resource<Route> resourceRoute = null;
            if (!string.IsNullOrWhiteSpace(domain) && !string.IsNullOrWhiteSpace(subDomain))
            {
                var bindDomain = GetDomainsBySpace(application.SpaceGuid).SingleOrDefault(x => x.Entity.Name.Equals(domain));
                resourceRoute = GetRoutes().FirstOrDefault(x => x.Entity.Host.Equals(subDomain));
                if (resourceRoute == null)
                {
                    var routeManifest = new RouteManifest
                        {
                            DomainId = bindDomain.Metadata.ObjectId,
                            Host = subDomain,
                            SpaceId = application.SpaceGuid
                        };
                    resourceRoute = CreateRoute(routeManifest);
                }
            }

            var provider = new ApplicationProvider(_credentialManager, _stableDataStorage);
            var resource = provider.PushApplication(application, projectPath);
            if (resourceRoute != null)
            {
                provider.BindRouteApplication(resource.Metadata.ObjectId, resourceRoute.Metadata.ObjectId);
            }
        }
        public Resource<Application> CreateApplication(ApplicationManifest application)
        {
            var provider = new ApplicationProvider(_credentialManager, _stableDataStorage);
            return provider.Create(application);
        }
        public Resource<Application> UpdateApplication(Resource<Application> resource)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.Update(resource);
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
        public Stream DownloadApplication(Guid applicationId)
        {
            var provider = new ApplicationProvider(_credentialManager);
            return provider.Download(applicationId);
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


        public Resource<Space> CreateSpace(SpaceManifest space)
        {
            var provider = new SpaceProvider(_credentialManager);
            return provider.Create(space);
        }
        public IEnumerable<Resource<Space>> GetSpaces()
        {
            var provider = new SpaceProvider(_credentialManager);
            return provider.GetAll();
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
        public Resource<Organization> CreateOrganization(OrganizationManifest organization)
        {
            var provider = new OrganizationProvider(_credentialManager);
            return provider.Create(organization);
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


        public Resource<Route> CreateRoute(RouteManifest route)
        {
            var provider = new RouteProvider(_credentialManager);
            return provider.Create(route);
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

        public Info GetInfo()
        {
            var provider = new InfoProvider(_credentialManager);
            return provider.GetInfo();
        }

        #endregion
    }
}

