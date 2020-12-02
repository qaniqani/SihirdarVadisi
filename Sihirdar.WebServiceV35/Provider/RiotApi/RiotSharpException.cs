using System;
using System.Net;

namespace Sihirdar.WebServiceV3.Provider.RiotApi
{
    /// <summary>
    /// RiotSharp exception.
    /// </summary>
    public class RiotSharpException: Exception
    {
        /// <summary>HTTP error code returned by the Riot API, causing this exception.</summary>
        public readonly HttpStatusCode HttpStatusCode;

        public RiotSharpException(string message, HttpStatusCode httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
