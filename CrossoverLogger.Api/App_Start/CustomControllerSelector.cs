using CrossoverLogger.Commons;
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
                    controllerName = string.Concat(defaultControllerName, Constants.Others.API_DEFAULT_VERSION);
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
            if (request.Headers.Contains(Constants.HttpHeader.API_VERSION))
            {
                var versionHeader = request.Headers.GetValues(Constants.HttpHeader.API_VERSION).FirstOrDefault();
                if (versionHeader != null)
                {
                    return versionHeader;
                }
            }

            return "v1";
        }
    }
}