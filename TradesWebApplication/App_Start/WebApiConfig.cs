
using System.Web.Http;

class WebApiConfig
{
    public static void Register(HttpConfiguration configuration)
    {

       

        configuration.Routes.MapHttpRoute(
            name: "ActionApi",
            routeTemplate: "api/{controller}/{action}/{id}",
            defaults: new { controller = "Values", action = RouteParameter.Optional, id = RouteParameter.Optional });

        configuration.Routes.MapHttpRoute(
            name: "Optional parameters route",
            routeTemplate: "api/{controller}/{action}/"
            );

        configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}",
           new { id = RouteParameter.Optional });
        
    }
}