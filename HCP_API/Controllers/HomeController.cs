﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HCP_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult FIRST()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
