using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab2_Cifrado.Instancia;

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
                Data.Instancia.ModificarPermutaciones = true;
                return RedirectToAction("IndexSDES", "SDES"); //mandarlo directamente a cargar archivo
            }

            if (formCollection["Continuar"] != null)
            {
                return RedirectToAction("IndexSDES", "SDES");
            }

            return null;
        }
    }
}
