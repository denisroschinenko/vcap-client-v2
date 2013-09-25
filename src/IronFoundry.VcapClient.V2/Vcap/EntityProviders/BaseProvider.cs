
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using IronFoundry.VcapClient.V2.Models;
using PortableRest;

namespace IronFoundry.VcapClient.V2
{
    internal abstract class BaseProvider<T, U>
    {
        protected readonly VcapCredentialManager CredentialManager;
        protected readonly VcapRequest VcapRequest;
        private readonly string _queryParameter = "q=";
        private readonly string _inlineRelation = "inline-relations-depth=";

        protected abstract string Constant { get; }

        protected readonly string V2 = "v2";

        protected BaseProvider(VcapCredentialManager credentialManager, bool isLogin)
        {
            CredentialManager = credentialManager;
            VcapRequest = new VcapRequest(CredentialManager, isLogin);
        }

        protected BaseProvider(VcapCredentialManager credentialManager)
            : this(credentialManager, false) { }

        public virtual IEnumerable<Resource<T>> GetAll()
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, V2, Constant);
            return VcapRequest.Execute<ResponseData<T>>().Resources;
        }

        public virtual Resource<T> GetById(Guid entityId)
        {
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, V2, Constant, entityId);
            return VcapRequest.Execute<Resource<T>>();
        }

        public virtual Resource<T> GetByParam(string paramName, object paramValue)
        {
            var args = BuildFilteringArgs(new KeyValuePair<string, object>(paramName, paramValue));
            VcapRequest.BuildRequest(HttpMethod.Get, ContentTypes.Json, V2, args);
            var responceData = VcapRequest.Execute<ResponseData<T>>();
            return responceData.Resources.SingleOrDefault();
        }

        public virtual Resource<T> Create(U entity)
        {
            VcapRequest.BuildRequest(HttpMethod.Post, ContentTypes.Json, V2, Constant);
            VcapRequest.AddBodyParameter(Constant, entity);
            return VcapRequest.Execute<Resource<T>>();
        }

        public virtual Resource<T> Update(Resource<T> resource)
        {
            VcapRequest.BuildRequest(HttpMethod.Put, ContentTypes.Json, V2, Constant, resource.Metadata.ObjectId);
            VcapRequest.AddBodyParameter(Constant, resource.Entity);
            return VcapRequest.Execute<Resource<T>>();
        }

        public virtual void Delete(Guid entityId)
        {
            VcapRequest.BuildRequest(HttpMethod.Delete, ContentTypes.Json, V2, Constant, entityId);
            VcapRequest.Execute<Resource<T>>();
        }

        #region Auxillary methods

        private string BuildArgs()
        {
            return null;
        }

        private string BuildFilteringArgs(KeyValuePair<string, object> param)
        {
            string args = string.Format("{0}?{1}{2}:{3}", Constant, _queryParameter, param.Key, param.Value);
            return args;
        }

        private string BuildRelationshipsArgs(int depth)
        {
            return string.Format("{0}{1}", _inlineRelation, depth);
        }
        #endregion
    }
}
