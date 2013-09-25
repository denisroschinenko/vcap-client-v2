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

        protected override string Constant
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

        public Resource<Application> PushApplication(ApplicationManifest application, string projectPath)
        {
            if (string.IsNullOrWhiteSpace(projectPath))
            {
                throw new ArgumentException("Path must be entered");
            }
            if (string.IsNullOrWhiteSpace(application.Name))
            {
                throw new ArgumentException("Name must be entered");
            }
            if (Guid.Empty == application.SpaceGuid)
            {
                throw new ArgumentException("Space must be entered");
            }
            if (Guid.Empty == application.StackGuid)
            {
                throw new ArgumentException("Stack must be entered");
            }
            if (application.Memory == 0)
            {
                throw new ArgumentException("Memory must be entered");
            }
            if (application.NumberInstance == 0)
            {
                throw new ArgumentException("Instance must be entered");
            }

            var resource = Create(application);

            var tempDirectoryPath = StableDataStorage.CopyProjectToTempDirectory(projectPath);
            var resources = StableDataStorage.FilteringResources(tempDirectoryPath, CheckResources);
            byte[] fileBytes = StableDataStorage.CreateZipFile(tempDirectoryPath);

            UploadApplicationBits(resource, fileBytes, resources);

            resource = StartApplication(resource.Metadata.ObjectId);
            return resource;
        }

        public Resource<Application> BindRouteApplication(Guid applicationId, Guid routeId)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.Json, V2, Constant, applicationId, Constants.Route, routeId);
            return VcapRequest.Execute<Resource<Application>>();
        }

        private ResourceFile[] CheckResources(ResourceFile[] resourcesArray)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.Json, V2, Constants.ResourceMatch);
            VcapRequest.AddBodyParameter("resources", resourcesArray);
            return VcapRequest.Execute<ResourceFile[]>();
        }

        private void UploadApplicationBits(Resource<Application> application, byte[] fileBytes, ResourceFile[] resourcesArray)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.MultipartFormData, V2, Constant, application.Metadata.ObjectId, Constants.Bits);
            VcapRequest.AddFile("application", fileBytes, application.Metadata.ObjectId.ToString());
            VcapRequest.AddBodyParameter("resources", resourcesArray);
            VcapRequest.Execute();
        }

        public Stream Download(Guid applicationId)
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, V2, Constants.Application, applicationId, Constants.Download);
            return VcapRequest.Download();
        }
    }
}
