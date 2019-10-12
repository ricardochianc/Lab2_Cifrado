using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab2_Cifrado.Instancia;

namespace Lab2_Cifrado.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult PaginaPrincipalLab()
        {

            if (Data.Instancia.primeraVez)
            {
                var path = Server.MapPath("~/MisCifrados/");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                Data.Instancia.RutaAbsolutaServer = path;
                Data.Instancia.primeraVez = false;
            }

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PaginaPrincipalLab(FormCollection formCollection)
        {
            if (formCollection["Serie1"] != null)
            {
                return RedirectToAction("SelecccionCifrado", "Serie1");
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