// -----------------------------------------------------------------------
// <copyright file="IWebRequestFactory.cs" company="SimpleBrowser">
// Copyright © 2010 - 2019, Nathan Ridley and the SimpleBrowser contributors.
// See https://github.com/SimpleBrowserDotNet/SimpleBrowser/blob/master/readme.md
// </copyright>
// -----------------------------------------------------------------------

namespace SimpleBrowser.Network
{
    using System;
    using System.Security.Cryptography.X509Certificates;

    // TODO Review
    //   1) consider adding XML comments (documentation) to all public members

    
    public interface IWebRequestFactory
    {
        IHttpWebRequest GetWebRequest(Uri url);
    }

    public class DefaultRequestFactory : IWebRequestFactory
    {
        private X509Certificate _cert;

        public DefaultRequestFactory()
        {
        }

        public DefaultRequestFactory(X509Certificate cert)
        {
            _cert = cert;        
        }

        public IHttpWebRequest GetWebRequest(Uri url)
        {
            if(_cert == null)
                return new WebRequestWrapper(url);
            else
                return new WebRequestWrapper(url, _cert);
        }
    }
}