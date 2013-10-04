using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using IronFoundry.VcapClient.V2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PortableRest;


namespace IronFoundry.VcapClient.V2
{
    internal class ApplicationProvider : BaseProvider<Application, ApplicationManifest>
    {
        private readonly IStableDataStorage StableDataStorage;

        public ApplicationProvider(VcapCredentialManager credentialManager) :
            base(credentialManager) { }

        public ApplicationProvider(VcapCredentialManager credentialManager, IStableDataStorage stableDataStorage)
            : this(credentialManager)
        {
            StableDataStorage = stableDataStorage;
        }

        protected override string EntityName
        {
            get { return Constants.Application; }
        }

        public Resource<Application> StartApplication(Guid applicationId)
        {
            var resource = GetById(applicationId);
            resource.Entity.State = Application.ApplicationStates.Started;
            return Update(resource);
        }
        public Resource<Application> StopApplication(Guid applicationId)
        {
            var resource = GetById(applicationId);
            resource.Entity.State = Application.ApplicationStates.Stopped;
            return Update(resource);

        }
        public void RestartApplication(Guid applicationId)
        {
            StopApplication(applicationId);
            StartApplication(applicationId);
        }

        public Resource<Application> PushApplication(string name, Guid stackId, Guid spaceId, long memory, int numerInstance, string projectPath)
        {
            if (string.IsNullOrWhiteSpace(projectPath))
            {
                throw new ArgumentNullException("Path must be entered");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Name must be entered");
            }
            if (memory <= 0)
            {
                throw new ArgumentNullException("Memory must be entered");
            }
            if (numerInstance <= 0)
            {
                throw new ArgumentNullException("Instance must be entered");
            }

            var resource = Create(name, stackId, spaceId, memory, numerInstance);

            UploadApplication(projectPath, resource.Metadata.ObjectId);

            resource = StartApplication(resource.Metadata.ObjectId);
            return resource;
        }

        public void UploadApplication(string projectPath, Guid applicationId)
        {
            if (string.IsNullOrWhiteSpace(projectPath))
            {
                throw new ArgumentNullException("projectPath path must be entered");
            }

            var tempDirectoryPath = StableDataStorage.CopyProjectToTempDirectory(projectPath);
            var resources = StableDataStorage.FilteringResources(tempDirectoryPath, CheckResources);
            byte[] fileBytes = StableDataStorage.CreateZipFile(tempDirectoryPath);

            UploadBits(applicationId, fileBytes, resources);
        }

        private ResourceFile[] CheckResources(ResourceFile[] resourcesArray)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.Json, GetEntityNameV2(Constants.ResourceMatch));
            VcapRequest.AddBodyParameter("resources", resourcesArray);
            return VcapRequest.Execute<ResourceFile[]>();
        }

        private void UploadBits(Guid applicationId, byte[] fileBytes, ResourceFile[] resourcesArray)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.MultipartFormData, GetEntityNameV2(), applicationId, Constants.Bits);
            VcapRequest.AddFile("application", fileBytes, applicationId.ToString());
            VcapRequest.AddBodyParameter("resources", resourcesArray);
            VcapRequest.Execute();
        }

        public Stream Download(Guid applicationId)
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, GetEntityNameV2(), applicationId, Constants.Download);
            return VcapRequest.Download();
        }

        public Resource<Application> BindRouteApplication(Guid applicationId, Guid routeId)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.Json, GetEntityNameV2(), applicationId, Constants.Route, routeId);
            return VcapRequest.Execute<Resource<Application>>();
        }

        public void UnbindRouteApplication(Guid applicationId, Guid routeId)
        {
            VcapRequest.BuildRequest(HttpMethod.Delete, ContentTypes.Json, GetEntityNameV2(), applicationId, Constants.Route, routeId);
            VcapRequest.Execute();
        }

        public Resource<Application> Create(string name, Guid stackId, Guid spaceId, long memory, int numerInstance)
        {
            if (EntityExists(name))
            {
                throw new VcapException();
            }

            var applicationManifest = new ApplicationManifest
                {
                    Name = name,
                    StackGuid = stackId,
                    SpaceGuid = spaceId,
                    Memory = memory,
                    NumberInstance = numerInstance
                };

            return Create(applicationManifest);
        }

        public IEnumerable<StatInfo> GetStats(Guid applicationId)
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, GetEntityNameV2(), applicationId, Constants.Stats);
            var statInfos = VcapRequest.Execute<Dictionary<int, StatInfo>>();
            var result = new List<StatInfo>();
            foreach (KeyValuePair<int, StatInfo> statInfo in statInfos)
            {
                StatInfo si = statInfo.Value;
                si.StatInfoId = statInfo.Key;
                result.Add(si);
            }
            return result;
        }

        public IEnumerable<InstanceDetail> GetInstances(Guid applicationId)
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, GetEntityNameV2(), applicationId, Constants.Instances);
            var instanceDetails = VcapRequest.Execute<Dictionary<int, InstanceDetail>>();
            return instanceDetails.Values;
        }

        public IEnumerable<Crashlog> GetCrashlogs(Guid applicationId)
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, GetEntityNameV2(), applicationId, Constants.Crash);
            return VcapRequest.Execute<IEnumerable<Crashlog>>();
        }

        public IEnumerable<Resource<ApplicationEvent>> GetEvents(Application application)
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, application.EventsUrl);
            return VcapRequest.Execute<ResponseData<ApplicationEvent>>().Resources;
        }

        public IEnumerable<Resource<Application>> GetApplicationsBySpace(Space space)
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, space.AppsUrl);
            return VcapRequest.Execute<ResponseData<Application>>().Resources;
        }

        public Resource<Application> SetApplicationEnvironmentVariables(Guid applicationId, KeyValuePair<string, string> variable)
        {
            var resource = GetById(applicationId);
            JToken token;
            if (resource.Entity.EnvironmentInfo.TryGetValue(variable.Key, out token))
            {
                throw new VcapException("");
            }
            resource.Entity.EnvironmentInfo.Add(variable.Key, variable.Value);
            return Update(resource);
        }

        public Resource<Application> UnsetApplicationEnvironmentVariables(Guid applicationId, string keyVariable)
        {
            var resource = GetById(applicationId);
            JToken token;
            if (!resource.Entity.EnvironmentInfo.TryGetValue(keyVariable, out token))
            {
                throw new VcapException("");
            }
            resource.Entity.EnvironmentInfo.Remove(keyVariable);
            return Update(resource);
        }


        public string GetApplicationEnvironmentVariables(Guid applicationId)
        {
            var resource = GetById(applicationId);

            return resource.Entity.EnvironmentInfo.ToString().Replace(Environment.NewLine, string.Empty);
        }
    }
}
