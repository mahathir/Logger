using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace CrossoverLogger.Api
{
    public class CustomControllerSelector : DefaultHttpControllerSelector
    {
        private HttpConfiguration _config;
        private const string HEADER_NAME = "X-CrossoverLogger-Version";
        private const string DEFAULT_VERSION = "V1";

        public CustomControllerSelector(HttpConfiguration config) : base(config)
        {
            _config = config;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            try
            {
                var controllers = GetControllerMapping();
                var routeData = request.GetRouteData();

                var defaultControllerName = routeData.Values["controller"].ToString();
                HttpControllerDescriptor controllerDescriptor;

                string versionNum = GetVersionFromHeader(request).ToUpper();

                var controllerName = string.Concat(defaultControllerName, versionNum);
                if (controllers.TryGetValue(controllerName, out controllerDescriptor))
                {
                    return controllerDescriptor;
                }
                else
                {
                    controllerName = string.Concat(defaultControllerName, DEFAULT_VERSION);
                    if (controllers.TryGetValue(controllerName, out controllerDescriptor))
                    {
                        return controllerDescriptor;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetVersionFromHeader(HttpRequestMessage request)
        {
            if (request.Headers.Contains(HEADER_NAME))
            {
                var versionHeader = request.Headers.GetValues(HEADER_NAME).FirstOrDefault();
                if (versionHeader != null)
                {
                    return versionHeader;
                }
            }

            return "v1";
        }
    }
}