using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Example.MsTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1 : MvcIntegrationTestFramework.MvcControllerTest
    {
        public UnitTest1()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        [TestInitialize]
        public void Setup() 
        {
            InitializeAspNetRuntime();

            Get("home/index");
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(200,Response.StatusCode);
        }
    }
}
