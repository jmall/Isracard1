using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace isracard.Controllers
{
    public class HomeController : Controller
    {
        List<string> jsonElements = new List<string>();
        public ActionResult Index()
        {
            ViewBag.Message = "The application of Meir Plevinski";

            return View();
        }

        public ActionResult ModalAction(int Id)
        {
            ViewBag.Id = Id;
            return PartialView("ModalContent");
        }

        public object details (string search)
        {
            Session["ID"] = search;
            return View("Details");

        }

        // -------------------------------------
        // זו הפונקציה הראשית של החיפוש
        // -------------------------------------
        public object search (string search)
        {
            string url = "https://api.github.com/search/repositories?q=" + search;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;



            string sHtml = "";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request;
            HttpWebResponse response = null;
            Stream stream = null;
            request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Foo";
            request.Accept = "*/*";

            if (request.Proxy != null)
            {
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }
            response = (HttpWebResponse)request.GetResponse();

            stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.Default);
            sHtml = sr.ReadToEnd();
            if (stream != null) stream.Close();
            if (response != null) response.Close();


            Session["JsonRes"] = sHtml;//Store value in tempdata  

            dynamic stuff1 = Newtonsoft.Json.JsonConvert.DeserializeObject(sHtml);


            //extractDynamic(stuff1);
            extractDynamic(stuff1);
            //foreach (JProperty element in stuff1.Children())
            //{
            //    string propName = element.Name;
            //    var propVal = (string)element.Value;
            //};
        
            //string items = stuff1.items;

            return Redirect("/home/JsonRes");

                
        }

      
        public void extractDynamic(dynamic rec)
        {
            jsonElements = new List<string>();
            Newtonsoft.Json.Linq.JArray items = rec.items;
            bool idField = false;

            foreach (dynamic elementArray in items)
            {
                var propVal = "";
                string propName = "";
                foreach (Object element1 in elementArray)
                {
                    dynamic element = element1;
                    
                    idField = false;
                    if (element.Name == "id")
                    {
                        propName = element.Name;
                        idField = true;
                    }
                    //System.Diagnostics.Trace.WriteLine(element.Value.GetType());


                    if (element.Value.GetType() != typeof(Newtonsoft.Json.Linq.JValue))
                    {
                        extractDynamic1(element.Value);
                    }
                    else
                    {
                        if (idField == true)
                            propVal = (string)element.Value;
                        }
                    }
                jsonElements.Add(propName + ":" + propVal);
            }
            Session["JsonStr"] = jsonElements;
        }

        public void extractDynamic1(dynamic rec)
        {
            foreach (JProperty elementArray in rec)
            {
                //foreach (Object element1 in elementArray)
                {
                    dynamic element = elementArray;
                    var propVal = "";
                    string propName = element.Name;
                    //System.Diagnostics.Trace.WriteLine(element.Value.GetType());


                    if (element.Value.GetType() != typeof(Newtonsoft.Json.Linq.JValue))
                    {
                        //dynamic arr = Newtonsoft.Json.JsonConvert.DeserializeObject(element.Value.ToString());
                        extractDynamic(element.Value);
                    }
                    else
                    {
                        propVal = (string)element.Value;
                    }
                    System.Diagnostics.Trace.WriteLine(propName+":"+propVal);
                }
            }
        }
          
        public ActionResult About()
        {
            ViewBag.Message = "Meir plevunski - Who am I.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult JsonRes()
        {
            ViewBag.Message = "JsonRes.";

            return View(jsonElements);
        }
    }
}
