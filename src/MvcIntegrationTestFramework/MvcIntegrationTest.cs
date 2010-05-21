using System.Collections.Specialized;
using System.Web.Routing;
using System;
using MvcIntegrationTestFramework.Hosting;
using System.IO;
using MvcIntegrationTestFramework.Browsing;
using System.Web.Mvc;
namespace MvcIntegrationTestFramework
{
    public class MvcControllerTest
    {
        private AppHost _appHost;
        
        private bool IsInitialized() 
        {
            return _appHost != null;
        }

        /// <summary>
        /// Check this guy after you have invoked your web app
        /// </summary>
        protected MvcTestResponse Response { get; set; }


        /// <summary>
        /// Initializes the ASP net runtime.
        /// </summary>
        /// <param name="pathToYourWebProject">
        /// The path to your web project. This is optional if you don't
        /// specify we try to guess that it is in the first directory like
        /// ../../../*/web.config
        /// </param>
        /// <remarks>
        /// Has been known to cause severe damage to your immortal soul.
        /// </remarks>
        protected void InitializeAspNetRuntime(string pathToYourWebProject = null) 
        {
            if(pathToYourWebProject == null) 
            {
                var guessDirectory = new DirectoryInfo(
                                        Path.GetFullPath(
                                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"..","..","..")));

                var projectDirs = guessDirectory.GetDirectories();
                foreach(var pd in projectDirs) 
                {
                    if(pd.GetFiles("web.config").Length == 1) 
                    {
                        pathToYourWebProject = pd.FullName;
                        continue;
                    }
                }
            }

            var ourDll = Path.Combine(pathToYourWebProject,"bin","MvcIntegrationTestFramework.dll");
            if(!File.Exists(ourDll)) 
            {
                File.Copy( Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"MvcIntegrationTestFramework.dll"),ourDll);
            }

            _appHost = new AppHost(pathToYourWebProject, "/__test");
        }

        /// <summary>
        /// Sends a post to your url. Url should NOT start with a /
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formData"></param>
        /// <example>
        /// <code>
        /// Post("registration/create", new
        /// {
        ///     Form = new
        ///     {
        ///         InvoiceNumber = "10000",
        ///         AmountDue = "10.00",
        ///         Email = "chriso@innovsys.com",
        ///         Password = "welcome",
        ///         ConfirmPassword = "welcome"
        ///     }
        /// });
        /// </code>
        /// </example>
        protected void Post(string url, object formData)
        {
            EnsureInitialized();
            var myResponse = new MvcTestResponse();
            var formNameValueCollection = NameValueCollectionConversions.ConvertFromObject(formData);

           
            _appHost.SimulateBrowsingSession(browser =>
            {
                RequestResult result = browser.ProcessRequest(url, HttpVerbs.Post, formNameValueCollection);
                myResponse.StatusCode = result.Response.StatusCode;
            });
            Response = myResponse;
        }

        private void EnsureInitialized()
        {
            if(!IsInitialized()) 
            {
                throw new ApplicationException("You need to call that there initialize method first dude.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        protected void Get(string url) 
        {
            EnsureInitialized();

            var myResponse = new MvcTestResponse();
                                   
            _appHost.SimulateBrowsingSession(browser =>
            {
                RequestResult result = browser.ProcessRequest(url, HttpVerbs.Get,new NameValueCollection());
                myResponse.StatusCode = result.Response.StatusCode;
            });

            Response = myResponse;
        }
    }

    public static class NameValueCollectionConversions
    {
        public static NameValueCollection ConvertFromObject(object anonymous)
        {
           var nvc =  new NameValueCollection();
            var dict = new RouteValueDictionary(anonymous);
           
            foreach(var kvp in dict)
            {
                if(kvp.Value.GetType().Name.Contains("Anonymous"))
                {
                    var prefix = kvp.Key + ".";
                    foreach(var innerkvp in new RouteValueDictionary(kvp.Value))
                    {
                        nvc.Add(prefix + innerkvp.Key,innerkvp.Value.ToString());
                    }
                }
                else
                {
                    nvc.Add(kvp.Key,kvp.Value.ToString());
                }
                
                
            }
            return nvc;
        }
    }

    /// <summary>
    /// Use this to get data back to the test
    /// </summary>
    [Serializable]
    public class MvcTestResponse : MarshalByRefObject 
    {
        public int StatusCode { get; set; }
    }
}