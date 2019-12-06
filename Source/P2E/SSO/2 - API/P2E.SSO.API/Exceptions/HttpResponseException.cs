using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace P2E.SSO.API.Controllers
{
    [Serializable]
    public class HttpResponseException : Exception
    {
        public int Status { get; set; } = 500;

        public object Value { get; set; }
    }
}