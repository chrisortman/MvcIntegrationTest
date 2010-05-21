using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Example.Xunit
{
    public class SampleXunitTest : MvcIntegrationTestFramework.MvcControllerTest
    {
        public SampleXunitTest() 
        {
            InitializeAspNetRuntime();

            Get("home/index");
        }

        [Fact]
        public void Should_be_successful() 
        {
            Assert.Equal(200,Response.StatusCode);
        }
    }
}
