using System;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MyLib.Web.Helpers
{
    public static class ApplicationHelper
    {
        /// <summary>
        /// Return current assembly
        /// </summary>
        /// <returns></returns>
        private static Assembly GetAssembly()
        {
            HttpApplication instance = System.Web.HttpContext.Current.ApplicationInstance;
            Type baseType = instance.GetType().BaseType;
            if (baseType == null) return Assembly.GetExecutingAssembly();
            Assembly assembly = baseType.Assembly;
            return assembly;
        }

        /// <summary>
        /// Return version of the current web app
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            Assembly assembly = GetAssembly();

            String version = assembly.GetName().Version.ToString();

            String productName =
                assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)
                    .Cast<AssemblyProductAttribute>()
                    .Select(c => c.Product)
                    .FirstOrDefault();


            return productName + " - " + version;
        }
    }
}
