using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PhotinoAPI
{
    internal static class PhotonUtil
    {
        public static string GetResourceString(Assembly assembly, string name)
        {
            var resourceNames = assembly.GetManifestResourceNames();
            var indexHtmlResource = resourceNames.FirstOrDefault((n) => n.EndsWith(name));
            if (indexHtmlResource == null) throw new Exception("Could not find 'index.html' in resources");

            using var indexHtmlStream = assembly.GetManifestResourceStream(indexHtmlResource);
            if (indexHtmlStream == null) throw new Exception("Could not get mainfest resource stream for resource 'index.html'");

            using var indexHtmlReader = new StreamReader(indexHtmlStream);

            return indexHtmlReader.ReadToEnd();
        }
    }
}
