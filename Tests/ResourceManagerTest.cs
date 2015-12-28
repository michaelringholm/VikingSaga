using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Packaging;

namespace Tests
{
    [TestClass]
    public class ResourceManagerTest
    {
        [TestInitialize]
        public void Setup()
        {
            string s = System.IO.Packaging.PackUriHelper.UriSchemePack;
            PackUriHelper.Create(new Uri("reliable://0"));
            System.Windows.Application.ResourceAssembly = typeof(App).Assembly;
        }

        [TestMethod]
        public void CheckResources()
        {
            ResourceManager.CheckResources();
        }
    }
}
