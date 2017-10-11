using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMCGWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult InformationalKiosks()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GenericInformationalKiosk(int id)
        {
            ViewBag.Message = string.Format("Put information here about the Informational Kiosk #{0}", id);

            return View();
        }

        public ActionResult WayFindingSigns()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GenericWayFindingSign(int id)
        {
            ViewBag.Message = string.Format("Put information here about the Way Finding Sign #{0}", id);

            return View();
        }


    }
}