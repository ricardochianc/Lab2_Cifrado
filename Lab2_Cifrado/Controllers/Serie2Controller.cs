using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab2_Cifrado.Controllers
{
    public class Serie2Controller : Controller
    {
        // GET: Serie2
        public ActionResult SeleccionarOpcion()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SeleccionarOpcion(FormCollection formCollection)
        {
            if (formCollection["CargarPermutaciones"] != null)
            {
                return RedirectToAction("", "");
            }

            if (formCollection["Continuar"] != null)
            {
                return RedirectToAction("", "");
            }

            return null;
        }
    }
}
