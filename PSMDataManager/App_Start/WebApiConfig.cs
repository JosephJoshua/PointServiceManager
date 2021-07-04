using Microsoft.Owin.Security.OAuth;
using PSMDataManager.FilterAttributes;
using System.Web.Http;

namespace PSMDataManager
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Configure Web API to return any thrown API exceptions as a response
            config.Filters.Add(new ApiExceptionFilterAttribute());

            // Configure Web API to validate model state before any request
            config.Filters.Add(new ValidationActionFilterAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
