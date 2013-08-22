using System;
using System.Collections.Generic;
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

        public VcapClient(Uri uri, IStableDataStorage stableDataStorage)
            : this(stableDataStorage)
        {
            _credentialManager = new VcapCredentialManager(uri, stableDataStorage);
        }

        public VcapClient(string uri, IStableDataStorage stableDataStorage)
            : this(stableDataStorage)
        {
            Target(uri);
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

        public void Target(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentException("uri");
            }

            Uri validatedUri;
            if (!Uri.TryCreate(uri, UriKind.Absolute, out validatedUri))
            {
                validatedUri = new Uri("http://" + uri);
            }

            _credentialManager = new VcapCredentialManager(validatedUri, _stableDataStorage);
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
        public void PushApplication(Application application, string projectPath)
        {
            var provider = new ApplicationProvider(_credentialManager, _stableDataStorage);
            provider.PushApplication(application, projectPath);
        }
        public void UpdateApplication(Application application)
        {
            var provider = new ApplicationProvider(_credentialManager);
            provider.Update(application);
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

