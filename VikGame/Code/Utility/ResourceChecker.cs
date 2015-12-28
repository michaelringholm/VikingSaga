using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Resources;

namespace Vik.Code.Utility
{
    public class ResourceChecker
    {
        // Resource     : Files added to VS and compiled into an assembly
        // Content      : Separate files on disk, added to VS and copied to app on build.
        // SiteOfOrigin : Separate files on disk on a path relative to the app. Not (necessarily) added to VS

        public enum ResourceType { Resource, Content, SiteOfOrigin }

        public class ResourceEntry
        {
            public ResourceEntry(ResourceType type, string path)
            {
                this.Type = type;
                this.Path = path;
            }

            public ResourceType Type { get; private set; }
            public string Path { get; private set; }
        }

        private readonly List<ResourceEntry> _resourceFiles = new List<ResourceEntry>();
        private readonly List<ResourceEntry> _contentFiles = new List<ResourceEntry>();
        private readonly List<ResourceEntry> _siteOfOriginFiles = new List<ResourceEntry>();

        public ResourceChecker(string siteOfOriginPath)
        {
            _resourceFiles = GetResourceFiles().ToList();
            _contentFiles = GetContentFiles().ToList();
            //_siteOfOriginFiles = GetSiteOfOriginFiles(siteOfOriginPath).ToList();
        }

        private IEnumerable<ResourceEntry> AllEntries()
        {
            return _resourceFiles.Concat(_contentFiles.Concat(_siteOfOriginFiles));
        }

        public string DebugInfo(string resourceName)
        {
            string result = string.Empty;
            resourceName = resourceName.ToLowerInvariant();

            var matches = AllEntries().Where(e => Path.GetFileName(e.Path) == Path.GetFileName(resourceName) || e.Path == resourceName || e.Path.Contains(resourceName));

            if (matches.Count() == 0)
                result = "No matches found: " + resourceName;

            foreach(var match in matches)
            {
                result += string.Format("Resource type: <{0}>, Path: {1}{2}", match.Type, match.Path, Environment.NewLine);
            }

            return result;
        }

        public static IEnumerable<ResourceEntry> GetResourceFiles()
        {
            var assembly = Assembly.GetCallingAssembly();
            var resourcesName = assembly.GetName().Name + ".g.resources";
            var stream = assembly.GetManifestResourceStream(resourcesName);
            var resourceReader = new ResourceReader(stream);

            var result = resourceReader.OfType<DictionaryEntry>().Select(de => new ResourceEntry(ResourceType.Resource, (string)de.Key));
            return result;
        }

        public static IEnumerable<ResourceEntry> GetContentFiles()
        {
            var assembly = Assembly.GetEntryAssembly();
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyAssociatedContentFileAttribute), true).Cast<AssemblyAssociatedContentFileAttribute>();
            var result = attributes.Select(a => new ResourceEntry(ResourceType.Content, a.RelativeContentFilePath));
            return result;
        }

        public static IEnumerable<ResourceEntry> GetSiteOfOriginFiles(string siteOfOriginPath)
        {
            var result = Directory.GetFiles(siteOfOriginPath, "*", SearchOption.AllDirectories);
            return result.Select(f => new ResourceEntry(ResourceType.SiteOfOrigin, f.ToLowerInvariant()));
        }
    }
}
