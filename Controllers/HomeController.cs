using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UtvecklartestAgioMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "HR-avdelningen har fått en webbsida som visar alla anställda.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontakta mig";

            return View();
        }
    }
}