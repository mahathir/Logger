using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace CrossoverLogger.Api.Filters
{
    public class CrossoverAuthIdentity : GenericIdentity
    {
        /// <summary>
        /// Get/Set for password
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Get/Set for UserName
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// Basic Authentication Identity Constructor
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public CrossoverAuthIdentity(string token)
            : base(token)
        {
            AccessToken = token;
        }
    }
}