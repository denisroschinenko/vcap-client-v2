using System;
using System.IO;
using System.Net.Http;
using IronFoundry.VcapClient.V2.Models;
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

            var tempDirectoryPath = StableDataStorage.CopyProjectToTempDirectory(projectPath);
            var resources = StableDataStorage.FilteringResources(tempDirectoryPath, CheckResources);
            byte[] fileBytes = StableDataStorage.CreateZipFile(tempDirectoryPath);

            UploadApplicationBits(resource, fileBytes, resources);

            resource = StartApplication(resource.Metadata.ObjectId);
            return resource;
        }

        private ResourceFile[] CheckResources(ResourceFile[] resourcesArray)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.Json, GetEntityNameV2(Constants.ResourceMatch));
            VcapRequest.AddBodyParameter("resources", resourcesArray);
            return VcapRequest.Execute<ResourceFile[]>();
        }

        private void UploadApplicationBits(Resource<Application> application, byte[] fileBytes, ResourceFile[] resourcesArray)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.MultipartFormData, GetEntityNameV2(), application.Metadata.ObjectId, Constants.Bits);
            VcapRequest.AddFile("application", fileBytes, application.Metadata.ObjectId.ToString());
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
    }
}
