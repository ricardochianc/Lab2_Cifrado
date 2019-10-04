using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab2_Cifrado.Controllers
{
    public class Serie1Controller : Controller
    {
        public ActionResult SelecccionCifrado()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SeleccionCifrado(FormCollection formCollection)
        {
            if (formCollection["ZigZag"] != null)
            {
                return RedirectToAction("IndexZigZag", "ZigZag");
            }

            if (formCollection["Espiral"] != null)
            {
                return RedirectToAction("IndexEspiral", "Espiral");
            }

            if (formCollection["César"] != null)
            {
                return RedirectToAction("IndexZigZag", "ZigZag");
            }
            return null;
        }
    }
}
