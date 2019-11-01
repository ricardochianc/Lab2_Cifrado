using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab2_Cifrado.Instancia;

namespace Lab2_Cifrado.Controllers
{
    public class Serie3Controller : Controller
    {
        // GET: Serie3
        public ActionResult SeleccionarOpcionSerie3()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SeleccionarOpcionSerie3(FormCollection formCollection)
        {
            if (formCollection["GenerarLlaves"] != null)
            {
                Data.Instancia.GenerarLlaves = true;
                return RedirectToAction("IndexRSA", "RSA"); //mandarlo directamente a cargar archivo
            }

            //if (formCollection["Continuar"] != null)
            //{
            //    return RedirectToAction("IndexSDES", "SDES");
            //}

            return null;
        }

    }
}
