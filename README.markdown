This project aims to provide functional controller tests to asp.net applications

98% of the work in it came from Steve Sanderson http://blog.stevensanderson.com/2009/06/11/integration-testing-your-aspnet-mvc-application/

I couldn't find anywhere that the code was hosted, so I am putting it out here on github and have added some things to make it feel a little more like rails.

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
    
    
      Post("registration/create", new
            {
                Form = new
                {
                    Email = "yogibear@jellystone.forest",
                    Password = "welcome",
                    ConfirmPassword = "welcome"
                }
            });