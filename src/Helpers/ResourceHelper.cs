using System.Reflection;
using System.IO;

namespace Finoaker.Web.Recaptcha
{
    internal static class ResourceHelper
    {
        internal static string GetEmbeddedResource(string resourceName, Assembly assembly)
        {
            resourceName = FormatResourceName(assembly, resourceName);
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                    return null;

                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static string FormatResourceName(Assembly assembly, string resourceName)
        {
            return assembly.GetName().Name + "." + resourceName
                .Replace(" ", "_")
                .Replace("\\", ".")
                .Replace("/", ".");
        }
    }
}
