﻿using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using PortableRest;

namespace IronFoundry.VcapClient.V2
{
    public class VcapRequest
    {
        private readonly VcapCredentialManager _credentialManager;
        private readonly RestClient _client;
        private RestRequest _request;

        public VcapRequest(VcapCredentialManager credentialManager, bool isLogin, bool isAuthentication)
        {
            _credentialManager = credentialManager;
            _client = BuildClient(isLogin, isAuthentication);
        }

        public void BuildRequest(HttpMethod method, ContentTypes format, params object[] args)
        {
            _request = BuildRestRequest(method, format);
            ProcessRequestArgs(_request, args);
        }

        public TResponse Execute<TResponse>()
        {
            var tasks = _client.ExecuteAsync<TResponse>(_request);
            return tasks.Result;
        }

        public TResponse ExecuteAnonymousType<TResponse>(TResponse anonymousObject)
        {
            var tasks = _client.ExecuteAsyncAnonymousType(_request, anonymousObject);
            return tasks.Result;
        }

        public void Execute()
        {
            var task = _client.ExecuteAsync(_request);
            task.Wait();
        }

        public Stream Download()
        {
            var tasks = _client.DownloadData(_request);
            return tasks.Result;
        }

        public void AddBodyParameter(string key, object value)
        {
            _request.AddParameter(key, value);
        }

        public void AddFile(string name, byte[] bytes, string filename)
        {
            _request.AddFile(name, bytes, filename);
        }

        private RestRequest BuildRestRequest(HttpMethod method, ContentTypes format)
        {
            var rv = new RestRequest
            {
                Method = method,
                ContentType = format
            };
            return rv;
        }

        private RestClient BuildClient(bool isLogin, bool isAuthentication)
        {
            Uri currentTargetUri = isLogin ? _credentialManager.LoginTarget : _credentialManager.CurrentTarget;

            string baseUrl = currentTargetUri.AbsoluteUri;

            var rv = new RestClient
            {
                BaseUrl = baseUrl,
            };

            if (isAuthentication && _credentialManager.HasToken)
            {
                rv.AddHeader("AUTHORIZATION", string.Format("{0} {1}", _credentialManager.CurrentToken.TokenType,
                    _credentialManager.CurrentToken.Token));
            }
            else if (isLogin)
            {
                byte[] encodedBytes = Encoding.UTF8.GetBytes(Constants.DefaultLogin);
                var defaultLogin = string.Format("Basic {0}", Convert.ToBase64String(encodedBytes));
                rv.AddHeader("AUTHORIZATION", defaultLogin);
            }
            return rv;
        }

        private void ProcessRequestArgs(RestRequest request, params object[] args)
        {
            if (null == request)
            {
                throw new ArgumentNullException("request");
            }
            if (args.Any())
            {
                request.Resource = String.Join("/", args).Replace("//", "/");
            }
        }
    }
}
