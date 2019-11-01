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
            if (Data.Instancia.Errores.Count == 0)
            {
                Data.Instancia.Errores.Add("P y Q deben de ser mayores a cero y mayores a 16, tampoco pueden ser iguales");
                Data.Instancia.Errores.Add("El valor de P y Q deben ser números primos");
                Data.Instancia.Errores.Add("P y Q no son coprimos entre ellos");
                Data.Instancia.Errores.Add("Ocurrió un error inesperado al generar las llaves");
            }

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

            if (formCollection["Continuar"] != null)
            {
                return RedirectToAction("IndexRSA", "RSA");
            }

            return null;
        }

    }
}
