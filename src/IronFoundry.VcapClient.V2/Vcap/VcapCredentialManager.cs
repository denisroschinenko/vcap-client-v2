using System;
using IronFoundry.VcapClient.V2.Models;

namespace IronFoundry.VcapClient.V2
{
    public class VcapCredentialManager
    {
        #region Fields

        private Uri _currentTarget;
        private Uri _loginTarget;
        private readonly TokenManager _tokenManager;
        #endregion

        #region Constructors

        public VcapCredentialManager(Uri targetUri, IStableDataStorage stableDataStorage)
        {
            _currentTarget = targetUri;
            _tokenManager = new TokenManager(stableDataStorage);
        }

        #endregion

        #region Properties

        public Uri CurrentTarget
        {
            get { return _currentTarget ?? Constants.DefaultLocalTarget; }
        }

        public Uri LoginTarget
        {
            get { return _loginTarget ?? Constants.LoginTarget; }
        }

        public bool HasToken
        {
            get { return CurrentToken != null && !string.IsNullOrWhiteSpace(CurrentToken.Token); }
        }

        public AccessToken CurrentToken
        {
            get
            {
                return _tokenManager.GetToken(CurrentTarget);
            }
        }

        #endregion

        public void RegisterToken(AccessToken token, Uri currentTarget)
        {
            _tokenManager.RegisterToken(token, currentTarget);
        }

        public void SetLoginUri(Uri loginUri)
        {
            _loginTarget = loginUri;
        }
    }


}

