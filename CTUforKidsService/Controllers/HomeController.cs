using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTUforKidsService.Controllers
{
    public class HomeController : Controller
    {
        //Simple Rest API that will act as the service,
        //querying and updating data in the database.
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
