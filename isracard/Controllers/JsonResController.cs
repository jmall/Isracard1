using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace isracard.Controllers
{
    public class JsonResController : Controller
    {
        public List<string> JsonRes = new List<string>();
        //
        // GET: /JsonRes/

        public ActionResult Index()
        {
            return View();
        }

        public object idHandler(string idStr)
        {
            return null;
        }

        public object search (string search)
        {
            string url = "https://api.github.com/search/repositories?q=" + search;
            return Redirect("JsonRes");
        }
    }
}
