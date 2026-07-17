using System.Globalization;
using System.Threading;

namespace BugNET.Common
{
    public enum GlobalResources
    {
        SharedResources,
        Exceptions,
        Notifications
    }

    public static class ResourceStrings
    {
        /// <summary>
        /// Gets an application-level resource string based on the specified ClassKey and ResourceKey properties
        /// </summary>
        /// <param name="classKey">A string that represents the ClassKey property of the requested resource object.</param>
        /// <param name="resourceKey">A string that represents the ResourceKey property of the requested resource object.</param>
        /// <param name="defaultValue">A string the represents a default value if the resource cannot be found.</param>
        /// <returns></returns>
        public static string GetGlobalResource(GlobalResources classKey, string resourceKey, string defaultValue = "")
        {
            var cultureInfo = Thread.CurrentThread.CurrentUICulture;
            return GetGlobalResource(classKey, resourceKey, cultureInfo, defaultValue);
        }

        public static string GetGlobalResource(GlobalResources classKey, string resourceKey, CultureInfo culture, string defaultValue = "")
        {
            // System.Web.HttpContext is not available in .NET 10 outside of ASP.NET Core.
            // Resource lookup defers to the default value in this context.
            return defaultValue;
        }

        /// <summary>
        /// Gets a page-level resource object based on the specified VirtualPath and ResourceKey properties.
        /// </summary>
        /// <param name="virtualPath">The VirtualPath property for the local resource object.</param>
        /// <param name="resourceKey">A string that represents a ResourceKey property of the requested resource object.</param>
        /// <param name="defaultValue">A string the represents a default value if the resource cannot be found.</param>
        /// <returns></returns>
        public static string GetLocalResource(string virtualPath, string resourceKey, string defaultValue = "")
        {
            // System.Web.HttpContext is not available in .NET 10 outside of ASP.NET Core.
            // Resource lookup defers to the default value in this context.
            return defaultValue;
        }
    }
}
