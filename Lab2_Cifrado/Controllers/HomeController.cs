﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab2_Cifrado.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult PaginaPrincipalLab()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PaginaPrincipalLab(FormCollection formCollection)
        {
            if (formCollection["Serie1"] != null)
            {
                return RedirectToAction("Index", "ZigZag");
            }

            if (formCollection["Serie2"] != null)
            {
                return RedirectToAction("PaginaPrincipalLab");
            }

            if (formCollection["Serie3"] != null)
            {
                return RedirectToAction("PaginaPrincipalLab");
            }
            return null;
        }
    }
}