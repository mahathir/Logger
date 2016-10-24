using CrossoverLogger.Commons.Translation;
using CrossoverLogger.DTO;
using CrossoverLogger.IBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Filters;

namespace CrossoverLogger.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CrossoverAuthFilter : AuthorizationFilterAttribute
    {
        /// &lt;summary>
        /// Public default Constructor
        /// &lt;/summary>
        public CrossoverAuthFilter()
        {
        }

        /// &lt;summary>
        /// Checks basic authentication request
        /// &lt;/summary>
        /// &lt;param name="filterContext">&lt;/param>
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            var identity = FetchAuthHeader(filterContext);
            if (identity == null)
            {
                ChallengeAuthRequest(filterContext);
                return;
            }
            var genericPrincipal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = genericPrincipal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = genericPrincipal;
            }
            if (!OnAuthorizeUser(identity.Name, filterContext))
            {
                ChallengeAuthRequest(filterContext);
                return;
            }

            if (IsRateLimitExceed(filterContext, identity.ApplicationId))
            {
                ChallengeAuthRequest(filterContext, MessagesResx.RateLimitExceed);
                return;
            }

            base.OnAuthorization(filterContext);
        }

        private bool IsRateLimitExceed(HttpActionContext filterContext, string appId)
        {
            var currentCallTime = DateTime.UtcNow;
            var rateService = filterContext.ControllerContext.Configuration
                                  .DependencyResolver.GetService(typeof(IRateLimitService)) as IRateLimitService;
            var result = rateService.MakeRequest(appId);
            return result.Result > 0;
        }

        /// &lt;summary>
        /// Virtual method.Can be overriden with the custom Authorization.
        /// &lt;/summary>
        /// &lt;param name="user">&lt;/param>
        /// &lt;param name="pass">&lt;/param>
        /// &lt;param name="filterContext">&lt;/param>
        /// &lt;returns>&lt;/returns>
        protected virtual bool OnAuthorizeUser(string tokenStr, HttpActionContext filterContext)
        {
            var tokenService = filterContext.ControllerContext.Configuration
                                  .DependencyResolver.GetService(typeof(ITokenService)) as ITokenService;
            var token = tokenService.Retrieve(tokenStr);
            if (token != null)
            {
                var identity = Thread.CurrentPrincipal.Identity as CrossoverAuthIdentity;
                if (identity != null)
                    identity.ApplicationId = token.ApplicationId;
                return true;
            }
            return false;
        }

        /// &lt;summary>
        /// Checks for autrhorization header in the request and parses it, creates user credentials and returns as CrossoverAuthIdentity
        /// &lt;/summary>
        /// &lt;param name="filterContext">&lt;/param>
        protected virtual CrossoverAuthIdentity FetchAuthHeader(HttpActionContext filterContext)
        {
            string token = null;
            var authRequest = filterContext.Request.Headers.Authorization;
            if (authRequest != null)
                token = authRequest.Scheme;
            if (string.IsNullOrEmpty(token))
                return null;
            return string.IsNullOrEmpty(token) ? null : new CrossoverAuthIdentity(token);
        }


        /// &lt;summary>
        /// Send the Authentication Challenge request
        /// &lt;/summary>
        /// &lt;param name="filterContext">&lt;/param>
        private static void ChallengeAuthRequest(HttpActionContext filterContext, string message = null)
        {
            var dnsHost = filterContext.Request.RequestUri.DnsSafeHost;
            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Forbidden, message);
        }
    }
}